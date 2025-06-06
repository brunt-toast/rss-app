using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RssApp.Application.Models.FeedItem;

namespace RssApp.Application.Messages.FeedItem;

public class FeedItemSelectedMessage
{
    public FeedItemModel Model { get; }

    public FeedItemSelectedMessage(FeedItemModel model)
    {
        Model = model;
    }
}
