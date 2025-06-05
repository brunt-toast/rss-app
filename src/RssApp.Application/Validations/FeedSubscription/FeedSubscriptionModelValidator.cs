using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RssApp.Application.Models.FeedSubscription;

namespace RssApp.Application.Validations.FeedSubscription;

internal class FeedSubscriptionModelValidator : IValidator<FeedSubscriptionModel>
{
    public static Task<(bool Success, string Message)> ValidateAsync(IServiceProvider services, FeedSubscriptionModel item)
    {
        if (!Uri.TryCreate(item.FeedUri, UriKind.Absolute, out _))
        {
            return Task.FromResult((false, "Feed URI is invalid"));
        }

        return Task.FromResult((true, ""));
    }
}
