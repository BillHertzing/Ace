# How to Build Debug, and Deploy the app

# Versioning
- GitHub and Semantic Versioning is used for he ATAP libraries, for the Ace program, and for the Blazor GUI. LiquiBase is used for versioning the MS SQL Server database used in the DiskAnalysis plugin

# Directory.Build.Props
- Do not put <TargetFramework> or <TargetFrameworks> in here

# GUI
- The .csproj should specify <TargetFrameworks>netstandard2.0</TargetFrameworks>. Note the use of the plural form; this ensures a subdirectory will be created under Debug and release for netstandard2.0

# Dependencies
The GUI project (client) should depend on ServiceStack.Interfaces assembly in order to share DTO projects/assemblies/classes with the Agent (server) executables. But it may not depend on ServiceStack assembly, as that brings in too many other subdependencies, which will break the ILLinker step in building if it is referenced.

## Publish Profiles
- There is a DebugFolderProfile and a ReleasePublishProfile. Both publish to the FileSystem. The Debug publishs to a subdir relative to the GUI project. The Release publishes to a Release subdirectory relative to the root of the solution.

## Nuget Packaging
- TBD

# AceService
Located just below the solution, this folder holds the projects/assemblies that make up the Windows-specific version of the Agent
## AceService
- The .csproj should specify  <TargetFramework>net47</TargetFramework>. Note the use of the singular form; this ensures that there is no subdirectory created below Debug.

## AceService.GUIServices.Models
- The .csproj should specify <TargetFrameworks>netstandard2.0;net47</TargetFrameworks>. This will ensure that assemblies are built for both target frameworks. One will be linked to the GUI, the other will be linked to the AceService.
- The project (server) should depend on ServiceStack.Interfaces assembly in order to share DTO projects/assemblies/classes with the GUI (client) Blazor app.

## AceService.GUIServices.Interfaces
- The .csproj should specify <TargetFrameworks>net47</TargetFrameworks>. Note the use of the plural form; this ensures a subdirectory will be created under Debug and Release  for netstandard2.0
- The project (server) should depend on ServiceStack.Interfaces assembly in order to share DTO projects/assemblies/classes with the GUI (client) Blazor app.

# NuGet Packages Used
## Blazor

## Agent
### AceService
Newtonsoft
TimePerdiolibrary
### UnitTesting
Xunit
MOQ
FluentAssertions

# Runtime Notes
## Windows Service
### on Windows 10
#### Prerequisites

0.Install MySQL for Windows
Download the Windows Installer for community edition, install, taking all defaults.

Install Microsft SQL Server with Graph databases to support the DiskAnalysis plugin

1. [Install the Windows Subsytem for Linux (WSL) features in Windows 10](https://docs.microsoft.com/en-us/windows/wsl/install-win10)
reboot

Install Redis for Ubuntu
https://tecadmin.net/install-redis-ubuntu/

sudo apt-get update
sudo apt-get upgrade
sudo apt-get install redis-server
sudo systemctl enable redis-server.service
/etc/init.d/redis-server restart
>redis-cli
127.0.0.1:6379> enter ping expect pong

2. Create a Task Scheduler task to start the WSL subsystem on computer start.

    Inside of WSL, create a .bat file in shell:startup that will run bash.exe and tell bash to run crontab in a forever loop.
	
    Inside of WSL, create a shell script to start all necessary services (Redis, Fluree), and set the script to executable [WSL Tips: Starting Linux Background Services on Windows Login](https://dev.to/ironfroggy/wsl-tips-starting-linux-background-services-on-windows-login-3o98)
    
	Enable password-less sudo on the shell script (sudo visudo)[]()
   
    Put the shell script to start services into cron [How to run Ubuntu service on Windows (at startup)?](https://superuser.com/questions/1112007/how-to-run-ubuntu-service-on-windows-at-startup?noredirect=1&lq=1)




