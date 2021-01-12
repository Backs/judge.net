namespace Judge.Application.ViewModels.Account
{
    using System.Collections.Generic;

    public sealed class RegistrationResult
    {
        public bool Succeeded { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}