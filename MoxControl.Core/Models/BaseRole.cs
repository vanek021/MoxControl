using Microsoft.AspNetCore.Identity;
using MoxControl.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using IDKEY = System.Int64;

namespace MoxControl.Core.Models
{
    [Table("Roles")]
    public class BaseRole : IdentityRole<IDKEY>, IEntity
    {
        public BaseRole() { }

        public BaseRole(string roleName)
        {
            Name = roleName;
        }
    }
}
