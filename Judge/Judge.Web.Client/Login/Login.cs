using System.ComponentModel.DataAnnotations;

namespace Judge.Web.Client.Login
{
    public class Login
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}