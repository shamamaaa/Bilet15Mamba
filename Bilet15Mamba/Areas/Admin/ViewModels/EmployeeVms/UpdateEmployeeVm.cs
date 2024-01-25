using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Bilet15Mamba.Areas.Admin.ViewModels
{
    public class UpdateEmployeeVm
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
        [MaxLength(64, ErrorMessage = "Name must be maximum 64 characters.")]
        public string Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Surname must be at least 3 characters.")]
        [MaxLength(64, ErrorMessage = "Surname must be maximum 64 characters.")]
        public string Surname { get; set; }

        //relation
        [Required]
        public int PositionId { get; set; }

        //Image
        public string ImageUrl { get; set; }
        public IFormFile? Photo { get; set; }

        //Social
        public string? FbLink { get; set; }
        public string? TwLink { get; set; }
        public string? LinLink { get; set; }
        public string? IgLink { get; set; }

        public SelectList? Positions { get; set; }

    }
}
