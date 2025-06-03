using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;
using RssApp.Application.Models.FeedSubscription;

namespace RssApp.Application.Messages.FeedSubscription;

public class FeedSubscriptionEditRequestMessage : RequestMessage<FeedSubscriptionModel>
{
    public FeedSubscriptionModel Model { get; }

    public FeedSubscriptionEditRequestMessage(FeedSubscriptionModel model)
    {
        Model = model;
    }
}
