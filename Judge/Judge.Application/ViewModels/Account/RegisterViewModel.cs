namespace Judge.Application.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    public sealed class RegisterViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "EnterEmail")]
        [MaxLength(256)]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "EnterValidEmail")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "UserName")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "EnterUserName")]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Password")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "EnterPassword")]
        [MaxLength(100)]
        public string Password { get; set; }
    }
}