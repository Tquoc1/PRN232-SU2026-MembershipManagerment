using Membership.Entities.QuocDT.Models;
using Membership.Repositories.QuocDT.Base;
using Membership.Repositories.QuocDT.DBContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Membership.Repositories.QuocDT
{
    public class SystemUserAccountRepository : GenericRepository<SystemUserAccount>
    {
        public SystemUserAccountRepository() { }
        public SystemUserAccountRepository(CarWashManagementSystemContext context) :base(context)  => _context = context;
        public async Task<SystemUserAccount> GetUserAccount(string userName, string password)
        {
            return _context.SystemUserAccounts.FirstOrDefault(x => x.Email == userName && x.Password == password && x.IsActive);
            //return _context.SystemUserAccounts.FirstOrDefault(x => x.UserName == userName && x.Password == password && x.IsActive);
            //return _context.SystemUserAccounts.FirstOrDefault(x => x.Phone == userName && x.Password == password && x.IsActive);
            //return _context.SystemUserAccounts.FirstOrDefault(x => x.EmployeeCode == userName && x.Password == password && x.IsActive);
        }
    }
}
