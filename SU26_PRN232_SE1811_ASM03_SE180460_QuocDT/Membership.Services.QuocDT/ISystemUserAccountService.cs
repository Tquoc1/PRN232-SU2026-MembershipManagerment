using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Membership.Services.QuocDT
{
    public interface ISystemUserAccountService
    {
        Task<Membership.Entities.QuocDT.Models.SystemUserAccount> GetUserAccount(string userName, string password);
    }
}
