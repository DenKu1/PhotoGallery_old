using System.ComponentModel.DataAnnotations.Schema;

using PhotoGallery.DAL.Entities.Base;

namespace PhotoGallery.DAL.Entities
{
    public class Recommended : BaseEntity<int>
    {
        public virtual Photo Photo { get; set; }
        public virtual Photo RecommendedPhoto { get; set; }
        [ForeignKey("RecommendedPhoto")]
        public int RecommendedPhotoId { get; set; }
    }
}
