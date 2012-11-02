@echo off

if "%1" == "" goto err1
if "%2" == "" goto err1


SET VSVERSION=%1
SET OSVERSION=%2

echo .
echo This will install SqlCLRProject and DeployAddIn 
echo Installation starting ...
echo About to install the YukonDeploy.dll MSBUILD task and the attribute dll deployattributes.dll into the GAC.
pause

call .\yukondeploy\setup.exe

echo Installation of the YukonDeploy.dll MSBUILD task and the attribute dll completed OK.
echo About to install the DeployProperties Executable.
pause

call .\deployprop\setup.exe

echo Installation of the DeployProperties Executable task completed OK.
echo About to install the template files (there may be a slight delay before the installation starts).
pause

call .\templates\%VSVERSION%\VSSQLCLRTemplates.vsi

echo Installation of the templates completed OK.

cd deployaddin
call install_addin.bat %VSVERSION% %OSVERSION%

cd..

echo Ready.
echo .
echo You are now ready to start using the templates and the Add-In.
goto exit

:err1
@echo missing parameter(s) for version of Visual Studio (2005/2008) and version of OS: for XP and Win 2003 use XP and for Vista and Win 2008 use LH.
@echo example: install_all.bat 2008 LH
@echo off
goto end


:exit
pause

:end