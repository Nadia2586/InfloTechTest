using System;
#pragma warning disable IDE0005 // Using directive is unnecessary.
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace UserManagement.Models;

public class LogEntry
{
    public long Id { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Action { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Foreign key to User
    public long UserId { get; set; }
    public User? User { get; set; }
}
