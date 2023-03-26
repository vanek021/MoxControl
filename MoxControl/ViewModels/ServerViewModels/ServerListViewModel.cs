﻿using MoxControl.Connect.Models.Enums;
using Sakura.AspNetCore;

namespace MoxControl.ViewModels.ServerViewModels
{
    public class ServerListViewModel
    {
        public VirtualizationSystem VirtualizationSystem { get; set; }
        public List<ServerViewModel> Servers { get; set; }
    }
}
