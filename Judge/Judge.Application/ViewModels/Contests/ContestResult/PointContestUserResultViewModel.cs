namespace Judge.Application.ViewModels.Contests.ContestResult
{
    public sealed class PointContestUserResultViewModel : ContestUserResultViewModelBase
    {
        public override int CompareTo(object other)
        {
            var value = (PointContestUserResultViewModel) other;
            return Score.CompareTo(value.Score);
        }
    }
}
