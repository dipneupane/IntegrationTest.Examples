using IntegrationTest.Api.Dto;
using IntegrationTest.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IntegrationTest.Api.Services
{
	public interface IAccountService
	{
		Task<LoginResponseDto> AuthenticateUserAsync(LoginRequestDto request);
	}
	public class AccountService : IAccountService
	{
		private readonly AppDbContext _context;
		private readonly IConfiguration _configuration;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		public AccountService(
			AppDbContext context,
			IConfiguration configuration,
			UserManager<User> userManager,
			SignInManager<User> signInManager
		)
		{
			_context = context;
			_userManager = userManager;
			_configuration = configuration;
			_signInManager = signInManager;
		}

		public async Task<LoginResponseDto> AuthenticateUserAsync(LoginRequestDto request)
		{
			var user = await _context.Users.Where(x => x.Email == request.Email).FirstOrDefaultAsync();

			if (user == null)
			{
				return new LoginResponseDto
				{
					Message = "User not found"
				};
			}

			SignInResult signInResult = await _signInManager.PasswordSignInAsync((user.Email ?? user.UserName).Trim(), request.Password, false, lockoutOnFailure: true);
			if (signInResult.Succeeded)
			{
				return new LoginResponseDto
				{
					Success = true,
					Jwt = _generatetoken(user),
					Message = "Authenticated successfully"
				};
			}
			else
			{
				return new LoginResponseDto
				{
					Message = "Username or password doesnot match"
				};
			}
		}

		private string _generatetoken(User user)
		{
			string userRole = _userManager.GetRolesAsync(user).GetAwaiter().GetResult()[0];
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSetting:SecretKey"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			List<Claim> claims = new List<Claim>
			{
				new Claim ("Role", userRole),
				new Claim("Email", user.Email),
			};

			var token = new JwtSecurityToken(
				issuer: _configuration["JwtSetting:Issuer"],
				audience: _configuration["JwtSetting:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSetting:ExpiryTimeInMin"])),
				signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
