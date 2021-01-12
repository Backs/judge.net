namespace Judge.Application.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "EnterEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "EnterValidEmail")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Password")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "EnterPassword")]
        public string Password { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}