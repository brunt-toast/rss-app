using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using RssApp.Application.Messages.Navigation;

namespace RssApp.Application.Services.Navigation;

public interface INavigationService
{
    public void NavigateTo(object target);
}

public class NavigationService : INavigationService
{
    public void NavigateTo(object target)
    {
        WeakReferenceMessenger.Default.Send(new NavigationRequestedMessage(target));
    }
}
