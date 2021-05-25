using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using PhotoGallery.BLL.Interfaces;
using PhotoGallery.API.Controllers.Base;
using PhotoGallery.API.Models.In;
using PhotoGallery.API.Models.Out;
using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.DTO.Out;
using PhotoGallery.API.Filters;

namespace PhotoGallery.API.Controllers
{
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBaseWithUser
    {
        IMapper mapper;
        ICommentService commentService;

        public CommentController(IMapper mapper, ICommentService commentService)
        {
            this.mapper = mapper;
            this.commentService = commentService;
        }

        [HttpGet]
        [Route("api/photos/{photoId}/comments")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<CommentModel>>> GetComments([FromRoute] int photoId)
        {
            var commentDTOs = await commentService.GetCommentsAsync(photoId);

            return Ok(mapper.Map<IEnumerable<CommentModel>>(commentDTOs));
        }

        [HttpGet]
        [Route("api/comments/{commentId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<CommentModel>> GetComment([FromRoute] int commentId)
        {
            var commentDTO = await commentService.GetCommentAsync(commentId);

            return Ok(mapper.Map<CommentModel>(commentDTO));
        }

        [HttpPost]
        [Route("api/photos/{photoId}/comments")]
        [Authorize(Roles = "User")]
        [ModelStateValidation]
        public async Task<ActionResult<CommentModel>> PostComment([FromRoute] int photoId, [FromBody] CommentAddModel commentAddModel)
        {
            var commentAddDTO = mapper.Map<CommentAddDTO>(commentAddModel,
                opt => { opt.Items["photoId"] = photoId; opt.Items["userId"] = UserId; });

            return Ok(mapper.Map<CommentDTO>(await commentService.AddCommentAsync(commentAddDTO)));
        }

        [HttpDelete]
        [Route("api/comments/{commentId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> DeleteComment([FromRoute] int commentId)
        {
            await commentService.RemoveCommentAsync(commentId, UserId);

            return Ok();
        }

    }
}
