using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Extensions.System.Windows.Input;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Services.Dialogs;
using RssApp.Application.Tests.Mock;
using RssApp.Application.ViewModels.FeedSubscription;

namespace RssApp.Application.Tests.ViewModels.FeedSubscription;

[TestClass]
public class FeedSubscriptionDeleteViewModelTests
{
    [TestMethod]
    public async Task Entity_ShouldNotExist_AfterDelete()
    {
        IServiceProvider services = new MockServiceProvider();
        services.GetRequiredService<IDialogService>().ConfirmationRequested += (_, e) =>
        {
            e.SetConfirmed(true);
        };

        const string testFeedName = "Test feed";
        const string testFeedUri = "https://example.com/feed.xml";
        var feedSubscription = new Core.FeedSubscription { FeedName = testFeedName, FeedUri = testFeedUri };
        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            await ctx.FeedSubscriptions.AddAsync(feedSubscription);
            await ctx.SaveChangesAsync();
        }

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            bool exists = await ctx.FeedSubscriptions.AnyAsync(x => x.FeedSubscriptionId == feedSubscription.FeedSubscriptionId);
            Assert.IsTrue(exists);
        }

        var deleteViewModel = new FeedSubscriptionDeleteViewModel(services);
        deleteViewModel.Init(new FeedSubscriptionModel(feedSubscription));

        var deleteAsyncCommand = deleteViewModel.DeleteCommand as IAsyncRelayCommand;
        ArgumentNullException.ThrowIfNull(deleteAsyncCommand, $"{nameof(deleteViewModel.DeleteCommand)} was not of type {nameof(IAsyncRelayCommand)}");
        await deleteAsyncCommand.ExecuteAsync(null);

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            bool exists = await ctx.FeedSubscriptions.AnyAsync(x => x.FeedSubscriptionId == feedSubscription.FeedSubscriptionId);
            Assert.IsFalse(exists);
        }
    }

    [TestMethod]
    public async Task Entity_ShouldNotDelete_WhenConfirmationCancelled()
    {
        IServiceProvider services = new MockServiceProvider();
        services.GetRequiredService<IDialogService>().ConfirmationRequested += (_, e) =>
        {
            e.SetConfirmed(false);
        };

        const string testFeedName = "Test feed";
        const string testFeedUri = "https://example.com/feed.xml";
        var feedSubscription = new Core.FeedSubscription { FeedName = testFeedName, FeedUri = testFeedUri };
        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            await ctx.FeedSubscriptions.AddAsync(feedSubscription);
            await ctx.SaveChangesAsync();
        }

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            bool exists = await ctx.FeedSubscriptions.AnyAsync(x => x.FeedSubscriptionId == feedSubscription.FeedSubscriptionId);
            Assert.IsTrue(exists);
        }

        var deleteViewModel = new FeedSubscriptionDeleteViewModel(services);
        deleteViewModel.Init(new FeedSubscriptionModel(feedSubscription));

        var deleteAsyncCommand = deleteViewModel.DeleteCommand as IAsyncRelayCommand;
        ArgumentNullException.ThrowIfNull(deleteAsyncCommand, $"{nameof(deleteViewModel.DeleteCommand)} was not of type {nameof(IAsyncRelayCommand)}");
        await deleteAsyncCommand.ExecuteAsync(null);

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            bool exists = await ctx.FeedSubscriptions.AnyAsync(x => x.FeedSubscriptionId == feedSubscription.FeedSubscriptionId);
            Assert.IsTrue(exists);
        }
    }
}
