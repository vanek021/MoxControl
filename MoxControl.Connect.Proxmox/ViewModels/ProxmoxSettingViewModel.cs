﻿using System.ComponentModel.DataAnnotations;

namespace MoxControl.Connect.Proxmox.ViewModels
{
#pragma warning disable CS8618
    public class ProxmoxServerSettingViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Имя базового узла")]
        public string BaseNode { get; set; }
        [Display(Name = "Имя базового хранилища")]
        public string BaseStorage { get; set; }
        [Display(Name = "Realm")]
        public string Realm { get; set; }
    }
#pragma warning restore CS8618
}
