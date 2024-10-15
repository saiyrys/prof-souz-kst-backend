using AutoMapper;
using SendGrid.Helpers.Errors.Model;
using UserControl;
using Users.Application.Dto;
using Users.Application.Interface;
using Users.Domain.Entities;
using Users.Infrastructure.Data;
using Users.Infrastructure.Repositories;

namespace Users.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IControl<User> Control;

        public UserServices(ApplicationDbContext context, IMapper mapper, IUserRepository userRepository, IControl<User> control)
        {
            _context = context;
            _mapper = mapper;
            _userRepository = userRepository;
            Control = control;
        }

        public async Task<(IEnumerable<GetUserDto> user, int totalPage)> GetUserForAdmin(int page, string? search = null, string? sort = null, string? type = null, string? f = null)
        {
            int pageSize = 12;

            var users = await _userRepository.GetAllUser();

            if (search != null || sort != null || type != null || f != null)
            {
                users = await _userRepository.SearchUser(search, sort, type, f);
            }

            var userDto = _mapper.Map<List<GetUserDto>>(users);

            var pagination = await ApplyPagination(userDto, page, pageSize);
            userDto = pagination.Item1;

            var totalPage = pagination.Item2;

            return (userDto, totalPage);

        }
        public async Task<GetUserDto> GetUserById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id is nothing");

            var user = await Control.FindByIdAsync(id);

            if (user != null)
            {
                var userDto = _mapper.Map<GetUserDto>(user);

                return userDto;
            }

            throw new BadRequestException("User not found");
        }
        public async Task<GetUserDto> GetUserInfo(string token)
        {
            /*await Control.VerifyByTokenAsync();*/

            if (token is null)
                throw new BadRequestException("Unauthorized token");

            var user = await Control.FindByTokenAsync(token);

            /*if (user != null)
            {*/
                var userDto = _mapper.Map<GetUserDto>(user);

                return userDto;
            /*}

            throw new BadRequestException("user not found");*/

        }
        public Task<bool> UpdateUser(string userId)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteUser(string userId)
        {
            var userToDelete = await _userRepository.GetUserById(userId);

            if(!await _userRepository.DeleteUser(userToDelete))
            {
                throw new ArgumentNullException();
            }

            return true;
        }

        private static async Task<Tuple<List<T>, int>> ApplyPagination<T>(List<T> items, int page, int pageSize)
        {
            int totalItems = items.Count;

            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            int skip = (page - 1) * pageSize;
            var itemsForPage = items.Skip(skip).Take(pageSize).ToList();

            return Tuple.Create(itemsForPage, totalPages);
        } 
    }
}
