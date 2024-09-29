using Auth.Application.Dto;
using FunctionalTests.AuthTests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FunctionalTests.AuthTests
{
    public class AuthScenario : AuthScenarioBase
    {
        private const string RequestType = "application/json";


        [Fact]
        public async Task Post_register_user_response_ok_status_code()
        {
            using var server = CreateServer();

            var content = new StringContent(registerUser(), Encoding.UTF8, RequestType);
            var response = await server.CreateClient().PostAsync(Post.Register, content);

            response.EnsureSuccessStatusCode();

        }

        private static string registerUser()
        {
            var user = new RegistrationDto
            {
                userName = "test",
                password = "test",
                role = "ADMIN",

            };

            return JsonSerializer.Serialize(user);
        }
    }
}
