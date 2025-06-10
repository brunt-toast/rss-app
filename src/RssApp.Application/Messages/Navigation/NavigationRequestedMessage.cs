namespace RssApp.Application.Messages.Navigation;

public class NavigationRequestedMessage
{
    public NavigationRequestedMessage(object target)
    {
        Target = target;
    }

    public object Target { get; }
}
