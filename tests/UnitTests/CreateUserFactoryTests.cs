using Moq;
using System.Data;
using Users.Application.Dto;
using Users.Domain.Entities;
using Users.Domain.Factories;
using Users.Domain.Services.HashPassword;
using Users.Domain.Sevices.GenerationPassword;

namespace UnitTests
{
    public class CreateUserFactoryTests
    {
        private readonly CreateUserFactory _userFactoryApp;

        public CreateUserFactoryTests()
        {
            _userFactoryApp = new CreateUserFactory();
        }

        [Fact]
        public async Task CreateUserTests()
        {
            //Arrange
            var user = new UserDto()
            {
                userName = "test",
                firstName = "test",
                lastName = "test",
                middleName = "test",
                email = "test",
                password = "test",
                role = "USER"
            };

            //Act
            var result = await _userFactoryApp.CreateUser(
                user.userName,
                user.firstName,
                user.lastName,
                user.middleName,
                user.email,
                user.password,
                user.role
                );

            

            Assert.NotNull(result); // Проверяем, что результат не null
            Assert.Equal(user.userName, result.userName); // Проверяем, что имя пользователя совпадает
            Assert.Equal(user.firstName, result.firstName); // Проверяем имя
            Assert.Equal(user.lastName, result.lastName); // Проверяем фамилию
            Assert.Equal(user.middleName, result.middleName); // Проверяем отчество
            Assert.Equal(user.email, result.email); // Проверяем email
            Assert.Equal(user.role, "USER"); // Проверяем роль



        }
    }
}