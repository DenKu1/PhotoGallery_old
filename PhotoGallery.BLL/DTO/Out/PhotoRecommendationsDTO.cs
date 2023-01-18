using System.Collections.Generic;

namespace PhotoGallery.BLL.DTO.Out
{
    public class PhotoRecommendationsDTO
    {
        public IEnumerable<PhotoDTO> RecommendedPhotos { get; set; }
    }
}