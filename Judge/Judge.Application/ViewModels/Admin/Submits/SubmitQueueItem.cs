namespace Judge.Application.ViewModels.Admin.Submits
{
    using Judge.Model.CheckSolution;
    using Judge.Model.SubmitSolution;

    public sealed class SubmitQueueItem : Judge.Application.ViewModels.Submit.SubmitQueueItem
    {
        public string TaskLabel { get; }

        public int? ContestId { get; }

        public string RunDescription { get; }

        public string RunOutput { get; }

        public SubmitQueueItem(SubmitResult submitResult, string language, Task task, string taskLabel, int? contestId, string userName)
            : base(submitResult, language, task, userName)
        {
            this.TaskLabel = taskLabel;
            this.ContestId = contestId;
            this.ResultsEnabled = true;
            this.RunDescription = submitResult.RunDescription;
            this.RunOutput = submitResult.RunOutput;
        }
    }
}
