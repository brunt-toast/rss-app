namespace RssApp.Core;

public class FeedSubscription
{
    public Guid FeedSubscriptionId { get; set; } = Guid.NewGuid();
    public string FeedName { get; set; } = "";
    public string FeedUri { get; set; } = "";
}
