# Chatter SolidWorks Add-in

A SolidWorks add-in that embeds Reddit in a sidebar TaskPane, allowing users to browse r/SolidWorks and other subreddits directly within SolidWorks.

## Prerequisites

- SolidWorks 2020 or later
- .NET SDK 8.0+ (for building)
- .NET Framework 4.8 (runtime target, usually pre-installed)
- WebView2 Runtime (usually pre-installed on Windows 10/11)

## Project Structure

```
chatter-solidworks/
├── ChatterSolidworks.csproj    # Project file with SolidWorks API references
├── SwAddin.cs                  # Main add-in class implementing ISwAddin
├── Controls/
│   └── RedditTaskPaneControl.cs # TaskPane UserControl with WebView2
├── Properties/
│   └── AssemblyInfo.cs         # Assembly metadata and COM visibility
├── .vscode/
│   ├── tasks.json              # Cursor/VS Code build tasks
│   └── settings.json           # Editor settings
├── build.bat                   # Build script
├── register.bat                # Register add-in (requires Admin)
├── unregister.bat              # Unregister add-in (requires Admin)
├── reinstall.bat               # Unregister + Build + Register (requires Admin)
├── global.json                 # Pins .NET SDK version
└── README.md
```

## Quick Start

### Option 1: Batch Scripts (Recommended)

**Build only:**
```cmd
build.bat
```

**Full reinstall (unregister → build → register):**
```cmd
# Run as Administrator
reinstall.bat
```

**Register/Unregister separately:**
```cmd
# Run as Administrator
register.bat
unregister.bat
```

### Option 2: Cursor Tasks

Press `Ctrl+Shift+P` → "Tasks: Run Task" → Select:
- **Build** - Compile the project (Ctrl+Shift+B)
- **Build Release** - Compile in release mode
- **Clean** - Clean build artifacts
- **Register (Run as Admin)** - Register the DLL
- **Unregister (Run as Admin)** - Unregister the DLL
- **Reinstall (Run as Admin)** - Full rebuild and re-register

> **Note:** Register/Unregister tasks require Cursor to be running as Administrator.

### Option 3: Command Line

```cmd
# Build
dotnet build

# Build release
dotnet build -c Release

# Clean
dotnet clean
```

## Workflow

1. Make code changes in Cursor
2. Run `reinstall.bat` as Administrator (or use Ctrl+Shift+B to just build)
3. Restart SolidWorks to load the updated add-in

## Building on Different Machines

The project references SolidWorks API DLLs from a default installation path. If SolidWorks is installed elsewhere, override the path:

```cmd
dotnet build /p:SolidWorksPath="D:\Path\To\SOLIDWORKS"
```

Or set it as an environment variable before building.

## Development

### TaskPane Implementation

The add-in creates a TaskPane (sidebar) with an embedded WebView2 browser:

```csharp
// In SwAddin.cs - CreateTaskPane method:
_taskPaneView = _swApp.CreateTaskpaneView2("", "Reddit");
_taskPaneControl = _taskPaneView.AddControl(
    "ChatterSolidworks.RedditTaskPaneControl",
    "") as RedditTaskPaneControl;
```

### Adding New Commands

1. Add a new command ID constant in `SwAddin.cs`
2. Add the command in `AddCommandManager()` using `cmdGroup.AddCommandItem2()`
3. Create callback methods for the command

### Handling Document Events

```csharp
// Subscribe to document events
_swApp.DocumentLoadNotify2 += OnDocumentLoad;
_swApp.FileNewNotify2 += OnFileNew;
```

## Debugging

1. Build in Debug mode
2. Register the add-in
3. Attach Cursor/VS Code debugger to `SLDWORKS.exe` process
4. Set breakpoints in your code

## Key Classes

- `SwAddin` - Main entry point implementing `ISwAddin` interface
- `RedditTaskPaneControl` - WinForms UserControl hosting WebView2
- `ISldWorks` - SolidWorks application interface
- `ITaskpaneView` - TaskPane sidebar container

## Resources

- [SolidWorks API Help](https://help.solidworks.com/2023/english/api/sldworksapiprogguide/welcome.htm)
- [WebView2 Documentation](https://learn.microsoft.com/en-us/microsoft-edge/webview2/)
