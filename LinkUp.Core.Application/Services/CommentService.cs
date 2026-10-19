using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinkUp.Core.Application.Dtos.Battleship.Comment;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Entities; // IMPORTANTE: Para que reconozca 'Comment'
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Core.Application.Services
{
    public class CommentService : GenericService<Comment, CommentDto>, ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper) : base(commentRepository, mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        // CAMBIO AQUÍ: El nombre debe ser GetByIdAsync para coincidir con la base
        public override async Task<CommentDto?> GetByIdAsync(int id)
        {
            try
            {
                // Asegúrate de que tu ICommentRepository tenga este método o usa GetAllQuery()
                var query = _commentRepository.GetAllQuery();

                var entity = await query.FirstOrDefaultAsync(c => c.Id == id);

                if (entity == null) return null;

                return _mapper.Map<CommentDto>(entity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<CommentDto>> GetCommentsByPost(int postId)
        {
            try
            {
                var query = _commentRepository.GetAllQuery()
                    .Where(c => c.PostId == postId)
                    .OrderByDescending(c => c.CreatedAt);

                var listDtos = await query
                    .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return listDtos;
            }
            catch (Exception)
            {
                return new List<CommentDto>();
            }
        }
    }
}