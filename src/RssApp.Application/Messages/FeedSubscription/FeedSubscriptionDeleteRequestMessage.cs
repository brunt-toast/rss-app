using CommunityToolkit.Mvvm.Messaging.Messages;
using RssApp.Application.Models.FeedSubscription;

namespace RssApp.Application.Messages.FeedSubscription;

public class FeedSubscriptionDeleteRequestMessage : RequestMessage<FeedSubscriptionModel>
{
    public FeedSubscriptionModel Model { get; }

    public FeedSubscriptionDeleteRequestMessage(FeedSubscriptionModel model)
    {
        Model = model;
    }
}
