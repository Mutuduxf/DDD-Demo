using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Domain.AggregateRoots;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserApplicationService _userApplicationService;

        public UserController(UserApplicationService userApplicationService)
        {
            _userApplicationService = userApplicationService;
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
        public async Task ChangeNameAsync(Guid userId, [FromBody] string name)
        {
            await _userApplicationService.ChangeNameAsync(userId, name);
        }

        [HttpPut("~/celebratebirthday")]
        public async Task CelebrateBirthdayAsync(Guid userId)
        {
            await _userApplicationService.CelebrateBirthdayAsync(userId);
        }

        [HttpPut("~/settags")]
        public async Task SetTags(Guid userId, [FromBody] IEnumerable<string> tags)
        {
            await _userApplicationService.SetTags(userId, tags);
        }

        [HttpPut("~/addcard")]
        public async Task AddCard(Guid userId, [FromBody] string cardName)
        {
            await _userApplicationService.AddCard(userId, cardName);
        }
    }
}