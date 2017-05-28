using Judge.Application.ViewModels.Contests.ContestsList;
using Judge.Application.ViewModels.Problems.Statement;

namespace Judge.Application.ViewModels.Contests
{
    public sealed class ContestStatementViewModel : StatementViewModel
    {
        public ContestItem Contest { get; set; }
        public string Label { get; set; }
    }
}