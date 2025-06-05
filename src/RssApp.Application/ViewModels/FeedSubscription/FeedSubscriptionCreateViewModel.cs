using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RssApp.Application.Messages.FeedSubscription;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Services.Dialogs;
using RssApp.Application.Validations.FeedSubscription;
using RssApp.Application.ViewModels.FeedFilter;

namespace RssApp.Application.ViewModels.FeedSubscription;

public partial class FeedSubscriptionCreateViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private readonly IDialogService _dialogService;
    private readonly IFeedSubscriptionContextProvider _dbContextProvider;

    [ObservableProperty] public partial string FeedName { get; set; } = "";
    [ObservableProperty] public partial string FeedUri { get; set; } = "";

    public ICommand SaveCommand { get; }

    public FeedSubscriptionCreateViewModel(IServiceProvider services) : base(services)
    {
        SaveCommand = new RelayCommand(Save);
        _logger = services.GetRequiredService<ILoggerFactory>().CreateLogger<FeedSubscriptionCreateViewModel>();
        _dialogService = services.GetRequiredService<IDialogService>();
        _dbContextProvider = services.GetRequiredService<IFeedSubscriptionContextProvider>();
    }

    private async void Save()
    {
        var newPoco = new Core.FeedSubscription { FeedName = FeedName, FeedUri = FeedUri };

        var validation = await FeedSubscriptionModelValidator.ValidateAsync(Services, new FeedSubscriptionModel(newPoco));
        if (!validation.Success)
        {
            _logger.LogError("Declining to create a new feed subscription due to failed validation: {message}", validation.Message);
            await _dialogService.ShowErrorAsync("Validation failed", validation.Message);
            return;
        }

        await using var ctx = _dbContextProvider.New();
        ctx.FeedSubscriptions.Add(newPoco);
        await ctx.SaveChangesAsync();

        WeakReferenceMessenger.Default.Send(new FeedSubscriptionsChangedMessage());
    }
}
