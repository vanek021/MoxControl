using System.ComponentModel.DataAnnotations;

namespace MoxControl.ViewModels.SettingViewModels
{
#pragma warning disable CS8618
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
#pragma warning restore CS8618
}
