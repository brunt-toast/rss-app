using CommunityToolkit.Mvvm.ComponentModel;

namespace RssApp.Application.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected IServiceProvider Services { get; }

    public ViewModelBase(IServiceProvider services)
    {
        Services = services;
    }
}
