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
        IServiceHelper helper;

        JwtSettings jwtSettings;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork, IServiceHelper helper, IOptions<JwtSettings> jwtSettings)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.helper = helper;
            this.jwtSettings = jwtSettings.Value;
        }

        public async Task<UserDTO> GetUserAsync(int userId)
        {
            var user = await unitOfWork.Users.GetByIdAsync(userId);
            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(user);

            var roles = unitOfWork.UserManager.GetRolesAsync(user).Result.ToArray();

            return mapper.Map<UserDTO>(user, opt =>
            {
                opt.Items["roles"] = roles; 
            });
        }

        public async Task<UserDTO> GetUserByUserNameAsync(string userName)
        {
            var user = await unitOfWork.Users.GetByUserNameAsync(userName);
            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(user);

            var roles = unitOfWork.UserManager.GetRolesAsync(user).Result.ToArray();

            return mapper.Map<UserDTO>(user, opt => opt.Items["roles"] = roles);
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            var users = await unitOfWork.Users.GetAllAsync();

            return users.Select(u => mapper.Map<UserDTO>(u, opt =>
                opt.Items["roles"] = unitOfWork.UserManager.GetRolesAsync(u).Result.ToArray()));
        }

        public async Task CreateUserAsync(UserRegisterDTO userRegisterDTO)
        {
            var userByMail = await unitOfWork.UserManager.FindByEmailAsync(userRegisterDTO.Email);
            helper.ThrowPhotoGalleryFailedRegisterExceptionIfModelIsNotNull(userByMail, "User with this email already exists");

            var userByName = await unitOfWork.UserManager.FindByNameAsync(userRegisterDTO.UserName);
            helper.ThrowPhotoGalleryFailedRegisterExceptionIfModelIsNotNull(userByName, "User with this username already exists");

            var user = mapper.Map<User>(userRegisterDTO);

            await unitOfWork.UserManager.CreateAsync(user, userRegisterDTO.Password);
            await unitOfWork.UserManager.AddToRoleAsync(user, "User");
        }

        public async Task<UserWithTokenDTO> LoginAsync(UserLoginDTO userLoginDTO)
        {
            var user = await unitOfWork.UserManager.FindByNameAsync(userLoginDTO.UserName);
            var isPasswordValid = await unitOfWork.UserManager.CheckPasswordAsync(user, userLoginDTO.Password);
            helper.ThrowPhotoGalleryFailedLoginExceptionIfModelIsNullOrPasswordInvalid(user, isPasswordValid);

            var token = GenerateJwtToken(user);
            var roles = unitOfWork.UserManager.GetRolesAsync(user).Result.ToArray();

            return mapper.Map<UserWithTokenDTO>(user, opt => { opt.Items["roles"] = roles; opt.Items["token"] = token; });
        }       

        public async Task RemoveUserAsync(int userId)
        {
            var user = await unitOfWork.UserManager.FindByIdAsync(userId.ToString());
            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(user);

            // Delete user`s likes before deleting user account
            IEnumerable<Like> userLikes = await unitOfWork.Likes.FindAsync(like => like.UserId == userId);
            unitOfWork.Likes.RemoveRange(userLikes);

            // Delete user`s comments before deleting user account
            IEnumerable<Comment> userComments = await unitOfWork.Comments.FindAsync(comment => comment.UserId == userId);
            unitOfWork.Comments.RemoveRange(userComments);

            await unitOfWork.UserManager.DeleteAsync(user);
        }

        private string GenerateJwtToken(User user)
        {       
            var claims = unitOfWork.UserManager.GetRolesAsync(user).Result.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
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
    }
}
