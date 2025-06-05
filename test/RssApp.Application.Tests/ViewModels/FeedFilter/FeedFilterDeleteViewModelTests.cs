using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Models.FeedFilter;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Services.Dialogs;
using RssApp.Application.Tests.Mock;
using RssApp.Application.ViewModels.FeedFilter;
using RssApp.Core.Enums.Flags;

namespace RssApp.Application.Tests.ViewModels.FeedFilter;

[TestClass]
public class FeedFilterDeleteViewModelTests
{
    [TestMethod]
    public async Task Entity_ShouldNotExist_AfterDelete()
    {
        IServiceProvider services = new MockServiceProvider();
        services.GetRequiredService<IDialogService>().ConfirmationRequested += (_, e) =>
        {
            e.SetConfirmed(true);
        };

        var feedFilter = new Core.FeedFilter
        {
            FeedFilterId = Guid.NewGuid(),
            AppliesTo = FeedFilterAppliesToFlags.Title,
            FilterRegex = "^(AI)",
            IsWhitelist = false
        };

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            await ctx.FeedFilters.AddAsync(feedFilter);
            await ctx.SaveChangesAsync();
        }

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            bool exists = await ctx.FeedFilters.AnyAsync(x => x.FeedFilterId == feedFilter.FeedFilterId);
            Assert.IsTrue(exists);
        }

        var deleteViewModel = new FeedFilterDeleteViewModel(services);
        deleteViewModel.Init(new FeedFilterModel(feedFilter));

        var deleteAsyncCommand = deleteViewModel.DeleteCommand as IAsyncRelayCommand;
        ArgumentNullException.ThrowIfNull(deleteAsyncCommand, $"{nameof(deleteViewModel.DeleteCommand)} was not of type {nameof(IAsyncRelayCommand)}");
        await deleteAsyncCommand.ExecuteAsync(null);

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            bool exists = await ctx.FeedFilters.AnyAsync(x => x.FeedFilterId == feedFilter.FeedFilterId);
            Assert.IsFalse(exists);
        }
    }
}
