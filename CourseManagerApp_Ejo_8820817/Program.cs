/*
 * Name of Project  : Problem assignment #3
 * Written by       : Eunheui Jo(ID: 8820817)
 * Date             : Dec. 11. 2023
 * Programming MS Web Tech (PROG2230)
*/

using Microsoft.EntityFrameworkCore;
using Customers.Entities;
using Customers.Services;
using CustomerManagerApp_Ejo_8820817.DataAccess;
using CustomerManagerApp_Ejo_8820817.Services;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// add our context as a service:
string connStr = builder.Configuration.GetConnectionString("CustomerDb");


builder.Services.AddDbContext<CustomerDbContext>(options =>
{
    options.UseSqlServer(connStr);
}, ServiceLifetime.Scoped); 

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register service
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IProcessDataService, ProcessDataService>();

// configure sessions:
builder.Services.AddMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromSeconds(10);
    option.Cookie.HttpOnly = false;
    option.Cookie.IsEssential = true;
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// and use sessions in the middleware pipeline:
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customer}/{action=GetInitCustomers}"
    );

app.Run();
