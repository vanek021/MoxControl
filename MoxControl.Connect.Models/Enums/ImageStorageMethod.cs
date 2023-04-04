using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Models.Enums
{
    public enum ImageStorageMethod
    {
        [Display(Name = "Ссылка на скачивание")]
        DownloadLink = 0,
        [Display(Name = "Локально")]
        Local
    }
}
