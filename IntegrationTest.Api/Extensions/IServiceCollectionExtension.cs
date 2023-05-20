using IntegrationTest.Api.Entities;
using IntegrationTest.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace IntegrationTest.Api.Extensions
{
	public static class IServiceCollectionExtension
	{
		public static IServiceCollection ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDefaultIdentity<User>(options =>
			{
				options.Password.RequiredLength = 7;
				options.User.RequireUniqueEmail = true;
				options.Lockout.AllowedForNewUsers = true;
				options.Lockout.MaxFailedAccessAttempts = 15;
				options.SignIn.RequireConfirmedAccount = true;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
			}).AddRoles<Role>().AddEntityFrameworkStores<AppDbContext>();

			services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = configuration["JwtSetting:Issuer"],
					ValidAudience = configuration["JwtSetting:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSetting:SecretKey"]))
				};
			});

			return services;
		}

		public static IServiceCollection ConfigureCustomServices(this IServiceCollection services)
		{
			services.AddScoped<IAccountService, AccountService>();
			return services;
		}

		public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(option =>
			{
				option.SwaggerDoc("v1", new OpenApiInfo { Title = "Integration Testing Workaround API", Version = "v1" });
				option.CustomSchemaIds(type => type.ToString());
				option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter a valid token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "Bearer"
				});
				option.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type=ReferenceType.SecurityScheme,
								Id="Bearer"
							}
						},
						new string[]{}
					}
				});
			});
			return services;
		}
	}
}
