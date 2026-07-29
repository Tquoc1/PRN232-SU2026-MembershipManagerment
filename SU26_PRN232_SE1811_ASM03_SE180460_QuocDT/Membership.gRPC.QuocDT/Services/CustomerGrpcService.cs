using Grpc.Core;
using Membership.Entities.QuocDT.Models;
using Membership.Services.QuocDT;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Membership.gRPC.QuocDT.Services
{
    public class CustomerGrpcService : CustomerService.CustomerServiceBase
    {
        private readonly ICustomerMembershipsService _membershipService;
        private readonly ISystemUserAccountService _userService;
        private readonly IConfiguration _configuration;
        private readonly IMembershipTiersQuocDtService _tierService;

        public CustomerGrpcService(
            ICustomerMembershipsService membershipService,
            ISystemUserAccountService userService,
            IConfiguration configuration,
            IMembershipTiersQuocDtService tierService)
        {
            _membershipService = membershipService;
            _userService = userService;
            _configuration = configuration;
            _tierService = tierService;
        }

        public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Username and password are required."));
            }

            var user = await _userService.GetUserAccount(request.Username, request.Password);
            if (user == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid username or password.",
                    Token = string.Empty
                };
            }

            var token = GenerateJwtToken(user);
            return new LoginResponse
            {
                Success = true,
                Message = "Login successful.",
                Token = token
            };
        }

        [Authorize]
        public override async Task<CustomerListResponse> GetAll(Empty request, ServerCallContext context)
        {
            var customers = await _membershipService.GetAllAsync();
            var response = new CustomerListResponse();

            foreach (var customer in customers)
            {
                response.Customers.Add(MapToModel(customer));
            }

            return response;
        }

        [Authorize]
        public override async Task<CustomerResponse> GetById(GetByIdRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.MembershipId, out var id))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Membership ID format."));
            }

            var customer = await _membershipService.GetByIdAsync(id);
            if (customer == null)
            {
                return new CustomerResponse
                {
                    Success = false,
                    Message = $"Customer membership with ID {id} not found."
                };
            }

            return new CustomerResponse
            {
                Success = true,
                Message = "Customer membership retrieved successfully.",
                Customer = MapToModel(customer)
            };
        }

        [Authorize]
        public override async Task<CustomerResponse> Create(CreateCustomerRequest request, ServerCallContext context)
        {
            var customer = new CustomerMembershipsQuocDt
            {
                MembershipIdquocDt = Guid.NewGuid(),
                CustomerName = request.CustomerName,
                TierId = request.TierId,
                JoinDate = ParseDateTime(request.JoinDate) ?? DateTime.Now,
                IsAutoRenewalActive = request.IsAutoRenewalActive,
                CurrentPointsBalance = request.CurrentPointsBalance,
                LastReviewDate = ParseDateTime(request.LastReviewDate),
                MonthlySpendToDate = (decimal)request.MonthlySpendToDate,
                MembershipNotes = request.MembershipNotes,
                TierExpiryDate = ParseDateOnly(request.TierExpiryDate),
                TotalLifetimeVisits = request.TotalLifetimeVisits
            };

            // Validation
            await ValidateCustomerRequestAsync(customer);

            var result = await _membershipService.CreateAsync(customer);
            if (result <= 0)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Failed to create customer membership."));
            }

            // Retrieve created object to include join/nav tier details if any
            var createdCustomer = await _membershipService.GetByIdAsync(customer.MembershipIdquocDt);
            return new CustomerResponse
            {
                Success = true,
                Message = $"Customer membership created successfully. Database affected rows: {result}.",
                Customer = MapToModel(createdCustomer ?? customer)
            };
        }

        [Authorize]
        public override async Task<CustomerResponse> Update(UpdateCustomerRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.MembershipId, out var id))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Membership ID format."));
            }

            var existing = await _membershipService.GetByIdAsync(id);
            if (existing == null)
            {
                return new CustomerResponse
                {
                    Success = false,
                    Message = $"Customer membership with ID {id} not found."
                };
            }

            // Create temporary object to validate properties via DataAnnotations
            var tempCustomer = new CustomerMembershipsQuocDt
            {
                MembershipIdquocDt = id,
                CustomerName = request.CustomerName,
                TierId = request.TierId,
                JoinDate = ParseDateTime(request.JoinDate) ?? existing.JoinDate,
                IsAutoRenewalActive = request.IsAutoRenewalActive,
                CurrentPointsBalance = request.CurrentPointsBalance,
                LastReviewDate = ParseDateTime(request.LastReviewDate),
                MonthlySpendToDate = (decimal)request.MonthlySpendToDate,
                MembershipNotes = request.MembershipNotes,
                TierExpiryDate = ParseDateOnly(request.TierExpiryDate),
                TotalLifetimeVisits = request.TotalLifetimeVisits
            };

            // Validation
            await ValidateCustomerRequestAsync(tempCustomer);

            existing.CustomerName = request.CustomerName;
            existing.TierId = request.TierId;
            existing.JoinDate = ParseDateTime(request.JoinDate);
            existing.IsAutoRenewalActive = request.IsAutoRenewalActive;
            existing.CurrentPointsBalance = request.CurrentPointsBalance;
            existing.LastReviewDate = ParseDateTime(request.LastReviewDate);
            existing.MonthlySpendToDate = (decimal)request.MonthlySpendToDate;
            existing.MembershipNotes = request.MembershipNotes;
            existing.TierExpiryDate = ParseDateOnly(request.TierExpiryDate);
            existing.TotalLifetimeVisits = request.TotalLifetimeVisits;

            var result = await _membershipService.UpdateAsync(existing);
            if (result <= 0)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Failed to update customer membership."));
            }

            // var updatedCustomer = await _membershipService.GetByIdAsync(id);
            return new CustomerResponse
            {
                Success = true,
                Message = $"Customer membership updated successfully. Database affected rows: {result}.",
                // Customer = MapToModel(updatedCustomer ?? existing)
            };
        }

        [Authorize]
        public override async Task<DeleteResponse> Delete(DeleteRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.MembershipId, out var id))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Membership ID format."));
            }

            var result = await _membershipService.DeleteAsync(id);
            if (result <= 0)
            {
                return new DeleteResponse
                {
                    Success = false,
                    Message = $"Customer membership with ID {id} not found or failed to delete. Database affected rows: {result}."
                };
            }

            return new DeleteResponse
            {
                Success = true,
                Message = $"Customer membership deleted successfully. Database affected rows: {result}."
            };
        }

        private string GenerateJwtToken(SystemUserAccount user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Secret Key is not configured.")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserAccountId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task ValidateCustomerRequestAsync(CustomerMembershipsQuocDt customer)
        {
            var validationContext = new ValidationContext(customer, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(customer, validationContext, validationResults, validateAllProperties: true);

            if (!isValid)
            {
                var errors = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
                throw new RpcException(new Status(StatusCode.InvalidArgument, errors));
            }

            var tiers = await _tierService.GetAllMembershipTiersAsync();
            if (tiers == null || !tiers.Any(t => t.TierIdquocDt == customer.TierId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Tier ID {customer.TierId} does not exist."));
            }
        }

        private CustomerModel MapToModel(CustomerMembershipsQuocDt c)
        {
            return new CustomerModel
            {
                MembershipId = c.MembershipIdquocDt.ToString(),
                CustomerName = c.CustomerName ?? string.Empty,
                TierId = c.TierId,
                JoinDate = c.JoinDate?.ToString("o") ?? string.Empty,
                IsAutoRenewalActive = c.IsAutoRenewalActive ?? false,
                CurrentPointsBalance = c.CurrentPointsBalance ?? 0,
                LastReviewDate = c.LastReviewDate?.ToString("o") ?? string.Empty,
                MonthlySpendToDate = (double)(c.MonthlySpendToDate ?? 0m),
                MembershipNotes = c.MembershipNotes ?? string.Empty,
                TierExpiryDate = c.TierExpiryDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                TotalLifetimeVisits = c.TotalLifetimeVisits ?? 0,
                TierName = c.Tier?.TierName ?? string.Empty
            };
        }

        private DateTime? ParseDateTime(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return DateTime.TryParse(value, out var dt) ? dt : null;
        }

        private DateOnly? ParseDateOnly(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return DateOnly.TryParse(value, out var d) ? d : null;
        }
    }
}
