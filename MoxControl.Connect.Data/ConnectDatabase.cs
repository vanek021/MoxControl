using MoxControl.Connect.Data.Repositories;
using MoxControl.Core.Attributes;
using MoxControl.Core.Interfaces;
using MoxControl.Core.Repositories;

namespace MoxControl.Connect.Data
{
    [Injectable, Injectable(typeof(IDatabase))]
    public class ConnectDatabase : AbstractDatabase
    {
        public ConnectDatabase(ConnectDbContext context, TemplateRepository templatesRepo, ISOImageRepository isoImagesRepo,
            ConnectSettingRepository connectSettingRepo) : base(context)
        {
            Templates = templatesRepo;
            ISOImages = isoImagesRepo;
            ConnectSettings = connectSettingRepo;
        }

        public TemplateRepository Templates { get; private set; }
        public ISOImageRepository ISOImages { get; private set; }
        public ConnectSettingRepository ConnectSettings { get; private set; }
    }
}
