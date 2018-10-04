using ChatApplication.UserService;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ChatBackend.Controllers
{
	[Route("api/[controller]")]
	public class UsersController : Controller
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet("exists")]
		public IActionResult CheckIfUserExist([FromQuery] string name)
		{
			if (_userService.UsersOnline().FirstOrDefault(u => u.Name == name) != null)
			{
				return BadRequest($"User with name {name} already exists");
			}
			return NoContent();
		}
	}
}
