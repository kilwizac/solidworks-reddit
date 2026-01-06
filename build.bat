@echo off
cd /d "%~dp0"
echo Building ChatterSolidworks...
dotnet build -c Debug
if %ERRORLEVEL% EQU 0 (
    echo.
    echo Build successful!
) else (
    echo.
    echo Build failed!
)

echo.
pause
