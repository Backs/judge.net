using System.Collections.Generic;

namespace Judge.Application.ViewModels.Account
{
    public sealed class RegistrationResult
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
