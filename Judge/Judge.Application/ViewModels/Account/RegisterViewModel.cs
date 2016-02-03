using System.ComponentModel.DataAnnotations;

namespace Judge.Application.ViewModels.Account
{
    public sealed class RegisterViewModel
    {

        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        public string Password { get; set; }
    }
}
