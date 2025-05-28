using CommunityToolkit.Mvvm.ComponentModel;

namespace RssApp.Application.Models.FeedSubscription;

public partial class FeedSubscriptionModel : ObservableObject
{
    private readonly Core.FeedSubscription _poco;

    public string FeedName
    {
        get => _poco.FeedName;
        set
        {
            if (EqualityComparer<string>.Default.Equals(_poco.FeedName, value))
            {
                return;
            }

            OnPropertyChanging();
            _poco.FeedName = value;
            OnPropertyChanged();
        }
    }

    public string FeedUri
    {
        get => _poco.FeedUri;
        set
        {
            if (EqualityComparer<string>.Default.Equals(_poco.FeedUri, value))
            {
                return;
            }

            OnPropertyChanging();
            _poco.FeedUri = value;
            OnPropertyChanged();
        }
    }

    public FeedSubscriptionModel(Core.FeedSubscription poco)
    {
        _poco = poco;
    }

    public Core.FeedSubscription ToPoco() => _poco;
}
