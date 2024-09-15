using Application.Interfaces.IServices;
using Application.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace WebAPIs.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _memoryCache;

        public UserController(IUserService userService, IMemoryCache memoryCache)
        => (_userService, _memoryCache) = (userService, memoryCache);

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterModel model)
        {
            var user = await _userService.RegisterUserAsync(model);
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.LoginAsync(model);
            return Ok(user);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetUsers(int pageIndex = 1, int pageSize = 10)
        {
            var users = await _userService.GetUsersAsync(pageIndex, pageSize);
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult> GetUserById([FromRoute] Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult> GetUserProfile()
        {
            var key = $"User_HELLO";
            if (_memoryCache.TryGetValue(key, out ActionResult cacheResult))
            {
                return cacheResult;
            }
            var result = await _userService.GetUserProfile();
            _memoryCache.Set(key, result, TimeSpan.FromMinutes(5));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser([FromRoute] Guid id)
        {
            var user = await _userService.DeleteUser(id);
            return Ok(user);
        }

    }
}
