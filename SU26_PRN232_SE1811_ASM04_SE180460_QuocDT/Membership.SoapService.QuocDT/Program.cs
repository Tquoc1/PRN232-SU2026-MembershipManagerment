using Membership.Repositories.QuocDT;
using Membership.Repositories.QuocDT.DBContext;
using Membership.Services.QuocDT;
using Membership.SoapService.QuocDT.SoapServices;
using Microsoft.EntityFrameworkCore;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<CarWashManagementSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen();
builder.Services.AddSoapCore();
builder.Services.AddScoped<ICustomerMembershipsService, CustomerMembershipsService>();
builder.Services.AddScoped<ICustomerMembershipsQuocDtSoapService, CustomerMembershipsQuocDtSoapService>();
builder.Services.AddScoped<CustomerMembershipsQuocDtRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSoapEndpoint<ICustomerMembershipsQuocDtSoapService>("/CustomerMembershipsQuocDtService.svc", new SoapEncoderOptions());

app.Run();
