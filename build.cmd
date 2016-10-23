@echo off
cls
.paket\paket.exe restore
packages\FAKE\tools\Fake.exe build.fsx
pause