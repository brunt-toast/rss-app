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
using RssApp.Application.ViewModels.FeedFilter;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RssApp.Native.Windows.Controls.FeedFilter;
public sealed partial class FeedFilterListControl : UserControl
{
    public FeedFilterListViewModel ViewModel { get; }

    public FeedFilterListControl()
    {
        ViewModel = new FeedFilterListViewModel(App.Services);
        Loaded += async (_, _) => await ViewModel.InitAsync();
        this.InitializeComponent();
    }
}
