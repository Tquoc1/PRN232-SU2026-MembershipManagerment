using HotChocolate; // Đảm bảo bạn đã add package HotChocolate.AspNetCore
using Membership.Repositories.QuocDT;
using Membership.Repositories.QuocDT.DBContext;
using Membership.Services.QuocDT;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CarWashManagementSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =========================================================================
// JWT Authentication & Authorization Configuration
// =========================================================================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Secret Key is not configured."));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<CustomerMembershipsQuocDtRepository>();
builder.Services.AddScoped<ICustomerMembershipsService, CustomerMembershipsService>();
builder.Services.AddScoped<ISystemUserAccountService, SystemUserAccountService>();
builder.Services.AddScoped<SystemUserAccountRepository>();
builder.Services.AddScoped<MembershipTiersQuocDtRepository>();
builder.Services.AddScoped<IMembershipTiersQuocDtService, MembershipTiersQuocDtService>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()       // Đăng ký lớp Query (nơi chứa các hàm lấy dữ liệu)
    .AddMutationType<Mutation>() // Đăng ký lớp Mutation (nơi chứa các hàm thêm/sửa/xóa)
    .AddFiltering()              // Bật tính năng Filter (HotChocolate.Data)
    .AddSorting()                // Bật tính năng Sort (HotChocolate.Data)
    .AddAuthorization();         // Kích hoạt authorization cho HotChocolate

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

app.Run();