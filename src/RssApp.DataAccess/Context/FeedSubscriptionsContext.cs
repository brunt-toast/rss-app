using EntityFramework.Exceptions.Sqlite;
using Microsoft.Extensions.Logging;
using RssApp.Core;

namespace RssApp.DataAccess.Context;

public class FeedSubscriptionsContext : DbContext, IFeedSubscriptionsContext
{
    private readonly string? _sqlConnectionString;

    private ILogger? _logger;
    private LogLevel _logLevel;

    public FeedSubscriptionsContext(string? sqlConnectionString)
    {
        _sqlConnectionString = sqlConnectionString;
    }

    public void RegisterLogger(ILogger logger, LogLevel logLevel)
    {
        _logger = logger;
        _logLevel = logLevel;
    }

    public virtual DbSet<FeedSubscription> FeedSubscriptions { get; set; }

    public Task SaveChangesAsync() => base.SaveChangesAsync();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite(_sqlConnectionString)
            .UseExceptionProcessor()
            .LogTo(s => _logger?.Log(_logLevel, "DB context: {log}", s))
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeedFilter>()
            .HasKey(x => x.FeedFilterId);

        modelBuilder.Entity<FeedSubscription>()
            .HasKey(x => x.FeedSubscriptionId);
    }
}
