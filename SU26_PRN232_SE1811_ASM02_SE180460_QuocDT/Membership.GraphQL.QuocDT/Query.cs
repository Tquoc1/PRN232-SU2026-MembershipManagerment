using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Authorization;
using Membership.Services.QuocDT;
using Membership.Entities.QuocDT.Models;

public class Query
{
    [UseFiltering]
    [UseSorting]
    [Authorize(Roles = new[] { "1", "2" })]
    public async Task<IEnumerable<CustomerMembershipsQuocDt>> GetCustomerMemberships([Service] ICustomerMembershipsService membershipService)
    {
        return await membershipService.GetAllAsync();
    }

    [Authorize(Roles = new[] { "1", "2" })]
    public async Task<CustomerMembershipsQuocDt?> GetCustomerMembershipById(Guid id, [Service] ICustomerMembershipsService membershipService)
    {
        return await membershipService.GetByIdAsync(id);
    }
}