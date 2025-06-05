namespace RssApp.Application.Validations;

internal interface IValidator<in T>
{
    public static abstract Task<(bool Success, string Message)> ValidateAsync(IServiceProvider services, T item);
}
