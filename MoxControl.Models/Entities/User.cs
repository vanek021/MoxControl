using MoxControl.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Models.Entities
{
    public class User : BaseUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
