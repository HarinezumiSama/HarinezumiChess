@echo off

setlocal
set TAG=%~n0
set DEFAULT_CFG=Debug
set PROJECT=%~dp0\src\HarinezumiChess.sln
set MSBUILD_BASE_PARAMS=/noautoresponse /m /nodeReuse:false /DetailedSummary /clp:Verbosity=detailed;ShowTimestamp;EnableMPLogging

verify invalid params >nul
setlocal enableextensions enabledelayedexpansion
if errorlevel 1 (
    @echo.
    @echo * ERROR: Unable to turn on Command Shell extensions.
    exit /b 127
)

if "%~1" equ "/?" goto HELP
if "%~1" equ "-?" goto HELP
if "%~2" neq "" goto HELP

set CFG=%~1
if "%CFG%" equ "" set CFG=%DEFAULT_CFG%

set AppDir=%ProgramFiles%
if /i "%PROCESSOR_ARCHITECTURE%" equ "AMD64" set AppDir=%ProgramFiles(x86)%

set MSBuildPath=
for %%A in ("Community", "Professional", "Enterprise", "BuildTools") do (
    set TEMP_LOCAL=!AppDir!\Microsoft Visual Studio\2017\%%~A\MSBuild\15.0\Bin\MSBuild.exe
    if exist "!TEMP_LOCAL!" set MSBuildPath=!TEMP_LOCAL!
)

if "%MSBuildPath%" equ "" (
    @echo.
    @echo * ERROR: MSBuild 15.0 is not found.
    exit /b 127
)

call :CLEAN_DIR "%~dp0\bin" || goto ERROR
call :CLEAN_DIR "%~dp0\bin.tests" || goto ERROR

echo.
echo [%TAG%] NuGet: Clearing local caches...
nuget locals all -clear || goto ERROR
echo [%TAG%] NuGet: Clearing caches - DONE.

echo.
echo [%TAG%] NuGet: Restoring packages for "%PROJECT%"...
nuget.exe restore "%PROJECT%" -MSBuildVersion 14 -Verbosity detailed -NonInteractive -NoCache || goto ERROR
echo [%TAG%] NuGet: Restoring packages for "%PROJECT%" - DONE.

echo.
echo [%TAG%] MSBuild: Rebuilding "%PROJECT%"...
"%MSBuildPath%" "%PROJECT%" /t:Rebuild /p:Configuration="%CFG%" /p:Platform="Any CPU" /p:BuildProjectReferences=true %MSBUILD_BASE_PARAMS% || goto ERROR
echo [%TAG%] MSBuild: Rebuilding "%PROJECT%" - DONE.

goto :EOF

:: ------------------------------------------------------------------------------------------------------------------------

:ERROR
echo.
echo *** ERROR has occurred ***
exit /b 1

:: ------------------------------------------------------------------------------------------------------------------------

:CLEAN_DIR
echo.
echo [%TAG%] Cleaning directory "%~1"...
if exist "%~1" (
    rd /s /q "%~1" || exit /b 1
)
echo [%TAG%] Cleaning directory "%~1" - DONE.
goto :EOF

:: ------------------------------------------------------------------------------------------------------------------------

:HELP
echo.
echo Usage:
echo   "%~nx0" ^[Configuration^]
echo.
echo Parameters:
echo   Configuration - Solution configuration to build (optional; defaults to '%DEFAULT_CFG%').
echo.
exit /b 100
