using Membership.Entities.QuocDT.Models;
using Membership.Services.QuocDT;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Membership.SoapService.QuocDT.SoapServices
{
    public class CustomerMembershipsQuocDtSoapService : ICustomerMembershipsQuocDtSoapService
    {
        private readonly ICustomerMembershipsService _service;
        public CustomerMembershipsQuocDtSoapService(ICustomerMembershipsService service)
        {
            _service = service;
        }   
        public async Task<List<CustomerMembershipsQuocDt>> GetAllAsync()
        {

            //throw new NotImplementedException();
            try
            {
                var items = await _service.GetAllAsync();
                var opt = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                var itemJson = JsonSerializer.Serialize(items, opt);
                var result = JsonSerializer.Deserialize<List<CustomerMembershipsQuocDt>>(itemJson, opt);
                return result ?? new List<CustomerMembershipsQuocDt>();
            }
            catch (Exception ex)
            {
                throw new Exception("Get all customer memberships error: " + ex.Message, ex);
            }
        }

        public async Task<CustomerMembershipsQuocDt> GetByIdAsync(Guid id)
        {
            //throw new NotImplementedException();
            try
            {
                var items = await _service.GetByIdAsync(id);
                var opt = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                var itemJson = JsonSerializer.Serialize(items, opt);
                var result = JsonSerializer.Deserialize<CustomerMembershipsQuocDt>(itemJson, opt);
                return result ?? new CustomerMembershipsQuocDt();
            }
            catch (Exception ex)
            {
                throw new Exception("Get customer membership by ID error: " + ex.Message, ex);
            }
        }

        public Task<List<CustomerMembershipsQuocDt>> SearchAsync(string? customerName, int? currentPointsBalance, string? tierName)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateAsync(CustomerMembershipsQuocDt customerMembershipsQuocDt)
        {
            //throw new NotImplementedException();
            try
            {
                var opt = new JsonSerializerOptions() { ReferenceHandler = ReferenceHandler.IgnoreCycles, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                var itemJson = JsonSerializer.Serialize(customerMembershipsQuocDt, opt);
                var item = JsonSerializer.Deserialize<CustomerMembershipsQuocDt>(itemJson, opt);
                var result = await _service.CreateAsync(item);
                return result ;
            }
            catch (Exception ex)
            {
                throw new Exception("Create customer membership error: " + ex.Message, ex);
            }
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }


        public Task<int> UpdateAsync(CustomerMembershipsQuocDt customerMembershipsQuocDt)
        {
            throw new NotImplementedException();
        }
    }
}
