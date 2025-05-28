using RssApp.DataAccess.Context;

namespace RssApp.Application.Services.ContextProviders.FeedSubscriptions;

public interface IFeedSubscriptionContextProvider
{
    public IFeedSubscriptionsContext New();
}
