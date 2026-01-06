@echo off
echo Registering ChatterSolidworks add-in...
echo.
echo NOTE: This script must be run as Administrator!
echo.

set DLL_PATH=%~dp0bin\Debug\net48\ChatterSolidworks.dll

if not exist "%DLL_PATH%" (
    echo ERROR: DLL not found at %DLL_PATH%
    echo Please build the project first using build.bat
    exit /b 1
)

%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe /codebase "%DLL_PATH%"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Registration successful!
    echo Restart SolidWorks to load the add-in.
) else (
    echo.
    echo Registration failed! Make sure you're running as Administrator.
)

echo.
pause
