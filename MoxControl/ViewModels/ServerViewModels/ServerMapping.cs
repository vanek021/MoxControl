﻿using AutoMapper;
using MoxControl.Connect.Models;
using MoxControl.Connect.Models.Entities;
using MoxControl.Connect.Proxmox.Models;

namespace MoxControl.ViewModels.ServerViewModels
{
    public class ServerMapping : Profile
    {
        public ServerMapping()
        {
            CreateMap<ServerViewModel, BaseServer>();
            CreateMap<BaseServer, ServerViewModel>();
        }
    }
}
