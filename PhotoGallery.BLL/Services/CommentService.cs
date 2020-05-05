using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PhotoGallery.BLL.DTO;
using PhotoGallery.BLL.Exceptions;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;

namespace PhotoGallery.BLL.Services
{
    public class CommentService : Service, ICommentService
    {
        public CommentService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
        }

        public async Task<CommentDTO> AddCommentAsync(CommentAddDTO commentDTO, int userId)
        {
            if (commentDTO == null)
            {
                throw null;
            }

            var photo = await _unit.Photos.GetByIdAsync(commentDTO.PhotoId);

            if (photo == null)
            {
                throw new ValidationException("Photo was not found");
            }

            Comment comment = _mp.Map<Comment>(commentDTO);

            await _unit.Comments.AddAsync(comment);
            await _unit.SaveAsync();

            return _mp.Map<CommentDTO>(comment);
        }

        public async Task<CommentDTO> GetCommentAsync(int commentId)
        {
            var comment = await _unit.Photos.GetByIdAsync(commentId);

            if (comment == null)
            {
                throw new ValidationException("Comment was not found");
            }

            return _mp.Map<CommentDTO>(comment);
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsAsync(int photoId)
        {
            var photo = await _unit.Photos.GetByIdAsync(photoId);

            if (photo == null)
            {
                throw new ValidationException("Photo was not found");
            }

            var comments = photo.Comments;

            return _mp.Map<IEnumerable<CommentDTO>>(comments);
        }

        public async Task RemoveCommentAsync(int commentId, int userId)
        {
            var comment = await _unit.Comments.GetByIdAsync(commentId);

            if (comment == null)
            {
                throw new ValidationException("Comment was not found");
            }

            if (comment.UserId != userId)
            {
                throw new ValidationException("You don`t have permission to delete this comment");
            }

            _unit.Comments.Remove(comment);
            await _unit.SaveAsync();
        }
    }
}