@echo off
REM bxb-test.cmd (DOS/Windows batch wrapper for bxb-test.fsx)
REM Run as: bxb-test [options]

@dotnet fsi %~dp0bxb-test.fsx %*