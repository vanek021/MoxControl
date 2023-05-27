using Microsoft.AspNetCore.Identity;
using MoxControl.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
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
