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
                
        [HttpPost]
        [Route("api/register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> RegisterUser([FromBody] UserRegisterDTO userRegisterDTO) //[FromBody]
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

            return Ok("User is created.");
        }
                
        [Route("api/login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] UserLoginDTO userLoginDTO) //[FromBody]
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token;

            try
            {
                token = await _userService.LoginAsync(userLoginDTO);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(new { token });
        }

        //TODO: exceptions!
        [HttpDelete]
        [Route("api/users/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDTO>> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok();
        }




        /*
        [HttpGet("Profile")]
        public async Task<ActionResult<UserDTO>> GetProfile()
        {
            string id = User.Claims.First(c => c.Type == "UserId").Value;

            var user = await _userService.GetUserProfileAsync(id);

            if (user == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }
        */



        /*
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> UpdateProfile([FromBody] UserDTO user, int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (user.Id != id)
            {
                return BadRequest();
            }

            if (_userService.GetUserProfileAsync(id.ToString()) == null)
            {
                return NotFound();
            }

            try
            {
                await _userService.UpdateProfile(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }*/
    }
}
