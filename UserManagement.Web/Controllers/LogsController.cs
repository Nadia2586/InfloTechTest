using System;
#pragma warning disable IDE0005 // Using directive is unnecessary.
using Microsoft.AspNetCore.Mvc;
#pragma warning restore IDE0005 // Using directive is unnecessary.
using UserManagement.Data;
#pragma warning disable IDE0005 // Using directive is unnecessary.
using UserManagement.Services.Domain.Interfaces;
#pragma warning restore IDE0005 // Using directive is unnecessary.
using UserManagement.Web.Models.Logs;
using System.Linq;
#pragma warning disable IDE0005 // Using directive is unnecessary.
using UserManagement.Services;
#pragma warning restore IDE0005 // Using directive is unnecessary.


namespace UserManagement.Web.Controllers;

[Route("logs")]
public class LogsController : Controller
{
    private readonly IDataContext _dataContext;
    private readonly IUserService _userService;

    public LogsController(IDataContext dataContext, IUserService userService)
    {
        _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpGet("logfull")]
    public IActionResult LogFull(string? actionFilter, string? sortBy, string? searchTerm, int page = 1, int pageSize = 10)
    {
        var logsQuery = _userService.GetAllLogs();

        // Filter by action
        if (!string.IsNullOrEmpty(actionFilter))
        {
            logsQuery = logsQuery.Where(l => l.Action == actionFilter);
        }

        // Filter by search term
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            logsQuery = logsQuery.Where(l =>
                (l.Description != null && l.Description.Contains(searchTerm)) ||
                (l.Action != null && l.Action.Contains(searchTerm)) ||
                (l.User != null && (l.User.Forename + " " + l.User.Surname).Contains(searchTerm)));
        }

        // Sorting and pagination (as already implemented)

        var totalLogs = logsQuery.Count();
        var logs = logsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var model = new LogListViewModel
        {
            Logs = logs.Select(l => new LogListItemViewModel
            {
                Id = l.Id,
                Timestamp = l.Timestamp,
                ActionName = l.Action,
                Description = l.Description ?? "",
                UserName = l.User != null ? $"{l.User.Forename} {l.User.Surname}" : "Unknown"
            }).ToList(),
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalLogs / (double)pageSize),
            ActionFilter = actionFilter,
            SortBy = sortBy,
            SearchTerm = searchTerm
        };

        return View("LogFull", model);
    }



    [HttpGet("{id}")]
    public IActionResult LogDetails(long id)
    {
        var log = _dataContext.GetAllLogs()
            .FirstOrDefault(l => l.Id == id);

        if (log == null) return NotFound();

        var model = new LogListItemViewModel
        {
            Id = log.Id,
            Timestamp = log.Timestamp,
            ActionName = log.Action,
            Description = log.Description ?? "No description provided",
            UserName = log.User != null
                ? $"{log.User.Forename} {log.User.Surname}"
                : "Unknown"

        };

        return View("LogDetails", model);
    }

    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {
        var logs = _dataContext.GetAllLogs();

        var model = new LogDashboardViewModel
        {
            TotalLogs = logs.Count(),
            CreateCount = logs.Count(l => l.Action == "Create"),
            EditCount = logs.Count(l => l.Action == "Edit"),
            DeleteCount = logs.Count(l => l.Action == "Delete"),
            ViewCount = logs.Count(l => l.Action == "View") // Optional
        };

        return View("Dashboard", model);
    }

}
