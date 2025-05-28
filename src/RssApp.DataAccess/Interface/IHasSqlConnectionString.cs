namespace RssApp.DataAccess.Interface;

public interface IHasSqlConnectionString
{
    public string? SqlConnectionString { get; }
}
