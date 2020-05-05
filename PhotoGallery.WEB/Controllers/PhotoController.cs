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
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        private int UserId => int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet]
        [Route("api/albums/{id}/photos")]
        public async Task<ActionResult<IEnumerable<PhotoDTO>>> GetPhotos(int id)
        {
            IEnumerable<PhotoDTO> photos;           

            try
            {
                photos = await _photoService.GetPhotosAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(photos);
        }

        [HttpGet]
        [Route("api/photos/{id}")]
        public async Task<ActionResult<PhotoDTO>> GetPhoto(int id)
        {
            PhotoDTO photo;

            try
            {
                photo = await _photoService.GetPhotoAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(photo);
        }

        [HttpPost]
        [Route("api/albums/{id}/photos")]
        public async Task<ActionResult> PostPhoto(int id, [FromBody] PhotoAddDTO photoAddDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != photoAddDTO.AlbumId)
            {
                return BadRequest();
            }

            PhotoDTO photoDTO;

            try
            {
                photoDTO = await _photoService.AddPhotoAsync(photoAddDTO, UserId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return CreatedAtAction(nameof(GetPhoto), new { id = photoDTO.Id }, photoDTO);
        }

        [HttpPost]
        [Route("api/photos/{id}/like")]
        public async Task<ActionResult> LikePhoto(int id)
        {
            try
            {
                await _photoService.LikePhotoAsync(id, UserId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPut]
        [Route("api/photos/{id}")]
        public async Task<ActionResult> PutPhoto(int id, [FromBody] PhotoUpdateDTO photoUpdateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (photoUpdateDTO.Id != id)
            {
                return BadRequest();
            }

            try
            {
                await _photoService.UpdatePhotoAsync(photoUpdateDTO, UserId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpDelete]
        [Route("api/photos/{id}")]
        public async Task<ActionResult> DeletePhoto(int id)
        {
            try
            {
                await _photoService.RemovePhotoAsync(id, UserId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}
