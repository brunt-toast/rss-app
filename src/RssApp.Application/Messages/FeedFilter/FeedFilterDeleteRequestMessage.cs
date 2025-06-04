using CommunityToolkit.Mvvm.Messaging.Messages;
using RssApp.Application.Models.FeedFilter;

namespace RssApp.Application.Messages.FeedFilter;

public class FeedFilterDeleteRequestMessage : RequestMessage<FeedFilterModel>
{
    public FeedFilterModel Model { get; }

    public FeedFilterDeleteRequestMessage(FeedFilterModel model)
    {
        Model = model;
    }
}
