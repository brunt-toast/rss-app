using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.DataAccess.Context;

namespace RssApp.Application.Tests.Mock;

internal class MockFeedSubscriptionContextProvider : IFeedSubscriptionContextProvider
{
    public string FileName { get; } = $"Data Source=test-{Guid.NewGuid()}.db";

    public IFeedSubscriptionsContext New()
    {
        var ret = new FeedSubscriptionsContext(FileName);
        ret.Database.EnsureCreated();
        return ret;
    }
}
