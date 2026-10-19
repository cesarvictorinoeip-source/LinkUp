using LinkUp.Core.Application.Dtos.Battleship.Comment;
using LinkUp.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkUp.Core.Application.Interfaces
{
    // Al heredar de IGenericService, YA TIENES AddAsync, GetByIdAsync, etc.
    public interface ICommentService : IGenericService<Comment, CommentDto>
    {
        // SOLO lo que no existe en la base genérica
        Task<List<CommentDto>> GetCommentsByPost(int postId);
    }
}