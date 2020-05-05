using System;

namespace PhotoGallery.BLL.Configs
{
    public class JwtSettings
    {
        public string JwtKey { get; set; }
        public TimeSpan LifeTime { get; set; }
    }
}
