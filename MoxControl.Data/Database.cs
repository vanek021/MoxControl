using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Attributes;
using MoxControl.Core.Interfaces;
using MoxControl.Core.Repositories;
using MoxControl.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Data
{
    [Injectable, Injectable(typeof(IDatabase))]
    public class Database : AbstractDatabase
    {
        public Database(DbContext context, GeneralSettingRepository generalSettingRepo) : base(context)
        {
            GeneralSettings = generalSettingRepo;
        }

        public GeneralSettingRepository GeneralSettings { get; private set; }
    }
}
