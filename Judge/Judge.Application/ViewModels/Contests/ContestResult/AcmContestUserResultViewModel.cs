namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public sealed class AcmContestUserResultViewModel : ContestUserResultViewModelBase
    {
        public override int CompareTo(object other)
        {
            var value = (AcmContestUserResultViewModel)other;
            if (SolvedCount == value.SolvedCount)
            {
                return value.Score.CompareTo(Score);
            }

            return SolvedCount.CompareTo(value.SolvedCount);
        }
    }
}
