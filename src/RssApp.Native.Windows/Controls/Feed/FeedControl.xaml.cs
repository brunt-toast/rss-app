using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using RssApp.Application.Messages.Feed;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.ViewModels.Feed;

namespace RssApp.Native.Windows.Controls.Feed;

public sealed partial class FeedControl : UserControl
{
    public FeedViewModel ViewModel { get; }

    public FeedControl()
    {
        ViewModel = new FeedViewModel(App.Services);
        this.InitializeComponent();
    }

    public async Task InitAsync(FeedSubscriptionModel model)
    {
        await ViewModel.InitAsync(model);
    }
}
