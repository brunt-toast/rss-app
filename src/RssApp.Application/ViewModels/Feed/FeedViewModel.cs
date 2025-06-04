using System.Collections.ObjectModel;
using CodeHollow.FeedReader;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using RssApp.Application.Extensions.System.Collections.ObjectModel;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.Services.Dialogs;

namespace RssApp.Application.ViewModels.Feed;

public partial class FeedViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;

    public FeedViewModel(IServiceProvider services)
    {
        _dialogService = services.GetRequiredService<IDialogService>();
    }

    public ObservableCollection<FeedItem> Items { get; } = [];

    public async Task InitAsync(FeedSubscriptionModel model)
    {
        CodeHollow.FeedReader.Feed feed;

        int retries = 0;
        const int maxRetries = 10;
        while (true)
        {
            if (retries >= maxRetries)
            {
                await _dialogService.ShowErrorAsync("Could not get feed", $"Failed to get the feed after {maxRetries} retries.");
                return;
            }

            try
            {
                feed = await FeedReader.ReadAsync(model.FeedUri);
                break;
            }
            catch (Exception)
            {
                retries++;
            }
        }

        await Items.ReplaceRangeAsync(feed.Items);
    }
}
