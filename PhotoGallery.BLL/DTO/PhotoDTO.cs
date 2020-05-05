﻿using System;

namespace PhotoGallery.BLL.DTO
{
    public class PhotoDTO
    {
        public int Id { get; set; }
        //not shure if this prop can be useful
        public int AlbumId { get; set; }
        //in case we need to know IsOwned
        //need to specificly map
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime Created { get; set; }
        public int Likes { get; set; }
        public bool IsLiked { get; set; }
    }
}
