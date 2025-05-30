using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Models.FeedFilter;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Tests.Mock;
using RssApp.Application.ViewModels.FeedFilter;
using RssApp.Core.Enums.Flags;

namespace RssApp.Application.Tests.ViewModels.FeedFilter;

[TestClass]
public class FeedFilterEditViewModelTests
{
    [TestMethod]
    public async Task Entity_ShouldBeChanged_AfterEdit()
    {
        IServiceProvider services = new MockServiceProvider();

        var feedFilter = new Core.FeedFilter
        {
            FeedFilterId = Guid.NewGuid(),
            FilterRegex = "^(AI)",
            IsWhitelist = false,
            AppliesTo = FeedFilterAppliesToFlags.Title
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

        var viewModel = new FeedFilterEditViewModel(services);
        await viewModel.InitAsync(new FeedFilterModel(feedFilter));
        const string newRegex = "^([Aa][Ii])";
        viewModel.Model.FilterRegex = newRegex;
        viewModel.SaveCommand.Execute(null);

        await using (var ctx = services.GetRequiredService<IFeedSubscriptionContextProvider>().New())
        {
            var record = await ctx.FeedFilters.SingleOrDefaultAsync(x => x.FeedFilterId == feedFilter.FeedFilterId);
            ArgumentNullException.ThrowIfNull(record);
            Assert.IsTrue(record.FilterRegex == newRegex);
        }
    }
}
