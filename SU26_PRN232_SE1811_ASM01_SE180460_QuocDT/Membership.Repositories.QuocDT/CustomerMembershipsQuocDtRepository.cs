using Membership.Entities.QuocDT.Models;
using Membership.Repositories.QuocDT.Base;
using Membership.Repositories.QuocDT.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Membership.Repositories.QuocDT
{
    public class CustomerMembershipsQuocDtRepository : GenericRepository<CustomerMembershipsQuocDt>
    {
        public CustomerMembershipsQuocDtRepository() { }
        public CustomerMembershipsQuocDtRepository(CarWashManagementSystemContext context) : base(context) => _context = context;
        public async Task<List<CustomerMembershipsQuocDt>> GetAllAsync()
        {
            return await _context.CustomerMembershipsQuocDts
                .Include(x => x.Tier)
                .ToListAsync();
        }
        public async Task<CustomerMembershipsQuocDt> GetByIdAsync(Guid id)
        {
            return await _context.CustomerMembershipsQuocDts
                .Include(x => x.Tier)
                .FirstOrDefaultAsync(x => x.MembershipIdquocDt == id);
        }
        public async Task<List<CustomerMembershipsQuocDt>> SearchAsync(string customerName, int? pointsBalance, string tierName)
        {
            return await _context.CustomerMembershipsQuocDts
                .Include(x => x.Tier)
                .Where(x =>
                    (string.IsNullOrEmpty(customerName) || x.CustomerName.Contains(customerName)) &&
                    (!pointsBalance.HasValue || x.CurrentPointsBalance == pointsBalance) &&
                    (string.IsNullOrEmpty(tierName) || x.Tier.TierName.Contains(tierName))
                )
                .ToListAsync();
        }
    }
}
