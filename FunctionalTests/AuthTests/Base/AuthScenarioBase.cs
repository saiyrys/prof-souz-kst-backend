using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalTests.AuthTests.Base
{
    public class AuthScenarioBase
    {
        private const string ApiUrlBase = "api/auth";

        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(AuthScenarioBase))?.Location;

            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddJsonFile("appsetttings.json", optional: false)
                        .AddEnvironmentVariables();
                })
                .UseUrls("http://*:7001")
                .UseStartup<Program>();

            return new TestServer(hostBuilder);
        }

        public static class Get
        {
            public static string GetUserInfo = $"{ApiUrlBase}/access-token";
        }
        public static class Post
        {
            public static string Login = $"{ApiUrlBase}/login";
            public static string Register = $"{ApiUrlBase}/register";
        }
    }
}
