using Membership.Entities.QuocDT.Models;
using System.ServiceModel;

namespace Membership.SoapService.QuocDT.SoapServices
{
    [ServiceContract]
    public interface ICustomerMembershipsQuocDtSoapService
    {

        /// Queries 
        [OperationContract]
        Task<List<CustomerMembershipsQuocDt>> GetAllAsync();
        [OperationContract]
        Task<CustomerMembershipsQuocDt> GetByIdAsync(Guid id);
        [OperationContract]
        Task<List<CustomerMembershipsQuocDt>> SearchAsync(string? customerName, int? currentPointsBalance, string? tierName);
        /// Mutation
        [OperationContract]
        Task<int> CreateAsync(CustomerMembershipsQuocDt customerMembershipsQuocDt);
        [OperationContract]
        Task<int> UpdateAsync(CustomerMembershipsQuocDt customerMembershipsQuocDt);
        [OperationContract]
        Task<bool> DeleteAsync(string id);
    }
}
