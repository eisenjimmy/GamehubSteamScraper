using System.Text.Json;
using System.Text.Json.Serialization;
using GamehubSteamScraper.Models;

namespace GamehubSteamScraper.Services;

public class SteamApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SteamApiService> _logger;

    public SteamApiService(HttpClient httpClient, ILogger<SteamApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
    }

    /// <summary>
    /// Search for games by name using Steam Store search
    /// </summary>
    public async Task<List<SteamApp>> SearchGamesAsync(string query, int maxResults = 10)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            return new List<SteamApp>();

        try
        {
            // Use Steam store search API
            var encodedQuery = Uri.EscapeDataString(query);
            var url = $"https://store.steampowered.com/api/storesearch/?term={encodedQuery}&l=english&cc=US";
            
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Steam search returned {StatusCode}", response.StatusCode);
                return new List<SteamApp>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var searchResult = JsonSerializer.Deserialize<StoreSearchResponse>(json, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (searchResult?.Items == null)
                return new List<SteamApp>();

            return searchResult.Items
                .Take(maxResults)
                .Select(item => new SteamApp 
                { 
                    Appid = item.Id, 
                    Name = item.Name 
                })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching games for query: {Query}", query);
            return new List<SteamApp>();
        }
    }

    /// <summary>
    /// Get detailed game information including screenshots
    /// </summary>
    public async Task<GameInfo?> GetGameDetailsAsync(int appId)
    {
        try
        {
            var url = $"https://store.steampowered.com/api/appdetails?appids={appId}";
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<Dictionary<string, SteamAppDetailsResponse>>(json, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (data == null || !data.TryGetValue(appId.ToString(), out var appDetails) || 
                !appDetails.Success || appDetails.Data == null)
                return null;

            var gameData = appDetails.Data;

            return new GameInfo
            {
                AppId = appId,
                Name = gameData.Name,
                ShortDescription = gameData.Short_description,
                HeaderUrl = gameData.Header_image,
                CoverUrl = $"https://steamcdn-a.akamaihd.net/steam/apps/{appId}/library_600x900_2x.jpg",
                ScreenshotUrls = gameData.Screenshots?
                    .Take(5)
                    .Select(s => s.Path_full)
                    .ToList() ?? new List<string>()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting game details for appId: {AppId}", appId);
            return null;
        }
    }
}

// Store search response models
public class StoreSearchResponse
{
    public int Total { get; set; }
    public List<StoreSearchItem>? Items { get; set; }
}

public class StoreSearchItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Tiny_image { get; set; }
}
