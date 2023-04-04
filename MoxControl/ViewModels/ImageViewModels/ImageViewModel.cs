using MoxControl.Connect.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MoxControl.ViewModels.ImageViewModels
{
    public class ImageViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Имя")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Способ хранения")]
        [Required]
        public ImageStorageMethod StorageMethod { get; set; }

        [Display(Name = "Ссылка на загрузку")]
        [Required]
        public string ImagePath { get; set; }
    }
}
