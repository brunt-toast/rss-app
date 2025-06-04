using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;
using RssApp.Application.Models.FeedFilter;
using RssApp.Application.Models.FeedSubscription;

namespace RssApp.Application.Messages.FeedFilter;
internal class FeedFilterEditRequestMessage : RequestMessage<FeedFilterModel>
{
    public FeedFilterModel Model { get; }

    public FeedFilterEditRequestMessage(FeedFilterModel model)
    {
        Model = model;
    }
}
