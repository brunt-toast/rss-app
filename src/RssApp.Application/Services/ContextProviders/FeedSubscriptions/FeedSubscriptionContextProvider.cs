using RssApp.Application.Services.AppSettings;
using RssApp.DataAccess.Context;

namespace RssApp.Application.Services.ContextProviders.FeedSubscriptions;

public class FeedSubscriptionContextProvider : IFeedSubscriptionContextProvider
{
    private readonly string _connectionString;

    public FeedSubscriptionContextProvider(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IFeedSubscriptionsContext New()
    {
        var ret = new FeedSubscriptionsContext(_connectionString);
        ret.Database.EnsureCreated();
        return ret;
    }
}
