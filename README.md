# 🎮 GameHub Steam Scraper
https://gamehubsteamscraper-bbg0dpawcsc2hybh.eastus2-01.azurewebsites.net/ (free to use)
A Blazor Server application that scrapes Steam cover art, screenshots, and metadata for use with **ES-DE** (EmulationStation Desktop Edition).

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4?logo=blazor)
![License](https://img.shields.io/badge/License-MIT-green)

## ✨ Features

- **🔍 Game Search** - Search Steam's catalog with autocomplete suggestions
- **📦 Batch Export** - Export multiple games at once as a ZIP file
- **🖼️ Media Scraping** - Automatically fetches covers, headers, and screenshots
- **📁 ES-DE Compatible** - Exports in the exact folder structure ES-DE expects
- **💾 Local Storage** - Your game list persists in browser local storage
- **🎨 Modern UI** - Beautiful Steam-inspired dark theme with Tailwind CSS

## 📋 Requirements

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- A modern web browser

## 🚀 Getting Started

### Clone & Run

```bash
# Clone the repository
git clone https://github.com/yourusername/GamehubSteamScraper.git
cd GamehubSteamScraper

# Restore dependencies
dotnet restore

# Run the application
dotnet run
```

The application will start at `https://localhost:5001` or `http://localhost:5000`.

### Build for Production

```bash
dotnet publish -c Release -o ./publish
```

## 📖 Usage

### 1. Search for Games
Type a game name in the search bar. Autocomplete suggestions will appear as you type.

### 2. Add Games to Your List
Click on a search result to add the game to your collection. The app will automatically fetch:
- Cover art (600x900 library image)
- Header image (460x215 capsule)
- Screenshots (up to 5)

### 3. Manage Your Collection
- View all added games in the main panel
- Remove games by clicking the ✕ button
- Your collection is automatically saved to browser local storage

### 4. Export for ES-DE
Click the **Export** button to download a ZIP file containing all media and ROM files.

## 📁 Export Structure

The exported ZIP file follows ES-DE's expected folder structure:

```
📦 steam-games-export.zip
├── 📂 ES-DE/
│   └── 📂 downloaded_media/
│       └── 📂 steam/
│           ├── 📂 covers/
│           │   └── Game Name.jpg        # 600x900 cover art
│           ├── 📂 miximages/
│           │   └── Game Name.jpg        # 460x215 header/capsule
│           └── 📂 screenshots/
│               └── Game Name.jpg        # In-game screenshot
└── 📂 ROMS/
    └── 📂 Steam/
        └── Game Name.steam              # Empty ROM file for ES-DE
```

### Installing to ES-DE

1. Extract the ZIP file
2. Copy the `ES-DE` folder contents to your ES-DE installation directory
3. Copy the `ROMS` folder contents to your ROMs directory
4. Refresh your ES-DE game list

## 🏗️ Project Structure

```
GamehubSteamScraper/
├── Components/
│   ├── Layout/
│   │   └── MainLayout.razor      # Main app layout
│   └── Pages/
│       └── Home.razor            # Main page with search & game list
├── Models/
│   └── GameInfo.cs               # Game data models
├── Services/
│   ├── SteamApiService.cs        # Steam API integration
│   └── ExportService.cs          # ZIP export functionality
├── wwwroot/                      # Static assets
├── Program.cs                    # App configuration
└── appsettings.json             # App settings
```

## 🔧 Configuration

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## 🌐 Steam API

This application uses Steam's public APIs:

| Endpoint | Purpose |
|----------|---------|
| `/api/storesearch/` | Search for games by name |
| `/api/appdetails/` | Get game details and screenshots |
| Steam CDN | Download cover art and images |

> **Note**: No Steam API key is required. The app uses publicly accessible endpoints.

## 📸 Image Sources

| Image Type | URL Pattern | Resolution |
|------------|-------------|------------|
| Cover Art | `steamcdn-a.akamaihd.net/.../library_600x900_2x.jpg` | 600×900 |
| Header | From `header_image` field | 460×215 |
| Screenshots | From `screenshots` array | Full resolution |

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 💡 Inspiration

This project was inspired by [TechDweeb's YouTube video](https://www.youtube.com/watch?v=aYpSlKnb0XU&t=665s) on setting up Steam games in ES-DE.

## 🙏 Acknowledgments

- [TechDweeb](https://www.youtube.com/@TechDweeb) - For the inspiration and ES-DE setup guide
- [Steam](https://store.steampowered.com/) - For providing public APIs
- [ES-DE](https://es-de.org/) - EmulationStation Desktop Edition
- [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor) - For the amazing web framework
- [Tailwind CSS](https://tailwindcss.com/) - For the utility-first CSS framework

---

<p align="center">
  Made with ❤️ for the ES-DE community
</p>
