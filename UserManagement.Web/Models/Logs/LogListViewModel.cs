namespace UserManagement.Web.Models.Logs;

public class LogListViewModel
{
    public List<LogListItemViewModel> Logs { get; set; } = new();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string? ActionFilter { get; set; }
    public string? SortBy { get; set; }
    public string? SearchTerm { get; set; }

}
