namespace Judge.Application.ViewModels
{
    public sealed class PagingViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
