using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Judge.Application.ViewModels.Submit
{
    public sealed class FileValidationAttribute : ValidationAttribute
    {
        private static readonly string[] ForbiddenExt = { ".exe" };

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;

            if (file == null)
                return true;

            if (ForbiddenExt.Any(o => file.FileName.EndsWith(o, System.StringComparison.InvariantCultureIgnoreCase)))
            {
                ErrorMessage = "Выберите файл с исходным кодом вашего решения";
                return false;
            }

            return true;
        }
    }
}
