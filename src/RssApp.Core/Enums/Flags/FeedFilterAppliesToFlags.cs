namespace RssApp.Core.Enums.Flags;

[Flags]
public enum FeedFilterAppliesToFlags
{
    Uri = 0x0001,
    Title = 0x0002,
    Body = 0x0004
}
