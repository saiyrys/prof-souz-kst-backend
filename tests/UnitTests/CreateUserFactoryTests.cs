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

            

            Assert.NotNull(result); // ���������, ��� ��������� �� null
            Assert.Equal(user.userName, result.userName); // ���������, ��� ��� ������������ ���������
            Assert.Equal(user.firstName, result.firstName); // ��������� ���
            Assert.Equal(user.lastName, result.lastName); // ��������� �������
            Assert.Equal(user.middleName, result.middleName); // ��������� ��������
            Assert.Equal(user.email, result.email); // ��������� email
            Assert.Equal(user.role, "USER"); // ��������� ����



        }
    }
}