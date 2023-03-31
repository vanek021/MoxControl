using Microsoft.EntityFrameworkCore;
using MoxControl.Connect.Data.Repositories;
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
        public ConnectDatabase(DbContext context, MachineTemplateRepository machineTemplatesRepo, ISOImageRepository isoImagesRepo) : base(context)
        {
            MachineTemplates = machineTemplatesRepo;
            ISOImages = isoImagesRepo;
        }

        public MachineTemplateRepository MachineTemplates { get; private set; }
        public ISOImageRepository ISOImages { get; private set; }
    }
}
