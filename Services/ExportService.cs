using System.IO.Compression;
using GamehubSteamScraper.Models;

namespace GamehubSteamScraper.Services;

public class ExportService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ExportService> _logger;

    public ExportService(HttpClient httpClient, ILogger<ExportService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Export games to a ZIP file with ES-DE folder structure
    /// </summary>
    public async Task<byte[]> ExportToZipAsync(List<GameInfo> games, IProgress<(int current, int total, string message)>? progress = null)
    {
        using var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            var total = games.Count;
            var current = 0;

            foreach (var game in games)
            {
                current++;
                var safeFileName = game.SafeFileName;
                
                progress?.Report((current, total, $"Processing {game.Name}..."));

                // Download and add cover art
                await TryAddImageToArchive(archive, game.CoverUrl, $"covers/{safeFileName}.jpg", game.Name, "cover");

                // Download and add Steam header/capsule image
                await TryAddImageToArchive(archive, game.HeaderUrl, $"steam/{safeFileName}.jpg", game.Name, "header");

                // Download and add first screenshot
                if (game.ScreenshotUrls.Any())
                {
                    await TryAddImageToArchive(archive, game.ScreenshotUrls.First(), 
                        $"screenshots/{safeFileName}.jpg", game.Name, "screenshot");
                }
            }
        }

        memoryStream.Position = 0;
        return memoryStream.ToArray();
    }

    private async Task TryAddImageToArchive(ZipArchive archive, string imageUrl, string entryPath, string gameName, string imageType)
    {
        if (string.IsNullOrEmpty(imageUrl)) return;

        try
        {
            var imageData = await _httpClient.GetByteArrayAsync(imageUrl);
            var entry = archive.CreateEntry(entryPath, CompressionLevel.Optimal);
            
            await using var entryStream = entry.Open();
            await entryStream.WriteAsync(imageData);
            
            _logger.LogDebug("Added {ImageType} for {GameName}", imageType, gameName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to download {ImageType} for {GameName} from {Url}", 
                imageType, gameName, imageUrl);
        }
    }
}
