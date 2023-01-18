using System.Collections.Generic;

namespace PhotoGallery.API.Models.Out
{
    public class PhotoRecommendationsModel
    {
        public IEnumerable<PhotoModel> RecommendedPhotos { get; set; }
    }
}