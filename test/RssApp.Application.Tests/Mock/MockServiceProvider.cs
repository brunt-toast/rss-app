using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Services.Dialogs;

namespace RssApp.Application.Tests.Mock;

internal class MockServiceProvider : IServiceProvider
{
    private readonly IServiceProvider _services;

    public MockServiceProvider()
    {
        ServiceCollection builder = new();

        builder.AddSingleton<IFeedSubscriptionContextProvider, MockFeedSubscriptionContextProvider>();
        builder.AddSingleton<IDialogService, MockDialogService>();
        builder.AddSingleton<ILoggerFactory, NullLoggerFactory>();

        _services = builder.BuildServiceProvider();
    }

    public object? GetService(Type serviceType)
    {
        return _services.GetService(serviceType);
    }
}
