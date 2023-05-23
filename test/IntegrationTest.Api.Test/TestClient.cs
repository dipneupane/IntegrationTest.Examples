using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTest.Api.Test
{
	public static class TestClient
	{
		public static HttpClient GetTestClient()
		{
			return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
			{
				builder.ConfigureTestServices(services =>
				{
					services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
				});
			}).CreateClient();
		}
	}
}