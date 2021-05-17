using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.DTO.Out;
using PhotoGallery.BLL.Exceptions;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;

namespace PhotoGallery.BLL.Services
{
    public class CommentService : ICommentService
    {
        IMapper mapper;
        IUnitOfWork unitOfWork;

        public CommentService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<CommentDTO> AddCommentAsync(CommentAddDTO commentAddDTO)
        {
            var photo = await unitOfWork.Photos.GetByIdAsync(commentAddDTO.PhotoId);

            if (photo == null)
            {
                throw new PhotoGalleryNotFoundException("Photo was not found");
            }

            var comment = mapper.Map<Comment>(commentAddDTO);            

            await unitOfWork.Comments.AddAsync(comment);
            await unitOfWork.SaveAsync();

            var commentDTO = mapper.Map<CommentDTO>(comment);

            return commentDTO;
        }

        public async Task<CommentDTO> GetCommentAsync(int commentId)
        {
            var comment = await unitOfWork.Comments.GetByIdAsync(commentId);

            if (comment == null)
            {
                throw new PhotoGalleryNotFoundException("Comment was not found");
            }

            var commentDTO = mapper.Map<CommentDTO>(comment);

            return commentDTO;
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsAsync(int photoId)
        {
            var comments = await unitOfWork.Comments.Find(c => c.PhotoId == photoId);

            return mapper.Map<IEnumerable<CommentDTO>>(comments);
        }

        public async Task RemoveCommentAsync(int commentId, int userId)
        {
            var comment = await unitOfWork.Comments.GetByIdAsync(commentId);

            if (comment == null)
            {
                throw new PhotoGalleryNotFoundException("Comment was not found");
            }

            if (comment.UserId != userId)
            {
                throw new PhotoGalleryNotAllowedException("You don`t have permission to delete this comment");
            }

            unitOfWork.Comments.Remove(comment);
            await unitOfWork.SaveAsync();
        }
    }
}