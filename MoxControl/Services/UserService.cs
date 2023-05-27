using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoxControl.Core.Models;
using MoxControl.Models.Entities;
using MoxControl.ViewModels.UserViewModels;
using Sakura.AspNetCore;

namespace MoxControl.Services
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<BaseRole> _roleManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, RoleManager<BaseRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IPagedList<UserViewModel>> GetUserViewModelsAsync(int page, int pageSize)
        {
            var users = await _userManager.Users
                .Include(x => x.Roles)
                .OrderBy(x => x.Id)
                .ToPagedListAsync(pageSize, page);

            var userVms = new List<UserViewModel>();

            foreach (var user in users)
            {
                var vm = _mapper.Map<User, UserViewModel>(user);
                var roles = await _userManager.GetRolesAsync(user);
                vm.Role = roles.FirstOrDefault();
                userVms.Add(vm);
            }

            return new PagedList<List<UserViewModel>, UserViewModel>(userVms, userVms, pageSize, page, users.TotalCount, users.TotalPage);
        }

        public async Task<UserSettingsViewModel?> GetUserSettingsViewModelAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return null;

            var viewModel = _mapper.Map<UserSettingsViewModel>(user);

            return viewModel;
        }

        public async Task<bool> UpdateUserSettings(UserSettingsViewModel viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.Id.ToString());

            if (user is null)
                return false;

            user = _mapper.Map(viewModel, user);

            try
            {
                await _userManager.UpdateAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
