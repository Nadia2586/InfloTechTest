namespace UserManagement.Web.Models.Logs;

public class LogDashboardViewModel
{
    public int TotalLogs { get; set; }
    public Dictionary<string, int> ActionCounts { get; set; } = new();
    public int CreateCount { get; set; }
    public int EditCount { get; set; }
    public int DeleteCount { get; set; }
    public int ViewCount { get; set; } // Optional

}
