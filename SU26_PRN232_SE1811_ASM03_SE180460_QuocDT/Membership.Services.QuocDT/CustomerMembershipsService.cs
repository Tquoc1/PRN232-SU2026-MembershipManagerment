using Membership.Entities.QuocDT.Models;
using Membership.Repositories.QuocDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Membership.Services.QuocDT
{
    public class CustomerMembershipsService : ICustomerMembershipsService
    {
        private readonly CustomerMembershipsQuocDtRepository _repository;
        public CustomerMembershipsService(CustomerMembershipsQuocDtRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task<int> CreateAsync(CustomerMembershipsQuocDt customer)
        {
            //throw new NotImplementedException();
            try
            {
                var result = _repository.CreateAsync(customer).Result;
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return Task.FromResult(0);
            }
        }

        public Task<int> DeleteAsync(Guid id)
        {
            //throw new NotImplementedException();
            try
            {
                var customer = _repository.GetByIdAsync(id).Result;
                if (customer == null)
                    return Task.FromResult(0);
                var result = _repository.RemoveAsync(customer).Result;
                return Task.FromResult(result ? 1 : 0);

            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return Task.FromResult(0);
            }
        }

        public Task<IEnumerable<CustomerMembershipsQuocDt>> GetAllAsync()
        {
            //throw new NotImplementedException();
            try
            {
                var customers = _repository.GetAllAsync().Result;
                return Task.FromResult(customers.AsEnumerable());

            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return Task.FromResult(Enumerable.Empty<CustomerMembershipsQuocDt>());
            }
        }
        public Task<CustomerMembershipsQuocDt> GetByIdAsync(Guid id)
        {
            //throw new NotImplementedException();
            try
            {
                var customer = _repository.GetByIdAsync(id).Result;
                return Task.FromResult(customer);

            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return Task.FromResult<CustomerMembershipsQuocDt>(null);
            }
        }

        public Task<IEnumerable<CustomerMembershipsQuocDt>> SearchAsync(string? customerName, int? currentPointsBalance, string? tierName)
        {
            //throw new NotImplementedException();
            try
            {
                IEnumerable<CustomerMembershipsQuocDt> customers = _repository.GetAllAsync().Result;
                if (!string.IsNullOrWhiteSpace(customerName))
                    customers = customers.Where(c => c.CustomerName.Contains(customerName, StringComparison.OrdinalIgnoreCase));
                if (currentPointsBalance.HasValue)
                    customers = customers.Where(c => c.CurrentPointsBalance.HasValue && c.CurrentPointsBalance.Value == currentPointsBalance.Value);
                if (!string.IsNullOrWhiteSpace(tierName))
                    customers = customers.Where(c => c.Tier != null && c.Tier.TierName.Contains(tierName, StringComparison.OrdinalIgnoreCase));
                return Task.FromResult(customers);

            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return Task.FromResult(Enumerable.Empty<CustomerMembershipsQuocDt>());
            }
        }

        public Task<int> UpdateAsync(CustomerMembershipsQuocDt customer)
        {
            //throw new NotImplementedException();
            try
            {
                var result = _repository.UpdateAsync(customer).Result;
                return Task.FromResult(result);

            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return Task.FromResult(0);
            }
        }
    }
}
