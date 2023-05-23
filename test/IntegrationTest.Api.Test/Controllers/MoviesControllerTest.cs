using System.Net;

namespace IntegrationTest.Api.Test.Controllers
{
	[TestClass]
	public class MoviesControllerTest
	{
		[TestMethod]
		public async Task GetMovies_ShouldReturn200StatusCode()
		{
			//Setup
			var client = TestClient.GetTestClient();

			//arrange
			var response = await client.GetAsync("api/Movies");

			//Assert
			Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
		}
	}
}
