using MoxControl.Connect.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Services
{
    public class ImageManager
    {
        private readonly ConnectDatabase _connectDatabase;

        public ImageManager(ConnectDatabase connectDatabase) 
        {
            _connectDatabase = connectDatabase;
        }
    }
}
