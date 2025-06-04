using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using RssApp.Application.Extensions.System.Collections.ObjectModel;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.ViewModels.FeedFilter;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RssApp.Native.Windows.Views.Dialogs.FeedFilter;
public sealed partial class FeedFilterCreateDialog : ContentDialog
{
    public FeedFilterCreateViewModel ViewModel { get; }

    public FeedFilterCreateDialog(XamlRoot xamlRoot)
    {
        XamlRoot = xamlRoot;
        ViewModel = new FeedFilterCreateViewModel(App.Services);
        Loaded += async (_, _) => await ViewModel.InitAsync();
        this.InitializeComponent();
    }

    private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var addedItems = e.AddedItems.Where(x => x is FeedSubscriptionModel).Cast<FeedSubscriptionModel>();
        var removedItems = e.RemovedItems.Where(x => x is FeedSubscriptionModel).Cast<FeedSubscriptionModel>();

        ViewModel.SelectedFeedSubscriptions.AddRange(addedItems);
        foreach (var removedItem in removedItems)
        {
            ViewModel.SelectedFeedSubscriptions.Remove(removedItem);
        }
    }
}
