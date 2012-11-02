There are four major parts to this project:
  * The MSBUILD task dll (yukondeploy.dll) and the attribute dll (deployattributes.dll) that does all the deployment procedures.
  * A front-end (DeployProperties.exe) for the task dll which allows you to deploy already compiled assemblies.
  * Visual Studio 2005/2008 Project and Item template files.
  * Visual Studio 2005/2008 Add-in for deployment and debugging from inside the VS IDE.
	
If you have an existing installation of SQLCLRProject follow the instructions in UNINSTALL-README.txt.

To set this up; open a command prompt in the .\setup directory and run the install_all.bat file. The file expects two parameters:
  * the first parameter defining if you are installing against Visual Studio 2005 or 2008. Parameters 2005 and 2008 respectively.
  * the second parameter decides if you install on Windows XP/Windows Server 2003 or Vista/Windows Server 2008. Parameter XP and LH respectively.

So to install on Windows Server 2008 for Visual Studio 2008, the command would look like so: install_all.bat 2008 LH

After installation: 
  * The MSBUILD task dll (yukondeploy.dll) together with the attribute dll (deployattribute.dll) has been installed into the GAC. 
    * Both the dll's have also been copied to the ..\Program Files\DevelopMentor\Yukondeploy\bin directory. 
    * Documentation for the YukonDeploy task has been placed in the .\docs directory.
    * A template build file (build.proj) has been placed in the .\buildfiles directory
    * In the .\buildtest directory are a couple of files which allows you to test from command-line the YukonDeploy tasks. Read the README.txt file in that directory for more information. 
  * The GUI executable is located in ..\Program Files\DevelopMentor\DeployProperties, together with documentation in the .\docs directory. 
  * The add-in dll together with a .addin file is placed in the Visual studio's Addins folder (location is user and Visual studio version dependent). 
  * Finally the templates are placed in Visual Studio's Templates folder (location is user and Visual studio version dependent).