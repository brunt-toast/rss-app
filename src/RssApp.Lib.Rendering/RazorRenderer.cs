using System.Reflection;
using CodeHollow.FeedReader;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using RssApp.Lib.Rendering.Templates;

namespace RssApp.Lib.Rendering;

public static class RazorRenderer
{
    public static async Task<string> RenderFeedItemAsync(FeedItem feedItem)
    {
        IServiceCollection services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        await using var htmlRenderer = new HtmlRenderer(serviceProvider, new NullLoggerFactory());
        return await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var dictionary = new Dictionary<string, object?>
            {
                { "Model", feedItem }
            };
            var parameters = ParameterView.FromDictionary(dictionary);
            var output = await htmlRenderer.RenderComponentAsync<FeedItemTemplate>(parameters);
            return output.ToHtmlString();
        });
    }
}
