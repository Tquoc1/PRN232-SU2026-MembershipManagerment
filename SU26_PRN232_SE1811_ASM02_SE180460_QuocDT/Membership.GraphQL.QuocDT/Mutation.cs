using HotChocolate;
using HotChocolate.Authorization;
using Membership.Services.QuocDT;
using Membership.Entities.QuocDT.Models;

public class Mutation
{
    public async Task<string?> Login(
        string username,
        string password,
        [Service] ISystemUserAccountService userService,
        [Service] IConfiguration config)
    {
        var user = await userService.GetUserAccount(username, password);
        if (user == null)
        {
            return null;
        }

        var jwtSettings = config.GetSection("Jwt");
        var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Secret Key is not configured.")));
        var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.UserAccountId.ToString()),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email ?? string.Empty),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.RoleId.ToString())
        };

        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
    }



    private async Task ValidateCustomerRequestAsync(CustomerMembershipsQuocDt customer, IMembershipTiersQuocDtService tierService)
    {
        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(customer, serviceProvider: null, items: null);
        var validationResults = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();

        bool isValid = System.ComponentModel.DataAnnotations.Validator.TryValidateObject(customer, validationContext, validationResults, validateAllProperties: true);

        if (!isValid)
        {
            var errors = string.Join("; ", validationResults.Select(r => r.ErrorMessage));
            throw new HotChocolate.GraphQLException(errors);
        }

        var tiers = await tierService.GetAllMembershipTiersAsync();
        if (tiers == null || !tiers.Any(t => t.TierIdquocDt == customer.TierId))
        {
            throw new HotChocolate.GraphQLException($"Tier ID {customer.TierId} does not exist.");
        }
    }

    [Authorize(Roles = new[] { "1" })]
    public async Task<int> CreateCustomerMembership(
        CustomerMembershipsQuocDt customer,
        [Service] ICustomerMembershipsService membershipService,
        [Service] IMembershipTiersQuocDtService tierService) 
    {
        await ValidateCustomerRequestAsync(customer, tierService);
        
        /*
        // Return data instead of row count:
        var result = await membershipService.CreateAsync(customer);
        return result > 0 ? customer : null;
        */

        return await membershipService.CreateAsync(customer);
    }

    [Authorize(Roles = new[] { "1" })]
    public async Task<int> UpdateCustomerMembership(
        CustomerMembershipsQuocDt customer,
        [Service] ICustomerMembershipsService membershipService,
        [Service] IMembershipTiersQuocDtService tierService)
    {
        await ValidateCustomerRequestAsync(customer, tierService);

        /*
        // Return data instead of row count:
        var result = await membershipService.UpdateAsync(customer);
        return result > 0 ? customer : null;
        */

        return await membershipService.UpdateAsync(customer);
    }

    [Authorize(Roles = new[] { "1" })]
    public async Task<bool> DeleteCustomerMembership(
        Guid id,
        [Service] ICustomerMembershipsService membershipService)
    {
        var result = await membershipService.DeleteAsync(id);
        return result > 0;
    }
}