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
using RssApp.Application.Models.FeedFilter;
using RssApp.Application.ViewModels.FeedFilter;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace RssApp.Native.Windows.Views.Dialogs.FeedFilter;
public sealed partial class FeedFilterDeleteDialog : ContentDialog
{
    public FeedFilterDeleteViewModel ViewModel { get; }

    public FeedFilterDeleteDialog(XamlRoot xamlRoot)
    {
        XamlRoot = xamlRoot;
        ViewModel = new FeedFilterDeleteViewModel(App.Services);
        this.InitializeComponent();
    }

    public void Init(FeedFilterModel model)
    {
        ViewModel.Init(model);
    }
}
