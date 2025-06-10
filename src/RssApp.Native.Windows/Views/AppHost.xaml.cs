using System;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using RssApp.Application.Messages.FeedFilter;
using RssApp.Application.Messages.FeedSubscription;
using RssApp.Application.Services.Dialogs;
using RssApp.Native.Windows.Extensions.Microsoft.UI.Xaml.Controls;
using RssApp.Native.Windows.Views.Dialogs.FeedFilter;
using RssApp.Native.Windows.Views.Dialogs.FeedSubscription;

namespace RssApp.Native.Windows.Views;

public sealed partial class AppHost : Page
{
    public AppHost()
    {
        this.InitializeComponent();

        App.Services.GetRequiredService<IDialogService>().ShowErrorRequested += OnShowErrorRequested;
        WeakReferenceMessenger.Default.Register<FeedSubscriptionEditRequestMessage>(this, OnFeedSubscriptionEditRequestMessage);
        WeakReferenceMessenger.Default.Register<FeedSubscriptionDeleteRequestMessage>(this, OnFeedSubscriptionDeleteRequestMessage);
        WeakReferenceMessenger.Default.Register<FeedFilterDeleteRequestMessage>(this, OnFeedFilterDeleteRequestMessage);
    }

    private async void OnFeedFilterDeleteRequestMessage(object recipient, FeedFilterDeleteRequestMessage message)
    {
        var dialog = new FeedFilterDeleteDialog(XamlRoot);
        dialog.ViewModel.Init(message.Model);
        await dialog.ShowAsync();
    }

    private async void OnShowErrorRequested(object? sender, ShowErrorRequestedEventArgs e)
    {
        ContentDialog d = new() { XamlRoot = XamlRoot, Title = e.Title, Content = e.Message, PrimaryButtonText = "OK" };
        await d.QueueShowAsync();
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
}
