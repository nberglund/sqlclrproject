@echo off

echo .
echo This will update the YukonDeploy dll used by SqlCLRProject and DeployAddIn in the GAC

echo Installation starting ...
echo About to re-install the YukonDeploy.dll MSBUILD task. Make sure you are executing this from a command window where gacutil is on the path.
pause

gacutil -i yukondeploy.dll /f

echo Installation of the YukonDeploy.dll MSBUILD task completed OK.

:exit
pause
