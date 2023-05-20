using IntegrationTest.Api.Dto;
using Microsoft.AspNetCore.Mvc;
using IntegrationTest.Api.Services;

namespace IntegrationTest.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService _accountService;
		private readonly ILogger<AccountController> _logger;
		public AccountController(
			IAccountService accountService,
			ILogger<AccountController> logger
		)
		{
			_logger = logger;
			_accountService = accountService;
		}

		[HttpPost("authenticate")]
		public async Task<IActionResult> AuthenticateUser([FromBody] LoginRequestDto dto)
		{
			try
			{
				var response = await _accountService.AuthenticateUserAsync(dto);
				return Ok(response);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest();
			}
		}
	}
}
