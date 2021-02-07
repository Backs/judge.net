namespace Judge.Application.ViewModels.Admin.Submits
{
    using Judge.Model.SubmitSolution;

    public sealed class SubmitsFilter
    {
        public int? SelectedLanguage { get; set; }

        public SubmitStatus? SelectedStatus { get; set; }
    }
}
