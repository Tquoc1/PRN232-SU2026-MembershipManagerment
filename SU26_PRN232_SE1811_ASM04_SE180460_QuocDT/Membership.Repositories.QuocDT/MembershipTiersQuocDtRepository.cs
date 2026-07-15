using Membership.Entities.QuocDT.Models;
using Membership.Repositories.QuocDT.Base;
using Membership.Repositories.QuocDT.DBContext;

namespace Membership.Repositories.QuocDT
{
    public class MembershipTiersQuocDtRepository : GenericRepository<MembershipTiersQuocDt>
    {
        public MembershipTiersQuocDtRepository() { }
        public MembershipTiersQuocDtRepository(CarWashManagementSystemContext context) : base(context) => _context = context;
    }
}
