using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoGallery.BLL.DTO;
using PhotoGallery.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoGallery.WEB.Controllers
{
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        private int UserId => int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [Route("api/photos/{id}/comments")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetComments(int id)
        {
            IEnumerable<CommentDTO> comments;

            try
            {
                comments = await _commentService.GetCommentsAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(comments);
        }

        [HttpGet]
        [Route("api/comments/{id}")]
        public async Task<ActionResult<CommentDTO>> GetComment(int id)
        {
            CommentDTO comment;

            try
            {
                comment = await _commentService.GetCommentAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(comment);
        }

        [HttpPost]
        [Route("api/photos/{id}/comments")]
        public async Task<ActionResult> PostComment(int id, [FromBody] CommentAddDTO commentAddDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (commentAddDTO.PhotoId != id)
            {
                return BadRequest();            
            }

            CommentDTO commentDTO;

            try
            {
                commentDTO = await _commentService.AddCommentAsync(commentAddDTO, UserId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction(nameof(GetComment), new { id = commentDTO.Id }, commentDTO);
        }

        [HttpDelete]
        [Route("api/comments/{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            try
            {
                await _commentService.RemoveCommentAsync(id, UserId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

    }
}
