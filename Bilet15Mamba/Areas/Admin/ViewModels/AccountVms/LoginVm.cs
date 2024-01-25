using System.ComponentModel.DataAnnotations;

namespace Bilet15Mamba.Areas.Admin.ViewModels
{
    public class LoginVm
    {
        [Required]
        [MinLength(1, ErrorMessage = "Email must be at least 1 character.")]
        [MaxLength(320, ErrorMessage = "Email must be maximum 320 characters.")]
        public string UserNameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsRemembered { get; set; }

    }
}
