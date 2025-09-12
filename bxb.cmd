@echo off
REM bxb.cmd (DOS/Windows batch wrapper for bxb.fsx)
REM Run as: bxb [options]

@dotnet fsi %~dp0bxb.fsx %*