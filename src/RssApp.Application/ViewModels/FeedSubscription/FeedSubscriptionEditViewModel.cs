using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RssApp.Application.Messages.FeedSubscription;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Services.Dialogs;
using RssApp.Application.Validations.FeedSubscription;

namespace RssApp.Application.ViewModels.FeedSubscription;

public partial class FeedSubscriptionEditViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private readonly IDialogService _dialogService;
    private readonly IFeedSubscriptionContextProvider _dbContextProvider;

    private Guid _feedSubscriptionId;

    public ICommand SaveCommand { get; }

    [ObservableProperty] public partial string FeedName { get; set; } = "";
    [ObservableProperty] public partial string FeedUri { get; set; } = "";

    public FeedSubscriptionEditViewModel(IServiceProvider services) : base(services)
    {
        _dbContextProvider = services.GetRequiredService<IFeedSubscriptionContextProvider>();
        _dialogService = services.GetRequiredService<IDialogService>();
        _logger = services.GetRequiredService<ILoggerFactory>().CreateLogger<FeedSubscriptionEditViewModel>();
        SaveCommand = new RelayCommand(Save);
    }

    public async Task InitAsync(FeedSubscriptionModel feedSubscriptionModel)
    {
        _feedSubscriptionId = feedSubscriptionModel.ToPoco().FeedSubscriptionId;

        await using var ctx = _dbContextProvider.New();
        var record = await ctx.FeedSubscriptions.SingleOrDefaultAsync(x => x.FeedSubscriptionId == _feedSubscriptionId);

        if (record is null)
        {
            throw new Exception($"Could not find a {nameof(Core.FeedSubscription)} record " +
                                $"with {nameof(Core.FeedSubscription.FeedSubscriptionId)} {_feedSubscriptionId}.");
        }

        FeedName = record.FeedName;
        FeedUri = record.FeedUri;
    }

    private async void Save()
    {
        await using var ctx = _dbContextProvider.New();

        var record = await ctx.FeedSubscriptions.SingleOrDefaultAsync(x => x.FeedSubscriptionId == _feedSubscriptionId);

        if (record is null)
        {
            _logger.LogError("Failed to update feed subscription: Could not find a feed subscription with ID {id} to update", _feedSubscriptionId);
            throw new Exception($"Could not find a {nameof(Core.FeedSubscription)} record " +
                                $"with {nameof(Core.FeedSubscription.FeedSubscriptionId)} {_feedSubscriptionId}.");
        }

        record.FeedName = FeedName;
        record.FeedUri = FeedUri;

        var validation = await FeedSubscriptionModelValidator.ValidateAsync(Services, new FeedSubscriptionModel(record));
        if (!validation.Success)
        {
            _logger.LogError("Declining to create a new feed subscription due to failed validation: {message}", validation.Message);
            await _dialogService.ShowErrorAsync("Validation failed", validation.Message);
            return;
        }

        await ctx.SaveChangesAsync();
        WeakReferenceMessenger.Default.Send(new FeedSubscriptionsChangedMessage());
    }
}
