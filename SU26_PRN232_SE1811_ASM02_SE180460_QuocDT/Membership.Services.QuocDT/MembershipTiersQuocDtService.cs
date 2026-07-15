using Membership.Repositories.QuocDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Membership.Services.QuocDT
{
    public class MembershipTiersQuocDtService : IMembershipTiersQuocDtService
    {
        private readonly MembershipTiersQuocDtRepository _repository;
        public MembershipTiersQuocDtService() { }
        public MembershipTiersQuocDtService(MembershipTiersQuocDtRepository repository) => _repository = new MembershipTiersQuocDtRepository();
        public async Task<List<Membership.Entities.QuocDT.Models.MembershipTiersQuocDt>> GetAllMembershipTiersAsync()
        {
            try
            {
                return await Task.Run(() => _repository.GetAll());
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
