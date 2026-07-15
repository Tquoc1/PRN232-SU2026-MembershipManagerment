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

        // 1. Thêm 'async', bỏ '.Result', dùng 'await' và return thẳng giá trị int
        public async Task<int> CreateAsync(CustomerMembershipsQuocDt customer)
        {
            try
            {
                var result = await _repository.CreateAsync(customer);
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        // 2. Hàm Delete
        public async Task<int> DeleteAsync(Guid id)
        {
            try
            {
                var customer = await _repository.GetByIdAsync(id);
                if (customer == null)
                    return 0;
                var result = await _repository.RemoveAsync(customer);
                return result ? 1 : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        // 3. Hàm GetAll
        public async Task<IEnumerable<CustomerMembershipsQuocDt>> GetAllAsync()
        {
            try
            {
                var customers = await _repository.GetAllAsync();
                return customers;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<CustomerMembershipsQuocDt>();
            }
        }

        // 4. Hàm GetById
        public async Task<CustomerMembershipsQuocDt> GetByIdAsync(Guid id)
        {
            try
            {
                var customer = await _repository.GetByIdAsync(id);
                return customer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // 5. Hàm Search
        public async Task<IEnumerable<CustomerMembershipsQuocDt>> SearchAsync(string? customerName, int? currentPointsBalance, string? tierName)
        {
            try
            {
                // Đổi var thành IEnumerable để nhận dữ liệu từ LINQ thoải mái
                IEnumerable<CustomerMembershipsQuocDt> query = await _repository.GetAllAsync();

                if (!string.IsNullOrWhiteSpace(customerName))
                {
                    query = query.Where(c => c.CustomerName.Contains(customerName, StringComparison.OrdinalIgnoreCase));
                }

                if (currentPointsBalance.HasValue)
                {
                    query = query.Where(c => c.CurrentPointsBalance.HasValue && c.CurrentPointsBalance.Value == currentPointsBalance.Value);
                }

                if (!string.IsNullOrWhiteSpace(tierName))
                {
                    query = query.Where(c => c.Tier != null && c.Tier.TierName.Contains(tierName, StringComparison.OrdinalIgnoreCase));
                }

                return query; // Trả về kết quả sau khi lọc thành công
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<CustomerMembershipsQuocDt>();
            }
        }

        // 6. Hàm Update
        public async Task<int> UpdateAsync(CustomerMembershipsQuocDt customer)
        {
            try
            {
                var result = await _repository.UpdateAsync(customer);
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}