using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.ViewModels.Feed;

namespace RssApp.Application.Messages.Feed;

public class FeedSelectedMessage
{
    public FeedSelectedMessage(FeedSubscriptionModel? model)
    {
        Model = model;
    }

    public FeedSubscriptionModel? Model { get; }
}
