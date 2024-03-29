﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using PhotoGallery.BLL.Interfaces;
using PhotoGallery.API.Models.In;
using PhotoGallery.API.Models.Out;
using System.ComponentModel.DataAnnotations;
using PhotoGallery.BLL.DTO.In;
using PhotoGallery.API.Filters;

namespace PhotoGallery.API.Controllers
{
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        IMapper mapper;
        IUserService userService;
        ITagService tagService;

        public UserController(IMapper mapper, IUserService userService, ITagService tagService)
        {
            this.mapper = mapper;
            this.userService = userService;
            this.tagService = tagService;
        }

        [HttpGet]
        [Route("api/users")]
        [Authorize(Roles = "Admin")]       
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            var userDTOs = await userService.GetUsersAsync();

            return Ok(mapper.Map<IEnumerable<UserModel>>(userDTOs));
        }

        [HttpGet]
        [Route("api/users/{userId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<UserModel>> GetUser([FromRoute] int userId)
        {
            var userDTO = await userService.GetUserAsync(userId);

            return Ok(mapper.Map<UserModel>(userDTO));
        }

        [HttpGet]
        [Route("api/users/by-user-name/{userName}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<UserModel>> GetUserByUserName([FromRoute][Required] string userName)
        {
            var userDTO = await userService.GetUserByUserNameAsync(userName);

            return Ok(mapper.Map<UserModel>(userDTO));
        }

        [HttpPost]
        [Route("api/register")]
        [AllowAnonymous]
        [ModelStateValidation]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterModel userRegisterModel)
        {
            var userRegisterDTO = mapper.Map<UserRegisterDTO>(userRegisterModel);

            await userService.CreateUserAsync(userRegisterDTO);

            return Ok();
        }
                
        [HttpPost]
        [Route("api/login")]
        [AllowAnonymous]
        [ModelStateValidation]
        public async Task<ActionResult<UserWithTokenModel>> LoginUser([FromBody] UserLoginModel userLoginModel)
        {
            var userLoginDTO = mapper.Map<UserLoginDTO>(userLoginModel);

            var userWithTokenDTO = await userService.LoginAsync(userLoginDTO);

            return Ok(mapper.Map<UserWithTokenModel>(userWithTokenDTO));
        }
        
        [HttpPost]
        [Route("api/users/{userId}/attachTags")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> AttachUserTags([FromRoute] int userId, [FromBody] IEnumerable<string> tags)
        {
            await tagService.AddTagsToUserAsync(tags, userId);

            return Ok();
        }
        
        [HttpPost]
        [Route("api/users/{userId}/detachTag")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> DetachUserTag([FromRoute] int userId, [FromBody] string tag)
        {
            await tagService.RemoveTagFromUserAsync(tag, userId);

            return Ok();
        }

        [HttpDelete]
        [Route("api/users/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser([FromRoute] int userId)
        {
            await userService.RemoveUserAsync(userId);

            return Ok();
        }
    }
}
