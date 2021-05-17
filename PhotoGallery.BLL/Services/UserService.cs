using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using AutoMapper;
using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.DTO.Out;
using PhotoGallery.BLL.Exceptions;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;
using PhotoGallery.BLL.Configuration;

namespace PhotoGallery.BLL.Services
{
    public class UserService : IUserService
    {
        IMapper mapper;
        IUnitOfWork unitOfWork;

        JwtSettings jwtSettings;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtSettings)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;

            this.jwtSettings = jwtSettings.Value;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var users = await unitOfWork.Users.GetAll();

            return mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserAsync(int id)
        {
            var user = await unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
            {
                throw new ValidationException("User was not found");
            }

            return mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> GetUserByUserNameAsync(string userName)
        {
            var user = await unitOfWork.Users.GetByUserNameAsync(userName);

            if (user == null)
            {
                throw new ValidationException("User was not found");
            }

            return mapper.Map<UserDTO>(user);
        }

        public async Task CreateUserAsync(UserRegisterDTO userRegisterDTO)
        {
            if (await unitOfWork.UserManager.FindByEmailAsync(userRegisterDTO.Email) != null)
            {
                throw new ValidationException("User with this email already exists");
            }

            if (await unitOfWork.UserManager.FindByNameAsync(userRegisterDTO.UserName) != null)
            {
                throw new ValidationException("User with this username already exists");
            }

            User user = new User
            {
                UserName = userRegisterDTO.UserName,
                Email = userRegisterDTO.Email
            };

            await unitOfWork.UserManager.CreateAsync(user, userRegisterDTO.Password);
            await unitOfWork.UserManager.AddToRoleAsync(user, "User");


            //TODO: Investigate where save changes????
        }

        public async Task<UserWithTokenDTO> LoginAsync(UserLoginDTO userLoginDTO)
        {
            User user = await unitOfWork.UserManager.FindByNameAsync(userLoginDTO.UserName);

            if (user == null || !await unitOfWork.UserManager.CheckPasswordAsync(user, userLoginDTO.Password))
            {
                throw new ValidationException("Incorrect username or password");
            }

            var token = GenerateJwtToken(user);

            return mapper.Map<UserWithTokenDTO>(user, opt => opt.Items["token"] = token);
        }       

        public async Task RemoveUserAsync(int userId)
        {
            var user = await unitOfWork.UserManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new ValidationException("User was not found");
            }

            //TODO: Investigate why we need this

            // Delete user`s likes before deleting user account
            IEnumerable<Like> userLikes = await unitOfWork.Likes.Find(like => like.UserId == userId);
            unitOfWork.Likes.RemoveRange(userLikes);

            // Delete user`s comments before deleting user account
            IEnumerable<Comment> userComments = await unitOfWork.Comments.Find(comment => comment.UserId == userId);
            unitOfWork.Comments.RemoveRange(userComments);

            await unitOfWork.UserManager.DeleteAsync(user);
        }

        private string GenerateJwtToken(User user)
        {
            var claims = user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name)).ToList();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            DateTime expires = DateTime.UtcNow.Add(jwtSettings.LifeTime);

            var key = Encoding.UTF8.GetBytes(jwtSettings.JwtKey);
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

/*        private UserDTO MapUserToDTO(User user)
        {
            //TODO: Remove this somehow

            var userDTO = mapper.Map<UserDTO>(user);

            var roles = unitOfWork.UserManager.GetRolesAsync(user).Result;

            userDTO.Roles = roles.ToArray();

            return userDTO;
        }*/
    }
}
