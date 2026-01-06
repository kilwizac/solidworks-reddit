@echo off
echo Unregistering ChatterSolidworks add-in...
echo.
echo NOTE: This script must be run as Administrator!
echo.

set DLL_PATH=%~dp0bin\Debug\net48\ChatterSolidworks.dll

if not exist "%DLL_PATH%" (
    echo ERROR: DLL not found at %DLL_PATH%
    echo Nothing to unregister.
    exit /b 1
)

%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe /unregister "%DLL_PATH%"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Unregistration successful!
) else (
    echo.
    echo Unregistration failed! Make sure you're running as Administrator.
)

echo.
pause
