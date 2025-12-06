namespace GamehubSteamScraper.Models;

public class GameInfo
{
    public int AppId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CoverUrl { get; set; } = string.Empty;
    public string HeaderUrl { get; set; } = string.Empty;
    public List<string> ScreenshotUrls { get; set; } = new();
    public string? ShortDescription { get; set; }
    public DateTime AddedDate { get; set; } = DateTime.Now;
    
    // Sanitized name for file export (ES-DE requires exact match)
    public string SafeFileName => SanitizeFileName(Name);
    
    private static string SanitizeFileName(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        return string.Join("", name.Select(c => invalid.Contains(c) ? '_' : c));
    }
}

public class SteamAppListResponse
{
    public AppListWrapper? Applist { get; set; }
}

public class AppListWrapper
{
    public List<SteamApp>? Apps { get; set; }
}

public class SteamApp
{
    public int Appid { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SteamAppDetailsResponse
{
    public bool Success { get; set; }
    public SteamAppData? Data { get; set; }
}

public class SteamAppData
{
    public string Name { get; set; } = string.Empty;
    public int Steam_appid { get; set; }
    public string Short_description { get; set; } = string.Empty;
    public string Header_image { get; set; } = string.Empty;
    public List<SteamScreenshot>? Screenshots { get; set; }
}

public class SteamScreenshot
{
    public int Id { get; set; }
    public string Path_thumbnail { get; set; } = string.Empty;
    public string Path_full { get; set; } = string.Empty;
}
