namespace Application.Interfaces.Services;

public interface IAppConfiguration
{
    string GetValue(string key);
}
