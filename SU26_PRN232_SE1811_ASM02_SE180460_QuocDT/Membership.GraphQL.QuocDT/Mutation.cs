using HotChocolate;
using Membership.Services.QuocDT;
using Membership.Entities.QuocDT.Models;

public class Mutation
{
    public async Task<SystemUserAccount?> Login(
        string username,
        string password,
        [Service] ISystemUserAccountService userService)
    {
        return await userService.GetUserAccount(username, password);
    }


    public async Task<int> CreateCustomerMembership(
        CustomerMembershipsQuocDt customer,
        [Service] ICustomerMembershipsService membershipService) 
    {
        return await membershipService.CreateAsync(customer);
    }

    public async Task<int> UpdateCustomerMembership(
        CustomerMembershipsQuocDt customer,
        [Service] ICustomerMembershipsService membershipService)
    {
        return await membershipService.UpdateAsync(customer);
    }

    public async Task<bool> DeleteCustomerMembership(
        Guid id,
        [Service] ICustomerMembershipsService membershipService)
    {
        var result = await membershipService.DeleteAsync(id);
        return result > 0;
    }
}