using System.ComponentModel.DataAnnotations;

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
