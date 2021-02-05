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

        [HttpGet]
        public async Task<IList<User>> Get()
        {
            return await _userApplicationService.GetAllUsersAsync();
        }
    }
}