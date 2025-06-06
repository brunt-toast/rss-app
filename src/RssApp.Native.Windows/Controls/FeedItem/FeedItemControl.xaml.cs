using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using RssApp.Application.Models.FeedItem;
using RssApp.Application.ViewModels.FeedItem;
using Windows.UI.WebUI;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RssApp.Native.Windows.Controls.FeedItem;

public sealed partial class FeedItemControl : UserControl
{
    private FeedItemViewModel ViewModel { get; set; }

    public FeedItemControl()
    {
        ViewModel = new FeedItemViewModel(App.Services);
        this.InitializeComponent();
    }

    public async Task InitAsync(FeedItemModel feedItem)
    {
        await WebView.EnsureCoreWebView2Async();
        await ViewModel.InitAsync(feedItem);
        ViewModel.OpenInBrowserCommand.Execute(null);
    }
}
