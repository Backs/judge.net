using System.ComponentModel.DataAnnotations;

namespace Judge.Application.ViewModels.Account
{
    public sealed class RegisterViewModel
    {

        [Display(Name = "Email")]
        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        [Display(Name = "Имя пользователя")]
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}
