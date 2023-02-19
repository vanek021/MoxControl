using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Attributes;
using MoxControl.Core.Interfaces;
using MoxControl.Core.Repositories;
using MoxControl.Models.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Data.Repositories
{
    [Injectable]
    [Injectable(typeof(IReadableRepository<GeneralSetting>))]

    public class GeneralSettingRepository : WriteableRepository<GeneralSetting>
    {
        public GeneralSettingRepository(DbContext context) : base(context)
        {

        }
    }
}
