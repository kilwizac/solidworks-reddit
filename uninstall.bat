@echo off
cd /d "%~dp0"

echo ============================================
echo   SolidWorks Reddit Sidebar - Uninstaller
echo ============================================
echo.

:: Check for admin privileges
net session >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: This uninstaller must be run as Administrator.
    echo.
    echo Right-click uninstall.bat and select "Run as administrator"
    echo.
    pause
    exit /b 1
)

:: Find the DLL
set DLL_PATH=%~dp0ChatterSolidworks.dll

if not exist "%DLL_PATH%" (
    echo ERROR: ChatterSolidworks.dll not found.
    echo Make sure uninstall.bat is in the same folder as the DLL.
    echo.
    pause
    exit /b 1
)

echo Uninstalling SolidWorks Reddit Sidebar...
echo.

:: Unregister the DLL
%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe /unregister "%DLL_PATH%"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ============================================
    echo   Uninstallation successful!
    echo ============================================
    echo.
    echo Restart SolidWorks to complete removal.
    echo.
) else (
    echo.
    echo ============================================
    echo   Uninstallation failed!
    echo ============================================
    echo.
    echo Make sure you're running as Administrator.
    echo.
)

pause
