@echo off

if "%1" == "" (
echo no param 
goto end
)

if "%2" == "" (
echo no param 
goto end
)

SET OSVERSION=%2

cd %1%
call install_addin.bat %OSVERSION%



:end