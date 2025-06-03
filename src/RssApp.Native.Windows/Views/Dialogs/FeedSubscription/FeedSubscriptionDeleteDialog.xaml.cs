using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.ViewModels.FeedSubscription;


namespace RssApp.Native.Windows.Views.Dialogs.FeedSubscription;

public sealed partial class FeedSubscriptionDeleteDialog : ContentDialog
{
    public FeedSubscriptionDeleteViewModel ViewModel { get; }

    public FeedSubscriptionDeleteDialog(XamlRoot xamlRoot)
    {
        XamlRoot = xamlRoot;
        ViewModel = new FeedSubscriptionDeleteViewModel(App.Services);
        this.InitializeComponent();
    }

    public void Init(FeedSubscriptionModel model)
    {
        ViewModel.Init(model);
    }
}
