using CommunityToolkit.Mvvm.ComponentModel;
using RssApp.Core.Enums.Flags;

namespace RssApp.Application.Models.FeedFilter;

public partial class FeedFilterModel : ObservableObject
{
    private Core.FeedFilter _poco;

    public string FilterRegex
    {
        get => _poco.FilterRegex;
        set
        {
            if (EqualityComparer<string>.Default.Equals(_poco.FilterRegex, value))
            {
                return;
            }

            OnPropertyChanging();
            _poco.FilterRegex = value;
            OnPropertyChanged();
        }
    }

    public bool IsWhitelist
    {
        get => _poco.IsWhitelist;
        set
        {
            if (EqualityComparer<bool>.Default.Equals(_poco.IsWhitelist, value))
            {
                return;
            }

            OnPropertyChanging();
            _poco.IsWhitelist = value;
            OnPropertyChanged();
        }
    }

    public FeedFilterAppliesToFlags AppliesTo
    {
        get => _poco.AppliesTo;
        set
        {
            if (EqualityComparer<FeedFilterAppliesToFlags>.Default.Equals(_poco.AppliesTo, value))
            {
                return;
            }

            OnPropertyChanging();
            _poco.AppliesTo = value;
            OnPropertyChanged();
        }
    }

    public FeedFilterModel(Core.FeedFilter poco)
    {
        _poco = poco;
    }

    public Core.FeedFilter ToPoco() => _poco;
}
