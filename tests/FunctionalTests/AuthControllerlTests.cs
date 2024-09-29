using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Auth.Application.Dto;
using Newtonsoft.Json;

namespace IntegrationalTests
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;

            _client = factory.CreateClient();
        }

        [Fact]
        public async Task LoginReturnsAccessTokenAndUser_OnValidCredentials()
        {
            // Arrange

            var loginUser = new LoginUserDto
            {
                userName = "string",
                password = "string"
            };
            var content = new StringContent(JsonConvert.SerializeObject(loginUser), Encoding.UTF8, "application/json");

            // act

            var response = await _client.PostAsync("/api/auth/login", content);

            // assert 

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(responseString);

            Assert.NotNull(loginResponse.AccessToken);
            Assert.NotNull(loginResponse.User);
            Assert.Equal("string", loginResponse.User.username);
        }

        [Fact]
        public async Task RegistrationReturnsTrue_OnValidCredentials()
        {
            // Arrange

            var registerUser = new RegistrationDto
            {
                userName = "test",
                firstName = "test",
                middleName = "test",
                lastName = "test",
                email = "test",
                role = "ADMIN",
                password = "123456"
            };

            var content = new StringContent(JsonConvert.SerializeObject(registerUser), Encoding.UTF8, "application/json");

            // act 

            var response = await _client.PostAsync("/api/auth/register", content);


            // assert

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var registerResult = JsonConvert.DeserializeObject<bool>(responseString);

            Assert.True(registerResult);
        }
    }
}
