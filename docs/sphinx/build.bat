@echo off
pushd %~dp0

REM Use the local virtualenv if present, otherwise fall back to PATH.
set SPHINXBUILD=.venv\Scripts\sphinx-build.exe
if not exist "%SPHINXBUILD%" set SPHINXBUILD=sphinx-build

REM Always start from a clean slate so pages deleted from src\
REM don't linger in the output.
if exist build rmdir /s /q build

REM -d keeps Sphinx's doctree cache out of build\, so it never
REM reaches the published site.
REM -W turns warnings into errors so a broken reference (a bad image
REM path, a dead :ref:) fails the build instead of shipping silently.
REM -c points Sphinx at conf.py, which lives in this folder, not in src,
REM so that src holds only .rst sources.
%SPHINXBUILD% -b html -W -c . -d .doctrees src build
if errorlevel 1 goto end

REM ---------------------------------------------------------------
REM Publish into docs\ -- that folder is what the Pages workflow
REM uploads, so the generated site has to sit at its root.
REM Clear the previously generated files first, but keep this
REM sphinx\ source folder and the Pages control files.
REM ---------------------------------------------------------------
for /d %%D in ("..\*") do (
    if /i not "%%~nxD"=="sphinx" rmdir /s /q "%%D"
)
for %%F in ("..\*") do (
    if /i not "%%~nxF"==".nojekyll" if /i not "%%~nxF"=="CNAME" del /q "%%F"
)

xcopy /e /i /y "build\*" "..\" >nul
if errorlevel 1 goto end

REM Required so GitHub Pages serves Sphinx's _static\ and _images\ folders.
if not exist "..\.nojekyll" type nul > "..\.nojekyll"

echo.
echo Build finished. Published to docs\index.html

:end
popd
