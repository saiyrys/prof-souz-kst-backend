using Auth.Application.Dto;
using Auth.Application.Interface;
using Auth.Domain.Interface;
using Auth.Domain.Models;
using Auth.Infrastructure.Factories;
using Auth.Infrastructure.Repository;
using Auth.Infrastructure.SomeService.GenerationPassword;
using AutoMapper;
using SendGrid.Helpers.Errors.Model;



namespace Auth.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IControl<User> Control;

        private readonly TokenGeneration _generateToken;

        private readonly IHashingPassword _hashingPassword;
        private readonly IMapper _mapper;

        private readonly UserFactory _userFactory;

        private readonly IAuthRepository _authRepository;
 
        public AuthService(IControl<User> control,
            TokenGeneration generateToken, IHashingPassword hashingPassword,
            IMapper mapper, UserFactory userFactory, IAuthRepository authRepository)
        {
            Control = control;
            _generateToken = generateToken;
            _hashingPassword = hashingPassword;
            _mapper = mapper;

            _userFactory = userFactory;

            _authRepository = authRepository;

        }

        public async Task<LoginResponseDto> Login(LoginUserDto loginUser)
        {
            var user = await Control.FindByNameAsync(loginUser.userName);
            
            if (user == null || !await ValidatePassword(loginUser.password, user.password, user.salt))
                throw new BadRequestException();
            
            var userDto = _mapper.Map<UserInfoDto>(user);

            var token = await _generateToken.GenerateToken(userDto);

            return new LoginResponseDto
            {
                AccessToken = token.Item1,
                User = userDto
            };

        }

        public async Task<bool> Registration(RegistrationDto registration)
        {
            var user = await Control.FindByNameAsync(registration.userName);

            if (user != null)
                throw new BadRequestException();

            var createUser = await _userFactory.CreateUser(
                registration.userName,
                registration.firstName,
                registration.middleName,
                registration.lastName,
                registration.email,
                registration.password,
                registration.role
                );

            var userMap = _mapper.Map<User>(createUser);

            if(!await _authRepository.CreateUserAsync(userMap))
                throw new BadRequestException("Что то пошло не так при сохранении данных");

            return true;
        }

        public async Task<UserInfoDto> GetUser(string token)
        {
            /*var accessToken = Control.VerifyByTokenAsync();*/

            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException("Token is unregister");

            var user = await Control.FindByTokenAsync(token);

            if (user == null)
                throw new ArgumentNullException("User not found");

            var userDto = _mapper.Map<UserInfoDto>(user);

            return userDto;
        }
        public async Task<LoginResponseDto> GetNewTokens(string refreshToken)
        {
            var user = await Control.FindByTokenAsync(refreshToken);

            var userDto = _mapper.Map<UserInfoDto>(user);

            var newToken = await _generateToken.GenerateToken(userDto);

            return new LoginResponseDto
            {
                AccessToken = newToken.Item1,
                User = userDto
            };
        }
        private async Task<bool> ValidatePassword(string loginPassword, string dbPassword, string salt)
            => await _hashingPassword.VerifyPassword(loginPassword, dbPassword, salt);
    }
}
