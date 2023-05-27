using System.ComponentModel.DataAnnotations;

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
