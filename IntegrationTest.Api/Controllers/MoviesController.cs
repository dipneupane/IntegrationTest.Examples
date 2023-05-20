using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTest_WorkAround.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class MoviesController : ControllerBase
	{
		private static readonly string[] Movies = new[]
		{
			"Mission Impossible", "Fast and Furious", "World War Z"
		};

		private readonly ILogger<MoviesController> _logger;

		public MoviesController(ILogger<MoviesController> logger)
		{
			_logger = logger;
		}

		[Authorize]
		[HttpGet(Name = "get-movies")]
		public IActionResult Get()
		{
			try
			{
				return Ok(Movies);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest();
			}
		}
	}
}