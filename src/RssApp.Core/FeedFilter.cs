using RssApp.Core.Enums.Flags;

namespace RssApp.Core;

public class FeedFilter
{
    public Guid FeedFilterId { get; set; }
    public string FilterRegex { get; set; } = "";
    public bool IsWhitelist { get; set; }
    public FeedFilterAppliesToFlags AppliesTo { get; set; }
}
