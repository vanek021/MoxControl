using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Models.Enums
{
    public enum AuthorizationType
    {
        [Display(Name = "По root данным")]
        RootCredentials = 0,
        [Display(Name = "По пользовательским данным")]
        UserCredentials
    }
}
