using System.ComponentModel.DataAnnotations;

namespace Bilet15Mamba.Areas.Admin.ViewModels
{
    public class CreatePositionVm
    {
        [Required]
        [MinLength(1, ErrorMessage = "Name must be at least 1 character.")]
        [MaxLength(64, ErrorMessage = "Name must be maximum 64 characters.")]
        public string Name { get; set; }
    }
}
