using System;

namespace UserManagement.Web.Models.Logs;

public class LogListItemViewModel
{
    public long Id { get; set; }
    public string ActionName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}
