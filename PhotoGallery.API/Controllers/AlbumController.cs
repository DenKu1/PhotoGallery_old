using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.API.Models.In;
using PhotoGallery.API.Models.Out;
using PhotoGallery.API.Controllers.Base;
using PhotoGallery.API.Filters;

namespace PhotoGallery.API.Controllers
{
    [ApiController]    
    [Authorize]
    public class AlbumController : ControllerBaseWithUser
    {
        IMapper mapper;
        IAlbumService albumService;

        public AlbumController(IMapper mapper, IAlbumService albumService)
        {
            this.mapper = mapper;
            this.albumService = albumService;
        }

        [HttpGet]
        [Route("api/users/{userId}/albums")]
        [AllowAnonymous]//[Authorize(Roles = "User")]
        public async Task<ActionResult<IEnumerable<AlbumModel>>> GetAlbums([FromRoute] int userId)
        {
            var albumDTOs = await albumService.GetAlbumsAsync(userId);

            return Ok(mapper.Map<IEnumerable<AlbumModel>>(albumDTOs));
        }

        [HttpGet]
        [Route("api/albums/{albumId}")]
        [AllowAnonymous]//[Authorize(Roles = "User")]
        public async Task<ActionResult<AlbumModel>> GetAlbum([FromRoute] int albumId)
        {
            var albumDTO = await albumService.GetAlbumAsync(albumId);

            return Ok(mapper.Map<AlbumModel>(albumDTO));
        }

        [HttpPost]
        [Route("api/albums")]
        [Authorize(Roles = "User")]
        [ModelStateValidation]
        public async Task<ActionResult<AlbumModel>> PostAlbum([FromBody] AlbumAddModel albumAddModel)
        {         
            var albumAddDTO = mapper.Map<AlbumAddDTO>(albumAddModel, opt => opt.Items["userId"] = UserId);

            return Ok(mapper.Map<AlbumModel>(await albumService.AddAlbumAsync(albumAddDTO)));
        }

        [HttpPut]
        [Route("api/albums/{albumId}")]
        [Authorize(Roles = "User")]
        [ModelStateValidation]
        public async Task<ActionResult> PutAlbum([FromRoute] int albumId, [FromBody] AlbumUpdateModel albumUpdateModel)
        {
            var albumUpdateDTO = mapper.Map<AlbumUpdateDTO>(albumUpdateModel,
                opt => { opt.Items["albumId"] = albumId; opt.Items["userId"] = UserId; });

            await albumService.UpdateAlbumAsync(albumUpdateDTO);

            return Ok();
        }

        [HttpDelete]
        [Route("api/albums/{albumId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> DeleteAlbum([FromRoute] int albumId)
        {
            await albumService.RemoveAlbumAsync(albumId, UserId);

            return Ok();
        }

    }
}
