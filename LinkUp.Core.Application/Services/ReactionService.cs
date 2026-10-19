using AutoMapper;
using LinkUp.Core.Application.Dtos.Battleship.Reaction;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Core.Application.Services
{
    public class ReactionService : GenericService<Reaction, ReactionDto>, IReactionService
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly IMapper _mapper;

        public ReactionService(IReactionRepository reactionRepository, IMapper mapper) : base(reactionRepository, mapper)
        {
            _reactionRepository = reactionRepository;
            _mapper = mapper;
        }

        public async Task<ReactionDto?> GetUserReactionToPostAsync(string userId, int postId)
        {
            try
            {
                var entity = await _reactionRepository.GetAllQuery()
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.PostId == postId);

                if (entity == null) return null;

                return _mapper.Map<ReactionDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}