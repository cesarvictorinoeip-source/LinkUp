using LinkUp.Core.Application.Dtos.Battleship.Post;
using LinkUp.Core.Domain.Entities;

namespace LinkUp.Core.Application.Interfaces
{
    public interface IPostService : IGenericService<Post, PostDto>
    {
        Task<List<PostDto>> GetAllByUser(string userId);
        Task<List<PostDto>> GetAllForHome(List<string> userIds);
    }
}