using System;
using UserManagement.Models;


public class UserDetailViewModel
{
    public long Id { get; set; }
    public string? Forename { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public List<LogEntry> Logs { get; set; } = new();

}
