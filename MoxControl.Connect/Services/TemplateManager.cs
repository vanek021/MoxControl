using MoxControl.Connect.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Services
{
    public class TemplateManager
    {
        private readonly ConnectDatabase _connectDatabase;

        public TemplateManager(ConnectDatabase connectDatabase)
        {
            _connectDatabase = connectDatabase;
        }
    }
}
