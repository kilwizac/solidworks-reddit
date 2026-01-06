@echo off
cd /d "%~dp0"

echo ============================================
echo   SolidWorks Reddit Sidebar - Installer
echo ============================================
echo.

:: Check for admin privileges
net session >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: This installer must be run as Administrator.
    echo.
    echo Right-click install.bat and select "Run as administrator"
    echo.
    pause
    exit /b 1
)

:: Find the DLL
set DLL_PATH=%~dp0ChatterSolidworks.dll

if not exist "%DLL_PATH%" (
    echo ERROR: ChatterSolidworks.dll not found.
    echo Make sure install.bat is in the same folder as the DLL.
    echo.
    pause
    exit /b 1
)

echo Installing SolidWorks Reddit Sidebar...
echo.

:: Register the DLL
%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe /codebase "%DLL_PATH%"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ============================================
    echo   Installation successful!
    echo ============================================
    echo.
    echo Next steps:
    echo   1. Open SolidWorks
    echo   2. Look for "Reddit" in the right sidebar
    echo.
    echo If you don't see it, go to:
    echo   Tools ^> Add-Ins ^> Enable "Chatter Reddit"
    echo.
) else (
    echo.
    echo ============================================
    echo   Installation failed!
    echo ============================================
    echo.
    echo Make sure you're running as Administrator.
    echo.
)

pause
