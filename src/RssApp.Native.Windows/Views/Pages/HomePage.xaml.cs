using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
using CodeHollow.FeedReader;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Messages.Feed;
using RssApp.Application.Messages.FeedFilter;
using RssApp.Application.Messages.FeedItem;
using RssApp.Application.Messages.FeedSubscription;
using RssApp.Application.Services.Dialogs;
using RssApp.Application.ViewModels.FeedSubscription;
using RssApp.Native.Windows.Controls.Feed;
using RssApp.Native.Windows.Controls.FeedItem;
using RssApp.Native.Windows.Extensions.Microsoft.UI.Xaml.Controls;
using RssApp.Native.Windows.Views.Dialogs.FeedFilter;
using RssApp.Native.Windows.Views.Dialogs.FeedSubscription;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RssApp.Native.Windows.Views.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class HomePage : Page, INotifyPropertyChanged
{
    public ICommand CreateNewSubscriptionCommand { get; }
    public ICommand CreateNewFilterCommand { get; }

    public FeedControl FeedControl
    {
        get;
        set => SetField(ref field, value);
    } = new();

    public FeedItemControl FeedItemControl
    {
        get;
        set => SetField(ref field, value);
    } = new();

    public HomePage()
    {
        CreateNewSubscriptionCommand = new AsyncRelayCommand(CreateNewSubscription);
        CreateNewFilterCommand = new AsyncRelayCommand(CreateNewFilter);

        this.InitializeComponent();

        WeakReferenceMessenger.Default.Register<FeedSelectedMessage>(this, OnFeedSelectedMessage);
        WeakReferenceMessenger.Default.Register<FeedItemSelectedMessage>(this, OnFeedItemSelectedMessage);
    }

    private async void OnFeedItemSelectedMessage(object recipient, FeedItemSelectedMessage message)
    {
        FeedItemControl = new FeedItemControl();
        await FeedItemControl.InitAsync(message.Model);
    }

    private async void OnFeedSelectedMessage(object recipient, FeedSelectedMessage message)
    {
        FeedControl = new FeedControl();

        if (message.Model is null)
        {
            return;
        }

        await FeedControl.InitAsync(message.Model);
    }

    private async Task CreateNewSubscription()
    {
        await new FeedSubscriptionCreateDialog(XamlRoot).ShowAsync();
    }

    private async Task CreateNewFilter()
    {
        await new FeedFilterCreateDialog(XamlRoot).ShowAsync();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
