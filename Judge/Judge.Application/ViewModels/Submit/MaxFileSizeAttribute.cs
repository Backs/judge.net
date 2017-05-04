using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Judge.Application.ViewModels.Submit
{
    public sealed class MaxFileSizeAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            return file?.ContentLength <= _maxFileSize;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Размер файла не может превышать {_maxFileSize / 1024.0} килобайт";
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(_maxFileSize.ToString()),
                ValidationType = "filesize"
            };
            rule.ValidationParameters["maxsize"] = _maxFileSize;
            yield return rule;
        }
    }
}
