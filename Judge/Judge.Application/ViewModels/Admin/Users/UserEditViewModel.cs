using System.ComponentModel.DataAnnotations;

namespace Judge.Application.ViewModels.Admin.Users
{
    public sealed class UserEditViewModel
    {
        public long Id { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "UserName")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "EnterUserName")]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "UserEmail")]
        [MaxLength(256)]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "EnterValidEmail")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "NewPassword")]
        public string NewPassword { get; set; }
    }
}
