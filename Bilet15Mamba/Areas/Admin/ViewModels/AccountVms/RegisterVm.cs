using System.ComponentModel.DataAnnotations;

namespace Bilet15Mamba.Areas.Admin.ViewModels
{
    public class RegisterVm
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
        [MaxLength(64, ErrorMessage = "Name must be maximum 64 characters.")]
        public string Name { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Surname must be at least 3 characters.")]
        [MaxLength(64, ErrorMessage = "Surname must be maximum 64 characters.")]
        public string Surname { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Username must be at least 1 character.")]
        [MaxLength(128, ErrorMessage = "Username must be maximum 128 characters.")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(5, ErrorMessage = "Email must be at least 5 characters.")]
        [MaxLength(320, ErrorMessage = "Email must be maximum 320 characters.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}
