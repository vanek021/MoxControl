﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MoxControl.Core.Models;

namespace MoxControl.Data.Seeds
{
    public static class RoleSeeds
    {
        private static List<string> roles = new() { "Administrator", "User" };

        public static void Seed(IServiceProvider serviceProvider)
        {
            var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<BaseRole>>();

            foreach (var role in roles)
                if (roleManager.FindByNameAsync(role).GetAwaiter().GetResult() is null)
                    roleManager.CreateAsync(new BaseRole(role)).Wait();
        }
    }
}
