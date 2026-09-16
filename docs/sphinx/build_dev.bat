@echo off
pushd %~dp0

REM Use the local virtualenv if present, otherwise fall back to PATH.
set SPHINXBUILD=.venv\Scripts\sphinx-build.exe
if not exist "%SPHINXBUILD%" set SPHINXBUILD=sphinx-build

REM Always start from a clean slate so pages deleted from src\
REM don't linger in the output.
if exist build rmdir /s /q build

REM The same invocation as build.bat, so a page that fails there fails
REM here too.
REM -d keeps Sphinx's doctree cache out of build\.
REM -W turns warnings into errors so a broken reference (a bad image
REM path, a dead :ref:) fails the build instead of passing silently.
REM -c points Sphinx at conf.py, which lives in this folder, not in src,
REM so that src holds only .rst sources.
%SPHINXBUILD% -b html -W -c . -d .doctrees src build
if errorlevel 1 goto end

REM Where this stops short of build.bat: nothing is copied into docs\,
REM so the published Pages content at the repository root is untouched.
echo.
echo Build finished. Preview at docs\sphinx\build\index.html

:end
popd
