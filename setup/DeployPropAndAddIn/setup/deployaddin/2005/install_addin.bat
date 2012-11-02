@echo off
echo About to install the deployment Add-In.

pause

if "%1" == "XP" SET VSPATH=%homedrive%"%homepath%\My Documents\Visual Studio 2005\Addins"
if "%1" == "LH" SET VSPATH=%homedrive%"%homepath%\Documents\Visual Studio 2005\Addins"


if not exist %VSPATH% (

echo The directory for addins does not exist, about to create it.
echo .
mkdir %VSPATH%
)



xcopy *.* %VSPATH% /I /E /Y /EXCLUDE:install_addin.bat

del %VSPATH%\install_addin.bat
echo Installation of the deployment Add-In completed OK.

:end 