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
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        private int UserId => int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        [HttpGet]
        [Route("api/users/{id}/albums")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<AlbumDTO>>> GetAlbums(int id)
        {
            IEnumerable<AlbumDTO> albumDTOs;

            try
            {
                albumDTOs = await _albumService.GetAlbumsAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(albumDTOs);
        }

        [HttpGet]
        [Route("api/albums/{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<AlbumDTO>> GetAlbum(int id)
        {
            AlbumDTO albumDTO;

            try
            {
                albumDTO = await _albumService.GetAlbumAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(albumDTO);
        }

        [HttpPost]
        [Route("api/albums")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<AlbumDTO>> PostAlbum([FromBody] AlbumAddDTO albumAddDTO)
        {         
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (albumAddDTO.UserId != UserId)
            {
                return BadRequest();
            }

            AlbumDTO albumDTO;

            try
            {
                albumDTO = await _albumService.AddAlbumAsync(albumAddDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(albumDTO);
        }

        [HttpPut]
        [Route("api/albums/{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> PutAlbum(int id, [FromBody] AlbumUpdateDTO albumUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (albumUpdateDTO.Id != id)
            {
                return BadRequest();
            }

            try
            {
                await _albumService.UpdateAlbumAsync(albumUpdateDTO, UserId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpDelete]
        [Route("api/albums/{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> DeleteAlbum(int id)
        {
            try
            {
                await _albumService.RemoveAlbumAsync(id, UserId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

    }
}
