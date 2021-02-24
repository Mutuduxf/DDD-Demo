using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.AggregateRoots;
using Domain.DomainServices;
using Zaaby.DDD.Abstractions.Application;

namespace Application
{
    public class UserApplicationService : IApplicationService
    {
        private readonly UserDomainService _userDomainService;

        public UserApplicationService(UserDomainService userDomainService)
        {
            _userDomainService = userDomainService;
        }

        public async Task<IList<User>> GetAllUsersAsync()
        {
            return await _userDomainService.GetAllUsersAsync();
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _userDomainService.GetUserAsync(userId);
        }

        public async Task AddUser()
        {
            await _userDomainService.AddUser();
        }

        public async Task ChangeNameAsync(Guid userId, string name)
        {
            await _userDomainService.ChangeNameAsync(userId, name);
        }

        public async Task CelebrateBirthdayAsync(Guid userId)
        {
            await _userDomainService.CelebrateBirthdayAsync(userId);
        }
    }
}