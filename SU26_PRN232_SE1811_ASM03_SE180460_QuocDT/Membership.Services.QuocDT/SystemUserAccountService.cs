using Membership.Entities.QuocDT.Models;
using Membership.Repositories.QuocDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Membership.Services.QuocDT
{
    public class SystemUserAccountService : ISystemUserAccountService
    {
        private readonly SystemUserAccountRepository _repository;
        public SystemUserAccountService() { }
        public SystemUserAccountService(SystemUserAccountRepository repository) => _repository = repository;
        public async Task<SystemUserAccount> GetUserAccount(string userName, string password)
        { 
            try
            {
                return await Task.Run(() => _repository.GetUserAccount(userName, password));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
