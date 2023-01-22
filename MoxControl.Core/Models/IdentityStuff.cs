using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDKEY = System.Int64;

namespace MoxControl.Core.Models
{
    public class BaseUserRole : IdentityUserRole<IDKEY> { }
    public class BaseUserClaim : IdentityUserClaim<IDKEY> { }
    public class BaseUserLogin : IdentityUserLogin<IDKEY> { }
    public class BaseRoleClaim : IdentityRoleClaim<IDKEY> { }
    public class BaseUserToken : IdentityUserToken<IDKEY> { }
}
