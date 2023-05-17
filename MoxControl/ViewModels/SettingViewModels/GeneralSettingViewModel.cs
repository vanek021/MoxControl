using System.ComponentModel.DataAnnotations;

namespace MoxControl.ViewModels.SettingViewModels
{
    public class GeneralSettingViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Системное название")]
        public string? SystemName { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Display(Name = "Значение")]
        public string Value { get; set; }
    }
}
