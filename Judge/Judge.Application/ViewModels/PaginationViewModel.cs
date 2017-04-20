namespace Judge.Application.ViewModels
{
    public sealed class PaginationViewModel
    {
        private const int Offset = 5;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public int FirstDisplayPage => CurrentPage - Offset < 1 ? 1 : CurrentPage - Offset;
        public int LastDisplayPage => CurrentPage + Offset > TotalPages ? TotalPages : CurrentPage + Offset;

        public bool IsCurrent(int page)
        {
            return CurrentPage == page;
        }
    }
}
