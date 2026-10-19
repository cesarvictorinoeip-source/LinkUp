using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinkUp.Core.Application.Dtos.Battleship.Post;
using LinkUp.Core.Application.Interfaces;
using LinkUp.Core.Application.Interfaces.Repositories;
using LinkUp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkUp.Core.Application.Services
{
    public class PostService : GenericService<Post, PostDto>, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository, IMapper mapper) : base(postRepository, mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<List<PostDto>> GetAllByUser(string userId)
        {
            return await _postRepository.GetAllQuery()
                .Where(p => p.UserId == userId)
                .ProjectTo<PostDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<PostDto>> GetAllForHome(List<string> userIds)
        {
            var posts = await _postRepository.GetAllQueryWithInclude(new List<string> { "User" })
                .Where(p => userIds.Contains(p.UserId))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return posts.Select(p => new PostDto
            {
                Id = p.Id,
                Content = p.Content,
                ImageUrl = p.ImageUrl,
                ImagePath = p.ImageUrl,
                YoutubeUrl = p.YoutubeUrl,
                UserId = p.UserId,
                CreatedAt = p.CreatedAt,
                AuthorName = p.User?.UserName ?? p.UserId,
                AuthorImage = p.User?.ProfileImage ?? "SIN_FOTO"
            }).ToList();
        }
    }
}