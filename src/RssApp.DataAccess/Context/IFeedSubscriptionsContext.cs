using RssApp.Core;

namespace RssApp.DataAccess.Context;

public interface IFeedSubscriptionsContext : IDisposable, IAsyncDisposable
{
    public DbSet<FeedSubscription> FeedSubscriptions { get; set; }

    public Task SaveChangesAsync();
}
