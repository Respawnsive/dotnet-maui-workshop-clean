using System.Net.Http.Json;

namespace MonkeyFinder.Services;

public class MonkeyService : IMonkeyService
{
    private readonly HttpClient _httpClient;
    private readonly IConnectivity _connectivity;

    public MonkeyService(IConnectivity connectivity)
    {
        _httpClient = new HttpClient();
        _connectivity = connectivity;
    }

    /// <summary>
    /// Act as a "minimal cache" for the monkeys
    /// </summary>
    private List<Monkey> _monkeyList;

    /// <summary>
    /// Manage the business logic to get the monkeys datas (cache/Connectivity check/Api calls/Local file loading/Handling Errors and Alerts)
    /// </summary>
    /// <returns>Always return a (not null) Monkeys List</returns>
    public async Task<List<Monkey>> GetMonkeys()
    {
        if (_monkeyList?.Count > 0)
            return _monkeyList;

        try
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                //Offline
                await Shell.Current.DisplayAlert("No connectivity!", $"Local monkeys will be loaded", "OK");
                _monkeyList = await GetEmbeddedMonkeys();
                return _monkeyList is not null ? _monkeyList : new List<Monkey>();
            }

            // Online
            var response = await _httpClient.GetAsync("https://www.montemagno.com/monkeys.json");
            if (response.IsSuccessStatusCode)
            {
                _monkeyList = await response.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey);
                return _monkeyList is not null ? _monkeyList : new List<Monkey>();
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get monkeys : {ex}");
            await Shell.Current.DisplayAlert("Error!", ex.Message + " - Local monkeys will be loaded", "OK");
        }
        finally
        {
            _monkeyList = await GetEmbeddedMonkeys();
        }
        return _monkeyList is not null ? _monkeyList : new List<Monkey>();
    }

    /// <summary>
    /// Load the local file "monkeydata.json" and deserialize it to a List of Monkeys
    /// </summary>
    /// <returns></returns>
    private async Task<List<Monkey>> GetEmbeddedMonkeys()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
        using var reader = new StreamReader(stream);
        var contents = await reader.ReadToEndAsync();
        var result = JsonSerializer.Deserialize(contents, MonkeyContext.Default.ListMonkey);
        return result is not null ? result : new List<Monkey>();
    }
}
