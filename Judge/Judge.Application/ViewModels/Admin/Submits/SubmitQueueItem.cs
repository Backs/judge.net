using Judge.Model.SubmitSolution;

namespace Judge.Application.ViewModels.Admin.Submits
{
    public sealed class SubmitQueueItem : Judge.Application.ViewModels.Submit.SubmitQueueItem
    {
        public string TaskLabel { get; }
        public int? ContestId { get; }
        public string RunDescription { get; }
        public string RunOutput { get; }

        public SubmitQueueItem(SubmitResult submitResult, string language, string problemName, string taskLabel, int? contestId, string userName)
            : base(submitResult, language, problemName, userName)
        {
            TaskLabel = taskLabel;
            ContestId = contestId;
            ResultsEnabled = true;
            RunDescription = submitResult.RunDescription;
            RunOutput = submitResult.RunOutput;
        }
    }
}
