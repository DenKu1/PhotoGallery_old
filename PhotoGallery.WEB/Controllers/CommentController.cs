using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoGallery.BLL.DTO;
using PhotoGallery.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhotoGallery.WEB.Controllers
{
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        private int UserId => int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [Route("api/photos/{id}/comments")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetComments(int id)
        {
            IEnumerable<CommentDTO> commentDTOs;

            try
            {
                commentDTOs = await _commentService.GetCommentsAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(commentDTOs);
        }

        [HttpGet]
        [Route("api/comments/{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<CommentDTO>> GetComment(int id)
        {
            CommentDTO commentDTO;

            try
            {
                commentDTO = await _commentService.GetCommentAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(commentDTO);
        }

        [HttpPost]
        [Route("api/photos/{id}/comments")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<CommentDTO>> PostComment(int id, [FromBody] CommentAddDTO commentAddDTO)
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

            return Ok(commentDTO);
        }

        [HttpDelete]
        [Route("api/comments/{id}")]
        [Authorize(Roles = "User")]
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
