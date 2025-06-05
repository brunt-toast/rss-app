using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RssApp.Application.Extensions.System.Collections.ObjectModel;
using RssApp.Application.Messages.FeedFilter;
using RssApp.Application.Messages.FeedSubscription;
using RssApp.Application.Models.FeedFilter;
using RssApp.Application.Models.FeedSubscription;
using RssApp.Application.Services.ContextProviders.FeedSubscriptions;
using RssApp.Application.Services.Dialogs;
using RssApp.Application.Validations.FeedFilter;
using RssApp.Core.Enums.Flags;
using RssApp.Core.Junctions;

namespace RssApp.Application.ViewModels.FeedFilter;

public partial class FeedFilterCreateViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private readonly IDialogService _dialogService;
    private readonly IFeedSubscriptionContextProvider _dbContextProvider;

    public ICommand SaveCommand { get; }
    public ObservableCollection<FeedSubscriptionModel> AvailableFeedSubscriptions { get; } = [];
    public ObservableCollection<FeedSubscriptionModel> SelectedFeedSubscriptions { get; } = [];

    [ObservableProperty] public partial string FilterRegex { get; set; } = "";
    [ObservableProperty] public partial bool IsWhitelist { get; set; }
    [ObservableProperty] public partial bool AppliesToUri { get; set; }
    [ObservableProperty] public partial bool AppliesToTitle { get; set; }
    [ObservableProperty] public partial bool AppliesToBody { get; set; }

    public FeedFilterCreateViewModel(IServiceProvider services) : base(services)
    {
        _logger = services.GetRequiredService<ILoggerFactory>().CreateLogger<FeedFilterCreateViewModel>();
        _dialogService = services.GetRequiredService<IDialogService>();
        _dbContextProvider = services.GetRequiredService<IFeedSubscriptionContextProvider>();
        SaveCommand = new RelayCommand(Save);
    }

    public async Task InitAsync()
    {
        await using var ctx = _dbContextProvider.New();

        var availableFeedSubscriptions = ctx.FeedSubscriptions.Select(x => new FeedSubscriptionModel(x));
        await AvailableFeedSubscriptions.ReplaceRangeAsync(availableFeedSubscriptions);
    }

    private async void Save()
    {
        var appliesTo = (FeedFilterAppliesToFlags)0;
        if (AppliesToUri) appliesTo ^= FeedFilterAppliesToFlags.Uri;
        if (AppliesToTitle) appliesTo ^= FeedFilterAppliesToFlags.Title;
        if (AppliesToBody) appliesTo ^= FeedFilterAppliesToFlags.Body;

        var feedFilter = new Core.FeedFilter
        {
            FeedFilterId = Guid.NewGuid(),
            FilterRegex = FilterRegex,
            IsWhitelist = IsWhitelist,
            AppliesTo = appliesTo
        };

        var junctions = SelectedFeedSubscriptions.Select(x => new FeedSubscriptionFeedFilter
        {
            FeedSubscriptionId = x.ToPoco().FeedSubscriptionId,
            FeedFilterId = feedFilter.FeedFilterId
        });

        var validation = await FeedFilterModelValidator.ValidateAsync(Services, new FeedFilterModel(feedFilter));
        if (!validation.Success)
        {
            _logger.LogError("Declining to create a new feed filter due to failed validation: {message}", validation.Message);
            await _dialogService.ShowErrorAsync("Validation failed", validation.Message);
            return;
        }

        await using var ctx = _dbContextProvider.New();
        await ctx.FeedFilters.AddAsync(feedFilter);
        await ctx.FeedSubscriptionFeedFilters.AddRangeAsync(junctions);
        await ctx.SaveChangesAsync();

        WeakReferenceMessenger.Default.Send(new FeedFiltersChangedMessage());
    }
}
