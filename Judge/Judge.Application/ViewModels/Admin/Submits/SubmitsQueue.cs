namespace Judge.Application.ViewModels.Admin.Submits
{
    using System.Collections.Generic;
    using Judge.Model.SubmitSolution;

    public sealed class SubmitsQueue
    {
        public IEnumerable<SubmitQueueItem> Submits { get; set; }

        public IEnumerable<LanguageItem> Languages { get; set; }

        public IEnumerable<SubmitStatusItem> Statuses { get; set; }

        public int? SelectedLanguage { get; set; }

        public SubmitStatus? SelectedStatus { get; set; }
    }
}
