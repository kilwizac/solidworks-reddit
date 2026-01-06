# SolidWorks Reddit Sidebar

Browse Reddit directly inside SolidWorks! This add-in embeds a Reddit browser in the SolidWorks sidebar, letting you search r/SolidWorks for help or browse while your models rebuild.

![Screenshot](docs/screenshot.png)

## Quick Install

1. **Download** the [latest release](https://github.com/kilwizac/solidworks-reddit/releases/latest)
2. **Extract** the ZIP file
3. **Right-click** `install.bat` → **Run as administrator**
4. **Restart** SolidWorks

The Reddit panel will appear in your right sidebar!

## Requirements

- SolidWorks 2020 or later
- Windows 10 or 11

## Troubleshooting

| Problem | Solution |
|---------|----------|
| Add-in not showing | Go to **Tools → Add-Ins** and enable "Chatter Reddit" |
| Blank/white panel | Install [WebView2 Runtime](https://developer.microsoft.com/microsoft-edge/webview2/) |
| Installation failed | Make sure you right-clicked and selected "Run as administrator" |

## Uninstalling

1. Right-click `uninstall.bat` → **Run as administrator**
2. Restart SolidWorks

---

# Building from Source

The sections below are for developers who want to modify or build the add-in themselves.

## Prerequisites

- SolidWorks 2020 or later (for API DLLs)
- .NET SDK 8.0+
- .NET Framework 4.8

## Project Structure

```
solidworks-reddit/
├── ChatterSolidworks.csproj    # Project file
├── SwAddin.cs                  # Main add-in entry point
├── Controls/
│   └── RedditTaskPaneControl.cs # WebView2 browser control
├── Properties/
│   └── AssemblyInfo.cs
├── build.bat                   # Build script
├── register.bat                # Register (requires build first)
├── unregister.bat              # Unregister
├── reinstall.bat               # Build + register in one step
└── global.json                 # .NET SDK version pin
```

## Building

```cmd
# Build only
build.bat

# Full rebuild + register (run as admin)
reinstall.bat
```

Or via command line:
```cmd
dotnet build
dotnet build -c Release
```

## Custom SolidWorks Path

If SolidWorks is installed in a non-default location:

```cmd
dotnet build /p:SolidWorksPath="D:\Path\To\SOLIDWORKS"
```

Or set the `SOLIDWORKS_INSTALL_PATH` environment variable.

## Development Workflow

1. Make code changes
2. Run `reinstall.bat` as Administrator
3. Restart SolidWorks to load changes

## Debugging

1. Build in Debug mode
2. Register the add-in
3. Attach debugger to `SLDWORKS.exe`
4. Set breakpoints

## Resources

- [SolidWorks API Help](https://help.solidworks.com/2023/english/api/sldworksapiprogguide/welcome.htm)
- [WebView2 Documentation](https://learn.microsoft.com/en-us/microsoft-edge/webview2/)

## License

MIT License - see [LICENSE](LICENSE)
