using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Domain.AggregateRoots;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ServiceHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserApplicationService _userApplicationService;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger,
            UserApplicationService userApplicationService)
        {
            _userApplicationService = userApplicationService;
            _logger = logger;
        }

        [HttpGet("~/GetAll")]
        public async Task<IList<User>> GetAll()
        {
            return await _userApplicationService.GetAllUsersAsync();
        }

        [HttpGet]
        public async Task<User> Get(Guid id)
        {
            return await _userApplicationService.GetUserAsync(id);
        }

        [HttpPost]
        public async Task AddUser()
        {
            await _userApplicationService.AddUser();
        }

        [HttpPut("~/changename")]
        public async Task ChangeNameAsync(Guid userId, string name)
        {
            await _userApplicationService.ChangeNameAsync(userId, name);
        }

        [HttpPut("~/celebratebirthday")]
        public async Task CelebrateBirthdayAsync(Guid userId)
        {
            await _userApplicationService.CelebrateBirthdayAsync(userId);
        }
    }
}