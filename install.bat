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

:: Unblock all files (removes "downloaded from internet" flag)
echo Unblocking files...
powershell -Command "Get-ChildItem -Path '%~dp0' -Recurse | Unblock-File -ErrorAction SilentlyContinue" 2>nul

:: Register the DLL
echo Registering add-in...
echo.
%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe /codebase "%DLL_PATH%"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ============================================
    echo   Installation failed!
    echo ============================================
    echo.
    echo The error message above should explain what went wrong.
    echo.
    pause
    exit /b 1
)

:: Verify the registry key was created
reg query "HKLM\SOFTWARE\SolidWorks\AddIns\{A8E7D4F2-3B1C-4E5A-9F8D-6C2B1A0E9D3F}" >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ============================================
    echo   Installation incomplete!
    echo ============================================
    echo.
    echo The DLL registered but SolidWorks integration failed.
    echo This can happen if the files are blocked by Windows.
    echo.
    echo Try moving this folder to C:\SolidWorksReddit\ and run again.
    echo.
    pause
    exit /b 1
)

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
pause
