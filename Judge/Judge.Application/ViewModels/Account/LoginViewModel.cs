using System.ComponentModel.DataAnnotations;

namespace Judge.Application.ViewModels.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}
