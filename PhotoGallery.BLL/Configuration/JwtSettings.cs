using System;

namespace PhotoGallery.BLL.Configuration
{
    public class JwtSettings
    {
        public string JwtKey { get; set; }
        public TimeSpan LifeTime { get; set; }
    }
}
