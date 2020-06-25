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

        public async Task<CommentDTO> AddCommentAsync(CommentAddDTO commentAddDTO, int userId)
        {
            if (commentAddDTO == null)
            {
                throw null;
            }

            var photo = await _unit.Photos.GetByIdAsync(commentAddDTO.PhotoId);

            if (photo == null)
            {
                throw new ValidationException("Photo was not found");
            }

            Comment comment = _mp.Map<Comment>(commentAddDTO);
            
            comment.UserId = userId;

            await _unit.Comments.AddAsync(comment);
            await _unit.SaveAsync();

            var commentDTO = _mp.Map<CommentDTO>(comment);

            var user = await _unit.UserManager.FindByIdAsync(comment.UserId.ToString());

            commentDTO.UserName = user.UserName;

            return commentDTO;
        }

        public async Task<CommentDTO> GetCommentAsync(int commentId)
        {
            var comment = await _unit.Comments.GetByIdAsync(commentId);

            if (comment == null)
            {
                throw new ValidationException("Comment was not found");
            }

            var commentDTO = _mp.Map<CommentDTO>(comment);

            commentDTO.UserName = comment.User.UserName;

            return commentDTO;
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsAsync(int photoId)
        {
            var photo = await _unit.Photos.GetByIdAsync(photoId);

            if (photo == null)
            {
                throw new ValidationException("Photo was not found");
            }

            var comments = photo.Comments;

            var commentDTOs = new List<CommentDTO>();

            foreach (var comment in comments)
            {
                var commentDTO = _mp.Map<CommentDTO>(comment);

                commentDTO.UserName = comment.User.UserName;

                commentDTOs.Add(commentDTO);
            }

            return commentDTOs;
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