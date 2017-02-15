@echo off

setlocal

set TAG=%~n0
set DEFAULT_CFG=Debug

if "%~1" equ "/?" goto HELP
if "%~1" equ "-?" goto HELP
if "%~2" neq "" goto HELP

set CFG=%~1
if "%CFG%" equ "" set CFG=%DEFAULT_CFG%

set TOOLS_DIR=%~dp0\tools
set NUNIT_PKG=NUnit.Runners
set NUNIT_PKG_VER=2.6.4
set NUNIT_DIR=%TOOLS_DIR%\%NUNIT_PKG%
set NUNIT_CONSOLE=%NUNIT_DIR%\tools\nunit-console.exe
set PROJECT_DIR=%~dp0\bin.tests\AnyCpu\%CFG%
set PROJECT_PATH=%PROJECT_DIR%\HarinezumiChess.Tests.dll

call :CLEAN_DIR "%NUNIT_DIR%" || goto ERROR

echo.
echo [%TAG%] NuGet: Installing '%NUNIT_PKG%' version %NUNIT_PKG_VER%...
nuget install "%NUNIT_PKG%" -Version %NUNIT_PKG_VER% -ExcludeVersion -OutputDirectory "%TOOLS_DIR%" -Verbosity detailed -NonInteractive -NoCache || goto ERROR
echo [%TAG%] NuGet: Installing '%NUNIT_PKG%' version %NUNIT_PKG_VER% - DONE.

echo.
echo [%TAG%] NUnit: Executing tests from "%PROJECT_PATH%"...
"%NUNIT_CONSOLE%" "%PROJECT_PATH%" /labels /result="%PROJECT_DIR%\TestResult.xml" || goto ERROR
echo [%TAG%] NUnit: Executing tests from "%PROJECT_PATH%" - DONE.

goto :EOF

:: ------------------------------------------------------------------------------------------------------------------------

:ERROR
echo.
echo *** ERROR has occurred ***
exit /b 1

:CLEAN_DIR
echo.
echo [%TAG%] Cleaning directory "%~1"...
if exist "%~1" (
    rd /s /q "%~1" || exit /b 1
)
echo [%TAG%] Cleaning directory "%~1" - DONE.
goto :EOF

:HELP
echo.
echo Usage:
echo   "%~nx0" ^[Configuration^]
echo.
echo Parameters:
echo   Configuration - Configuration of the artifacts to run NUnit for (optional; defaults to '%DEFAULT_CFG%').
echo.
exit /b 100