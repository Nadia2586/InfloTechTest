#pragma warning disable IDE0005 // Using directive is unnecessary.
using System.Collections.Generic;
#pragma warning restore IDE0005 // Using directive is unnecessary.
using System.Linq;
using System.Threading.Tasks;

using System.Threading;

using Microsoft.EntityFrameworkCore;
#pragma warning disable IDE0005 // Using directive is unnecessary.
using Microsoft.Extensions.Logging.Abstractions;
using UserManagement.Models;
#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace UserManagement.Data;

public interface IDataContext
{
    /// <summary>
    /// Get a list of items
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;

    /// <summary>
    /// Create a new item
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    void Create<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Uodate an existing item matching the ID
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    void Update<TEntity>(TEntity entity) where TEntity : class;

    void Delete<TEntity>(TEntity entity) where TEntity : class;

    void Log(UserManagement.Models.LogEntry entry);
    IQueryable<UserManagement.Models.LogEntry> GetAllLogs();

    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}
