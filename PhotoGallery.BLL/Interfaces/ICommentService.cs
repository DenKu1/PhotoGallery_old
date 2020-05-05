﻿using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoGallery.BLL.DTO;

namespace PhotoGallery.BLL.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDTO> AddCommentAsync(CommentAddDTO commentDTO, int userId);
        Task<CommentDTO> GetCommentAsync(int commentId);
        Task<IEnumerable<CommentDTO>> GetCommentsAsync(int photoId);
        Task RemoveCommentAsync(int commentId, int userId);
    }
}
