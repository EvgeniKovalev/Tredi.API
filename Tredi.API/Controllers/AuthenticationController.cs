using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tredi.API.DataServices;
using Tredi.API.Models;

namespace Tredi.API.Controllers
{
	[ApiController]
	[Produces("application/json")]
	[Route("[controller]")]
	public class AuthenticationController : ControllerBase
	{
		DataServiceCollection dataServices;

		public AuthenticationController(DataServiceCollection dataServices)
		{
			this.dataServices = dataServices;
		}

		[Authorize]
		[HttpGet("CheckLoggedIn")]
		public IActionResult CheckLoggedIn()
		{
			return Ok("ok");
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginDto form)
		{
			try
			{
				var user = await dataServices.UserService.GetUserByLogin(form);

				var claims = new List<Claim> {
					new Claim("Guid", user.Id),
					new Claim("Name", user.Name)
				};

				var identity = new ClaimsIdentity(claims, "TrediCooking");
				var claimsPrincipal = new ClaimsPrincipal(identity);

				await HttpContext.SignInAsync("TrediCooking", claimsPrincipal);
			}
			catch
			{
				return Unauthorized();
			}
			return Ok("ok");
		}

		[HttpGet("GetUser")]
		public UserDto GetUser()
		{
			var a = this.User.FindFirst("Guid")?.Value;
			return new UserDto() { UserName = a };
		}

		[HttpGet("Logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync("TrediCooking");
			return Ok("ok");
		}


		[HttpGet("Success")]
		public IActionResult Success()
		{
			return Ok("Success");
		}


		[HttpGet("OauthCallback")]
		public void OauthCallback()
		{
			int p = 0;
		}

		[HttpPost("Authorize")]
		public IActionResult Authorize(string username)
		{
			return Ok("authorized " + username);
		}

		[HttpGet("Token")]
		public IActionResult Token()
		{
			return Ok("Tokenaut");
		}

		[HttpGet("Add")]
		public async Task<IActionResult> Add()
		{
			//var newUser = new UserDto()
			//{
			//	Id = Guid.NewGuid().ToString(),
			//	PartitionKey = "Admins",
			//	UserName = "IRA"
			//};

			//await dataServices.UserService.AddUser(newUser);
			return Ok("add");
		}

		//[HttpGet("GetUsers")]
		//public async Task<IActionResult> GetUserss()
		//{
		//	var a = await _dbContext.Users.ToListAsync();
		//	return Ok(a);


		//var newUser = new UserDto()
		//{
		//	Id = Guid.NewGuid().ToString(),
		//	PartitionKey = "Admins",
		//	UserName = "птщь"
		//};

		//await dataServices.UserService.AddUser(newUser);
		//		return Ok("add");



		//}

	}
}
