using Judge.Application.ViewModels.Problems.Statement;

namespace Judge.Application.ViewModels.Contests
{
    public sealed class ContestStatementViewModel : StatementViewModel
    {
        public int ContestId { get; set; }
        public string Label { get; set; }
    }
}