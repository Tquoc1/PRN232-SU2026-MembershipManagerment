using HotChocolate;
using HotChocolate.Data;
using Membership.Services.QuocDT;
using Membership.Entities.QuocDT.Models; // Khớp với namespace chứa CustomerMembershipsQuocDt trong lỗi

public class Query
{
    [UseFiltering]
    [UseSorting]
    // Đổi từ Task<List<...>> sang Task<IEnumerable<...>> để khớp với Service của bạn
    public async Task<IEnumerable<CustomerMembershipsQuocDt>> GetCustomerMemberships([Service] ICustomerMembershipsService membershipService)
    {
        return await membershipService.GetAllAsync();
    }

    // Đổi tham số 'int id' thành 'Guid id' vì lỗi báo không thể convert int sang System.Guid
    public async Task<CustomerMembershipsQuocDt?> GetCustomerMembershipById(Guid id, [Service] ICustomerMembershipsService membershipService)
    {
        return await membershipService.GetByIdAsync(id);
    }
}