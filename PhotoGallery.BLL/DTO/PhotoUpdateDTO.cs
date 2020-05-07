﻿using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.BLL.DTO
{
    public class PhotoUpdateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name should be specified")]
        [MaxLength(50, ErrorMessage = "Photo name should be less than 50 characters")]
        public string Name { get; set; }
    }
}
