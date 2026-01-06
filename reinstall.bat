@echo off
cd /d "%~dp0"
echo ============================================
echo  ChatterSolidworks Reinstall
echo ============================================
echo.
echo NOTE: This script must be run as Administrator!
echo.

set DLL_PATH=%~dp0bin\Debug\net48\ChatterSolidworks.dll

:: Step 1: Unregister (if exists)
if exist "%DLL_PATH%" (
    echo [1/3] Unregistering existing add-in...
    %SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe /unregister "%DLL_PATH%" >nul 2>&1
    echo       Done.
) else (
    echo [1/3] No existing DLL to unregister, skipping...
)
echo.

:: Step 2: Build
echo [2/3] Building project...
dotnet build -c Debug
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Build failed! Aborting reinstall.
    exit /b 1
)
echo.

:: Step 3: Register
echo [3/3] Registering add-in...
%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe /codebase "%DLL_PATH%"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ============================================
    echo  Reinstall complete!
    echo  Restart SolidWorks to load the add-in.
    echo ============================================
) else (
    echo.
    echo Registration failed! Make sure you're running as Administrator.
)

echo.
pause
