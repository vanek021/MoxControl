using Microsoft.EntityFrameworkCore;
using MoxControl.Connect.Data.Repositories;
using MoxControl.Connect.Models.Entities;
using MoxControl.Core.Attributes;
using MoxControl.Core.Interfaces;
using MoxControl.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Data
{
    [Injectable, Injectable(typeof(IDatabase))]
    public class ConnectDatabase : AbstractDatabase
    {
        public ConnectDatabase(ConnectDbContext context, MachineTemplateRepository machineTemplatesRepo, ISOImageRepository isoImagesRepo,
            ConnectSettingRepository connectSettingRepo) : base(context)
        {
            MachineTemplates = machineTemplatesRepo;
            ISOImages = isoImagesRepo;
            ConnectSettings = connectSettingRepo;
        }

        public MachineTemplateRepository MachineTemplates { get; private set; }
        public ISOImageRepository ISOImages { get; private set; }
        public ConnectSettingRepository ConnectSettings { get; private set; }
    }
}
