using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PhotoGallery.BLL.Configs;
using PhotoGallery.BLL.DTO;
using PhotoGallery.BLL.Exceptions;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PhotoGallery.BLL.Services
{
    public class UserService : Service, IUserService
    {
        private readonly JwtSettings _jwtSettings;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettings) : base(mapper, unitOfWork)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var users = await _unit.Users.GetAll();

            List<UserDTO> userDTOs = users.Select(user => MapUserToDTO(user)).ToList();

            return userDTOs;
        }

        public async Task<UserDTO> GetUserAsync(int id)
        {
            User user = await _unit.Users.GetByIdAsync(id);

            if (user == null)
            {
                throw new ValidationException("User was not found");
            }

            UserDTO userDTO = MapUserToDTO(user);

            return userDTO;
        }

        public async Task<UserDTO> GetUserByUserNameAsync(string userName)
        {
            if (userName == null)
            {
                throw null;
            }

            User user = await _unit.Users.GetByUserNameAsync(userName);

            if (user == null)
            {
                throw new ValidationException("User was not found");
            }

            UserDTO userDTO = MapUserToDTO(user);

            return userDTO;
        }

        public async Task CreateUserAsync(UserRegisterDTO data)
        {
            if (data == null)
            {
                throw null;
            }

            if (await _unit.UserManager.FindByEmailAsync(data.Email) != null)
            {
                throw new ValidationException("User with this email already exists");
            }

            if (await _unit.UserManager.FindByNameAsync(data.UserName) != null)
            {
                throw new ValidationException("User with this username already exists");
            }

            User user = new User
            {
                UserName = data.UserName,
                Email = data.Email
            };

            await _unit.UserManager.CreateAsync(user, data.Password);
            await _unit.UserManager.AddToRoleAsync(user, "User");
        }

        public async Task<(string, UserDTO)> LoginAsync(UserLoginDTO data)
        {
            User user = await _unit.UserManager.FindByNameAsync(data.UserName);

            if (user == null || !await _unit.UserManager.CheckPasswordAsync(user, data.Password))
            {
                throw new ValidationException("Incorrect username or password");
            }

            UserDTO userDTO = MapUserToDTO(user);

            return (GenerateJwtToken(userDTO), userDTO);
        }       

        public async Task DeleteUserAsync(int id)
        {
            User user = await _unit.UserManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new ValidationException("User was not found");
            }

            // Delete user`s likes before deleting user account
            IEnumerable<Like> userLikes = await _unit.Likes.Find(like => like.UserId == id);
            _unit.Likes.RemoveRange(userLikes);

            // Delete user`s comments before deleting user account
            IEnumerable<Comment> userComments = await _unit.Comments.Find(comment => comment.UserId == id);
            _unit.Comments.RemoveRange(userComments);

            await _unit.UserManager.DeleteAsync(user);
        }

        private string GenerateJwtToken(UserDTO userDTO)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDTO.Id.ToString())
            };

            claims.AddRange(userDTO.Roles.Select(r => new Claim(ClaimTypes.Role, r)));

            DateTime expires = DateTime.UtcNow.Add(_jwtSettings.LifeTime);

            var key = Encoding.UTF8.GetBytes(_jwtSettings.JwtKey);
            var creds = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserDTO MapUserToDTO(User user)
        {
            var userDTO = _mp.Map<UserDTO>(user);

            var roles = _unit.UserManager.GetRolesAsync(user).Result;

            userDTO.Roles = roles.ToArray();

            return userDTO;
        }
    }
}
