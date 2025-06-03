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
using Windows.Media.Capture;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.ViewModels.FeedSubscription;
using RssApp.Native.Windows.Views.Dialogs.FeedSubscription;

namespace RssApp.Native.Windows.Controls.FeedSubscription;

public sealed partial class FeedSubscriptionListControl : UserControl
{
    public FeedSubscriptionListViewModel ViewModel { get; }

    public FeedSubscriptionListControl()
    {
        ViewModel = new FeedSubscriptionListViewModel(App.Services);
        Loaded += async (_, _) => await ViewModel.InitAsync();
        this.InitializeComponent();
    }
}
