using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using PhotoGallery.BLL.DTO.In;
using PhotoGallery.BLL.DTO.Out;
using PhotoGallery.BLL.Interfaces;
using PhotoGallery.DAL.Entities;
using PhotoGallery.DAL.Interfaces;

namespace PhotoGallery.BLL.Services
{
    public class TagService : ITagService
    {
        IMapper mapper;
        IUnitOfWork unitOfWork;
        IServiceHelper helper;

        public TagService(IMapper mapper, IUnitOfWork unitOfWork, IServiceHelper helper)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.helper = helper;
        }

        public async Task AddTagsToPhotoAsync(IEnumerable<string> tagNames, int photoId)
        {
            var photo = await unitOfWork.Photos.GetByIdAsync(photoId);
            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(photo);

            foreach (var tagName in tagNames)
            {
                var tag = await GetOrCreateTag(tagName);
                
                photo.Tags.Add(tag);
            }

            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<string>> GetTagsForPhotoAsync(int photoId)
        {
            var photo = await unitOfWork.Photos.GetByIdAsync(photoId);
            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(photo);

            return photo.Tags.Select(tag => tag.Name);
        }

        public async Task AddTagsToUserAsync(IEnumerable<string> tagNames, int userId)
        {
            var user = await unitOfWork.Users.GetByIdAsync(userId);
            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(user);
            
            foreach (var tagName in tagNames)
            {
                var tag = await GetOrCreateTag(tagName);
                
                user.Tags.Add(tag);
            }
            
            await unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<string>> GetTagsForUserAsync(int userId)
        {
            var user = await unitOfWork.Photos.GetByIdAsync(userId);
            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(user);

            return user.Tags.Select(tag => tag.Name);
        }

        public async Task RemoveTagFromPhotoAsync(string tagName, int photoId)
        {
            var tag = await GetTagByName(tagName);
            var photo = await unitOfWork.Photos.GetByIdAsync(photoId);
            
            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(tag);
            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(photo);

            photo.Tags.Remove(tag);

            await unitOfWork.SaveAsync();
        }

        public async Task RemoveTagFromUserAsync(string tagName, int userId)
        {
            var tag = await GetTagByName(tagName);
            var user = await unitOfWork.Users.GetByIdAsync(userId);
            
            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(tag);
            helper.ThrowPhotoGalleryNotFoundExceptionIfModelIsNull(user);

            user.Tags.Remove(tag);

            await unitOfWork.SaveAsync();
        }

        async Task<Tag> GetOrCreateTag(string tagName)
        {
            var tag = await GetTagByName(tagName);
            
            if (tag is null)
            {
                tag = new Tag { Name = tagName };
                await unitOfWork.Tags.AddAsync(tag);
            }

            return tag;
        }
        
        async Task<Tag> GetTagByName(string tagName)
        {
            return await unitOfWork.Tags.FindFirstOrDefaultAsync(tag => string.Equals(tag.Name, tagName));
        }
    }
}