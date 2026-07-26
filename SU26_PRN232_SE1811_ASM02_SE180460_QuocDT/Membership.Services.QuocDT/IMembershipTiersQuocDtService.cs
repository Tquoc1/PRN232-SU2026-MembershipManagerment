using Membership.Entities.QuocDT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Membership.Services.QuocDT
{
    public interface IMembershipTiersQuocDtService
    {
        Task<List<MembershipTiersQuocDt>> GetAllMembershipTiersAsync();
    }
}
