using RssApp.Core;
using RssApp.Core.Junctions;

namespace RssApp.DataAccess.Context;

public interface IFeedSubscriptionsContext : IDisposable, IAsyncDisposable
{
    public DbSet<FeedSubscription> FeedSubscriptions { get; set; }

    public DbSet<FeedFilter> FeedFilters { get; set; }

    public DbSet<FeedSubscriptionFeedFilter> FeedSubscriptionFeedFilters { get; set; }

    public Task SaveChangesAsync();
}
