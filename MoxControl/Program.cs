using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Extensions;
using MoxControl.Data;
using MoxControl.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddBasePgsqlContext<AppDbContext>(connectionString);

builder.Services.RegisterInjectableTypesFromAssemblies(typeof(Program), typeof(AppDbContext));

builder.Services.AddApplicationIdentity<AppDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddConfigurations(builder.Configuration);

builder.Services.AddAppServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error/500");
    app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    var ctx = serviceScope.ServiceProvider.GetService<AppDbContext>();
    ctx?.Database.Migrate();

}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
