﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
#pragma warning disable IDE0005 // Using directive is unnecessary.
using Microsoft.Extensions.Logging.Abstractions;
#pragma warning restore IDE0005 // Using directive is unnecessary.
using UserManagement.Models;


namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseInMemoryDatabase("UserManagement.Data.DataContext");

    protected override void OnModelCreating(ModelBuilder model)
        => model.Entity<User>().HasData(new[]
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", DateOfBirth = new DateTime(1989, 6, 12), Email = "ploew@example.com", IsActive = true },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", DateOfBirth = new DateTime(1994, 2, 10), Email = "bfgates@example.com", IsActive = true },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", DateOfBirth = new DateTime(1980, 12, 22), Email = "ctroy@example.com", IsActive = false },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", DateOfBirth = new DateTime(1982, 1, 12), Email = "mraines@example.com", IsActive = true },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", DateOfBirth = new DateTime(1982, 1, 14),Email = "sgodspeed@example.com", IsActive = true },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", DateOfBirth = new DateTime(2000, 9, 15), Email = "himcdunnough@example.com", IsActive = true },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", DateOfBirth = new DateTime(1985, 12, 25), Email = "cpoe@example.com", IsActive = false },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", DateOfBirth = new DateTime(1990, 8, 2), Email = "emalus@example.com", IsActive = false },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", DateOfBirth = new DateTime(1996, 3, 3), Email = "dmacready@example.com", IsActive = false },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", DateOfBirth = new DateTime(1970, 11, 20), Email = "jblaze@example.com", IsActive = true },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", DateOfBirth = new DateTime(2002, 6, 25), Email = "rfeld@example.com", IsActive = true },
        });

    public DbSet<User> Users { get; set; }
    public DbSet<UserManagement.Models.LogEntry>? LogEntries { get; set; }


    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        => base.Set<TEntity>();

    public void Create<TEntity>(TEntity entity) where TEntity : class
    {
        base.Add(entity);
        SaveChanges();
    }

    public new void Update<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        SaveChanges();
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        SaveChanges();
    }


    public void Log(UserManagement.Models.LogEntry entry)
    {
        base.Add(entry);
        SaveChanges();
    }

    public IQueryable<UserManagement.Models.LogEntry> GetAllLogs()
    {
        return LogEntries!.Include(l => l.User);
    }



}
