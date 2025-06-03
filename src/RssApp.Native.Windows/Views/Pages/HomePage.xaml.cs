using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RssApp.Application.Messages.FeedSubscription;
using RssApp.Application.ViewModels.FeedSubscription;
using RssApp.Native.Windows.Views.Dialogs.FeedSubscription;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RssApp.Native.Windows.Views.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class HomePage : Page
{
    public ICommand CreateNewSubscriptionCommand { get; }

    public HomePage()
    {
        CreateNewSubscriptionCommand = new AsyncRelayCommand(CreateNewSubscription);

        this.InitializeComponent();

        WeakReferenceMessenger.Default.Register<FeedSubscriptionEditRequestMessage>(this, OnFeedSubscriptionEditRequestMessage);
        WeakReferenceMessenger.Default.Register<FeedSubscriptionDeleteRequestMessage>(this, OnFeedSubscriptionDeleteRequestMessage);
    }

    private async void OnFeedSubscriptionEditRequestMessage(object recipient, FeedSubscriptionEditRequestMessage message)
    {
        var dialog = new FeedSubscriptionEditDialog(XamlRoot);
        await dialog.ViewModel.InitAsync(message.Model);
        await dialog.ShowAsync();
    }

    private async void OnFeedSubscriptionDeleteRequestMessage(object recipient, FeedSubscriptionDeleteRequestMessage message)
    {
        var dialog = new FeedSubscriptionDeleteDialog(XamlRoot);
        dialog.ViewModel.Init(message.Model);
        await dialog.ShowAsync();
    }

    private async Task CreateNewSubscription()
    {
        await new FeedSubscriptionCreateDialog(XamlRoot).ShowAsync();
    }
}
