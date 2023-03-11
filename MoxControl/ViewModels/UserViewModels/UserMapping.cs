using AutoMapper;
using MoxControl.Models.Entities;

namespace MoxControl.ViewModels.UserViewModels
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(src => src.Role, opt => opt.Ignore());
        }
    }
}
