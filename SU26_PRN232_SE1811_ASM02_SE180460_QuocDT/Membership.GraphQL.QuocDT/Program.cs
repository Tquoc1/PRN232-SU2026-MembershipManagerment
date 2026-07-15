using HotChocolate; // Đảm bảo bạn đã add package HotChocolate.AspNetCore
using Membership.Repositories.QuocDT;
using Membership.Repositories.QuocDT.DBContext;
using Membership.Services.QuocDT;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CarWashManagementSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =========================================================================
// 1. CẤU HÌNH DEPENDENCY INJECTION (DI) - Giữ nguyên các Service/Repo của bạn
// =========================================================================
builder.Services.AddScoped<CustomerMembershipsQuocDtRepository>();
builder.Services.AddScoped<ICustomerMembershipsService, CustomerMembershipsService>();
builder.Services.AddScoped<ISystemUserAccountService, SystemUserAccountService>();
builder.Services.AddScoped<SystemUserAccountRepository>();

// =========================================================================
// 2. CẤU HÌNH GRAPHQL (Thay thế cho Controllers và Swagger của REST)
// =========================================================================
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()       // Đăng ký lớp Query (nơi chứa các hàm lấy dữ liệu)
    .AddMutationType<Mutation>() // Đăng ký lớp Mutation (nơi chứa các hàm thêm/sửa/xóa)
    .AddFiltering()              // Bật tính năng Filter (HotChocolate.Data)
    .AddSorting();               // Bật tính năng Sort (HotChocolate.Data)

var app = builder.Build();

// =========================================================================
// 3. CẤU HÌNH HTTP PIPELINE (Bỏ MapControllers, chỉ chạy GraphQL)
// =========================================================================
app.UseHttpsRedirection();

// Map Endpoint duy nhất cho GraphQL (Mặc định là đường dẫn /graphql)
app.MapGraphQL();

app.Run();