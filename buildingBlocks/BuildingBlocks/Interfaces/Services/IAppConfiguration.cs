namespace BuildingBlocks.Interfaces.Services;

public interface IAppConfiguration
{
    string GetValue(string key);
}
