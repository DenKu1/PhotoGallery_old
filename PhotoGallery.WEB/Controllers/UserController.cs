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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("api/users")]
        [Authorize(Roles = "Admin")]       
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("api/users/{id}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            UserDTO userDTO;

            try
            {
                userDTO = await _userService.GetUserAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(userDTO);
        }

        [HttpGet]
        [Route("api/users/by-user-name/{username}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<UserDTO>> GetUserByUserName(string userName)
        {
            if (userName == null)
            {
                return BadRequest();
            }

            UserDTO userDTO;

            try
            {
                userDTO = await _userService.GetUserByUserNameAsync(userName);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(userDTO);
        }

        [HttpPost]
        [Route("api/register")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterUser([FromBody] UserRegisterDTO userRegisterDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.CreateUserAsync(userRegisterDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
                
        [HttpPost]
        [Route("api/login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            (string, UserDTO) loginData;

            try
            {
                loginData = await _userService.LoginAsync(userLoginDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(new
            {
                id = loginData.Item2.Id,
                userName = loginData.Item2.UserName,
                email = loginData.Item2.Email,
                roles = loginData.Item2.Roles.Select(role => role.ToLower()),
                token = loginData.Item1
            });
        }

        [HttpDelete]
        [Route("api/users/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDTO>> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}
