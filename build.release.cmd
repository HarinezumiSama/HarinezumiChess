@echo off

setlocal

set TAG=%~n0
set AppDir=%ProgramFiles%
if /i "%PROCESSOR_ARCHITECTURE%" equ "AMD64" set AppDir=%ProgramFiles(x86)%

set MSBUILD=%AppDir%\MSBuild\14.0\Bin\MSBuild.exe
set PROJECT=%~dp0\src\HarinezumiChess.sln

echo.
echo [%TAG%] NuGet: Clearing caches...
nuget locals all -clear || goto ERROR
echo [%TAG%] NuGet: Clearing caches - DONE.

echo.
echo [%TAG%] NuGet: Restoring packages for "%PROJECT%"...
nuget.exe restore "%PROJECT%" -MSBuildVersion 14 -Verbosity detailed -NonInteractive -NoCache || goto ERROR
echo [%TAG%] NuGet: Restoring packages for "%PROJECT%" - DONE.

echo.
echo [%TAG%] MSBuild: Rebuilding "%PROJECT%"...
"%MSBUILD%" "%PROJECT%" /t:Rebuild /p:Configuration="Release" /p:Platform="Any CPU" || goto ERROR
echo [%TAG%] MSBuild: Rebuilding "%PROJECT%" - DONE.

goto :EOF

:: ------------------------------------------------------------------------------------------------------------------------

:ERROR
echo.
echo *** ERROR has occurred ***
pause
exit /b 1
