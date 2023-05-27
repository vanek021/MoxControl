using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using MoxControl.Connect;
using MoxControl.Connect.DependencyInjection;
using MoxControl.Core.Extensions;
using MoxControl.Data;
using MoxControl.Extensions;
using MoxControl.Infrastructure.Extensions;
using MoxControl.Middlewares;
using MoxControl.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddBasePgsqlContext<AppDbContext>(connectionString);

builder.Services.RegisterConnectContexts(connectionString);
builder.Services.RegisterConnectServices();

builder.Services.RegisterInjectableTypesFromAssemblies(typeof(Program), typeof(AppDbContext));

builder.Services.AddApplicationIdentity<AppDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(Program), typeof(MoxControl.Core.Interfaces.IEntity), typeof(AppDbContext), typeof(User));

builder.Services.AddConfigurations(builder.Configuration);

builder.Services.AddAppServices();

builder.Services.AddFileSystemBucketStorage(builder.Environment.WebRootPath, "default");

builder.Services.AddDataProtection();

builder.Services.AddHangfire(hangfire => hangfire.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("HangfireConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error/500");
    app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseDbLogs();

app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireDashboardAuthorizeFilter() },
    IgnoreAntiforgeryToken = true
});


app.UseConnect();

app.Services.SeedData();

HangfireConnectManager.RegisterJobs();

app.Run();
