@echo off
cls

set "GREEN=[32m"
set "CYAN=[36m"
set "RESET=[0m"

echo %CYAN%--------------------------------------------------%RESET%
echo   Lancement de l'API TODO...
echo %CYAN%--------------------------------------------------%RESET%

dotnet run --project src/TODO.Api/TODO.Api.csproj

if %errorlevel% neq 0 (
    echo.
    echo %GREEN%[INFO]%RESET% L'application s'est arretee.
    pause
)