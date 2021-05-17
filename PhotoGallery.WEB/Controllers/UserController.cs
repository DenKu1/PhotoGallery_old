using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using PhotoGallery.BLL.Interfaces;
using PhotoGallery.WEB.Models.In;
using PhotoGallery.WEB.Models.Out;
using System.ComponentModel.DataAnnotations;
using PhotoGallery.BLL.DTO.In;

namespace PhotoGallery.WEB.Controllers
{
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        IMapper mapper;
        IUserService userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            this.mapper = mapper;
            this.userService = userService;
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
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterModel userRegisterModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userRegisterDTO = mapper.Map<UserRegisterDTO>(userRegisterModel);

            await userService.CreateUserAsync(userRegisterDTO);

            return Ok();
        }
                
        [HttpPost]
        [Route("api/login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] UserLoginModel userLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userLoginDTO = mapper.Map<UserLoginDTO>(userLoginModel);

            var userWithTokenDTO = await userService.LoginAsync(userLoginDTO);

            return Ok(mapper.Map<UserWithTokenModel>(userWithTokenDTO));
        }

        [HttpDelete]
        [Route("api/users/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserModel>> DeleteUser([FromRoute] int userId)
        {
            await userService.RemoveUserAsync(userId);

            return Ok();
        }

    }
}
