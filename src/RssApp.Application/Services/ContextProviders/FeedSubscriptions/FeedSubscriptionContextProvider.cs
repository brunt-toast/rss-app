using RssApp.Application.Services.AppSettings;
using RssApp.DataAccess.Context;

namespace RssApp.Application.Services.ContextProviders.FeedSubscriptions;

public class FeedSubscriptionContextProvider : IFeedSubscriptionContextProvider
{
    private readonly IAppSettings _appSettings;

    public FeedSubscriptionContextProvider(IAppSettings appSettings)
    {
        _appSettings = appSettings;
    }

    public IFeedSubscriptionsContext New()
    {
        return new FeedSubscriptionsContext(_appSettings.SqlConnectionString);
    }
}
