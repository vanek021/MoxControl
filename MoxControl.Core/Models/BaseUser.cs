using Microsoft.AspNetCore.Identity;
using MoxControl.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using IDKEY = System.Int64;

namespace MoxControl.Core.Models
{
    [Table("Users")]
    public class BaseUser : IdentityUser<IDKEY>, IEntity
    {
        public virtual ICollection<BaseUserRole> Roles { get; } = new List<BaseUserRole>();
        public virtual ICollection<BaseUserClaim> Claims { get; } = new List<BaseUserClaim>();
        public virtual ICollection<BaseUserLogin> Logins { get; } = new List<BaseUserLogin>();
    }
}
