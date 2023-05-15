namespace ProjectManagementSystem.ViewModels
{
    public class BookViewModel
    {
        public int CurProjectId { get; set; }
        public IEnumerable<BookIndexViewModel> BookIndexViewModels { get; set; }
    }
}
