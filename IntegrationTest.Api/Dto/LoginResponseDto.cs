namespace IntegrationTest.Api.Dto
{
	public class LoginResponseDto
	{
        public string Jwt { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
