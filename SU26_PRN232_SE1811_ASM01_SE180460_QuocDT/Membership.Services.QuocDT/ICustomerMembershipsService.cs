using Membership.Entities.QuocDT.Models;
using Membership.Repositories.QuocDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Membership.Services.QuocDT
{
    public interface ICustomerMembershipsService
    {
        // quyery method
        Task<IEnumerable<CustomerMembershipsQuocDt>> GetAllAsync();
        Task<CustomerMembershipsQuocDt> GetByIdAsync(Guid id);
        Task<IEnumerable<CustomerMembershipsQuocDt>> SearchAsync(string customerName, int? pointsBalance, string tierName);
        // mutaion method
        Task<int> CreateAsync(CustomerMembershipsQuocDt customer);
        Task<int> UpdateAsync(CustomerMembershipsQuocDt customer);
        Task<int> DeleteAsync(Guid id);
    }
}
