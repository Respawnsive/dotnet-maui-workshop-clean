using System.Collections.Generic;
using System.Net.Http.Json;

namespace MonkeyFinder.Services;

public class MonkeyService
{
    readonly HttpClient _httpClient;
    public MonkeyService()
    {
        _httpClient = new HttpClient();
    }

    private List<Monkey> _monkeyList;
    public async Task<List<Monkey>> GetMonkeys()
    {
        if (_monkeyList?.Count > 0)
            return _monkeyList;

        // Online
        var response = await _httpClient.GetAsync("https://www.montemagno.com/monkeys.json");
        if (response.IsSuccessStatusCode)
        {
            _monkeyList = await response.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey);
        }

        // Offline
        /*using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
        using var reader = new StreamReader(stream);
        var contents = await reader.ReadToEndAsync();
        _monkeyList = JsonSerializer.Deserialize(contents, MonkeyContext.Default.ListMonkey);*/

        return _monkeyList;
    }
}
