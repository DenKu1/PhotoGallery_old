using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using PhotoGallery.BLL.Interfaces;
using PhotoGallery.WEB.Controllers.Base;
using PhotoGallery.WEB.Models.In;
using PhotoGallery.WEB.Models.Out;
using PhotoGallery.BLL.DTO.In;

namespace PhotoGallery.WEB.Controllers
{
    [ApiController]
    [Authorize]
    public class PhotoController : ControllerBaseWithUser
    {
        IMapper mapper;
        IPhotoService photoService;

        public PhotoController(IMapper mapper, IPhotoService photoService)
        {
            this.mapper = mapper;
            this.photoService = photoService;
        }

        [HttpGet]
        [Route("api/albums/{albumId}/photos")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<PhotoModel>>> GetPhotos([FromRoute] int albumId)
        {
            var photoDTOs = await photoService.GetPhotosAsync(albumId);

            return Ok(mapper.Map<IEnumerable<PhotoModel>>(photoDTOs));
        }

        [HttpGet]
        [Route("api/photos/{photoId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<PhotoModel>> GetPhoto([FromRoute] int photoId)
        {
            var photoDTO = await photoService.GetPhotoAsync(photoId);

            return Ok(mapper.Map<PhotoModel>(photoDTO));
        }

        [HttpPost]
        [Route("api/albums/{albumId}/photos")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<PhotoModel>> PostPhoto([FromRoute] int albumId, [FromBody] PhotoAddModel photoAddModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var photoAddDTO = mapper.Map<PhotoAddDTO>(photoAddModel,
                opt => { opt.Items["albumId"] = albumId; opt.Items["userId"] = UserId; });

            return Ok(mapper.Map<PhotoModel>(await photoService.AddPhotoAsync(photoAddDTO)));
        }

        [HttpPost]
        [Route("api/photos/{photoId}/like")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> LikePhoto([FromRoute] int photoId)
        {
            await photoService.LikePhotoAsync(photoId, UserId);

            return Ok();
        }

        [HttpPut]
        [Route("api/photos/{photoId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> PutPhoto([FromRoute] int photoId, [FromBody] PhotoUpdateModel photoUpdateModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var photoUpdateDTO = mapper.Map<PhotoUpdateDTO>(photoUpdateModel,
                opt => { opt.Items["photoId"] = photoId; opt.Items["userId"] = UserId; });

            await photoService.UpdatePhotoAsync(photoUpdateDTO);

            return Ok();
        }

        [HttpDelete]
        [Route("api/photos/{photoId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> DeletePhoto([FromRoute] int photoId)
        {
            await photoService.RemovePhotoAsync(photoId, UserId);

            return Ok();
        }
    }
}
