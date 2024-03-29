﻿using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Models;
using System.Reflection;

namespace MoxControl.Core.Internal
{
    internal class ReflectionTools
    {
        public static void FindRealContextGenericArgumentsTypes<TContext>(ref Type? userType, ref Type? roleType) where TContext : DbContext
        {
            var cur = typeof(TContext);
            while (cur != null && cur != typeof(object))
            {
                if (cur.GetTypeInfo().IsGenericType)
                {
                    var args = cur.GetGenericArguments().ToArray();
                    if (args.Length == 2)
                    {
                        if (typeof(BaseUser).IsAssignableFrom(args[0]) && typeof(BaseRole).IsAssignableFrom(args[1]))
                        {
                            userType = args[0];
                            roleType = args[1];
                            break;
                        }
                    }
                }
                cur = cur.GetTypeInfo().BaseType;
            }
        }

        public static void CallGenericStaticMethodForDbContextType<TContext>(Type parentClassType, string methodName, object[] parametersList)
            where TContext : DbContext
        {
            Type? userType = null, roleType = null;
            FindRealContextGenericArgumentsTypes<TContext>(ref userType, ref roleType);

            if (userType == null)
                throw new ArgumentException("Invalid DbСontext type. Looking for DbContext<TUser, TRole>");

            var mtdGeneric = parentClassType.GetTypeInfo().GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic) 
                ?? throw new ArgumentException($"Invalid private static method name {methodName}");
            if (mtdGeneric.GetGenericArguments().Length != 3)
                throw new ArgumentException($"{methodName} should have 3 generic arguments\r\nMethodName<TContext, TUser, TRole>(...)");

            var generic = mtdGeneric.MakeGenericMethod(typeof(TContext), userType, roleType!);
            generic.Invoke(null, parametersList);
        }
    }
}
