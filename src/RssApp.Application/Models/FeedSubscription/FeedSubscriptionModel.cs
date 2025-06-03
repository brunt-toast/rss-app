using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RssApp.Application.Messages.FeedSubscription;

namespace RssApp.Application.Models.FeedSubscription;

public partial class FeedSubscriptionModel : ObservableObject
{
    private readonly Core.FeedSubscription _poco;

    public ICommand RequestEditCommand { get; }
    public ICommand RequestDeleteCommand { get; }

    public FeedSubscriptionModel(Core.FeedSubscription poco)
    {
        RequestEditCommand = new RelayCommand(RequestEdit);
        RequestDeleteCommand = new RelayCommand(RequestDelete);
        _poco = poco;
    }

    private void RequestEdit()
    {
        WeakReferenceMessenger.Default.Send(new FeedSubscriptionEditRequestMessage(this));
    }

    private void RequestDelete()
    {
        WeakReferenceMessenger.Default.Send(new FeedSubscriptionDeleteRequestMessage(this));
    }

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

    public Core.FeedSubscription ToPoco() => _poco;
}
