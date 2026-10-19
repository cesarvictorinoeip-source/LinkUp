using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinkUp.Core.Application.Dtos.Battleship.User;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Domain.Entities;
using LinkUp.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Core.Application.Services
{
    public class UserService : GenericService<AppUser, UserDto>, IUserService
    {
        private readonly IGenericRepository<AppUser> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<AppUser> userRepository, IMapper mapper)
            : base(userRepository, mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto?> GetByUsernameAsync(string username)
        {
            var entity = await _userRepository.GetAllQuery()
                .FirstOrDefaultAsync(u => u.UserName == username);
            return entity == null ? null : _mapper.Map<UserDto>(entity);
        }

        public async Task<List<UserDto>> GetUserSuggestionsAsync(string currentUserId)
        {
            var query = _userRepository.GetAllQuery()
                .Where(u => u.Id != currentUserId)
                .Take(10);
            return await query.ProjectTo<UserDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        Task<UserDto?> IUserService.GetById(int id)
        {
            throw new NotImplementedException();
        }

        Task<UserDto?> IUserService.GetByUsernameAsync(string username)
        {
            return GetByUsernameAsync(username);
        }

        Task<List<UserDto>> IUserService.GetUserSuggestionsAsync(string currentUserId)
        {
            return GetUserSuggestionsAsync(currentUserId);
        }
    }
}