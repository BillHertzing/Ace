# Overview
An Agent of technology to help make the world a better place. This is a project for people who want to use technology to make a difference for good in the world. The current state of the project relative to its goals is minuscule, but I hope that changes over the next decade or so.

If you want to know how to get started using the code in this repository, the Docs subfolder contains specific notes on building, installing and using this application.

The project currently runs only on Windows 10. The goal is to have it also run on Linux, MaxOS, iOS, and Android.

# Getting the application
There is no NuGet package nor Chocolatey install package yet. Getting the source code from this repository and building it yourself is the only option now. Easier installation options are planned. Details on how to copy/fork, build, and debug the app can be found in the Doc subfolder.

# Application Goals
The eventual goals are to create software that resides on any/most/smart pieces of electronics, that communicates with nearby instances of itself, to provide a wide range of information services to, and to take action on the on behalf of, the end users. The application also provides information about the current user to nearby instances of the application and to 3rd party web sites. Detailed goals can be found in Goals.md in the Doc subfolder, however, the authoritative list of features that the agent will eventually provide can be found in the Repository's "Requested Features" project.'

# Current features
    * Agent runs on ServiceStack under Windows 10. It runs as a ConsoleApp or as a Windows service.
	* Agent wraps ServiceStack in TopShelf to make it easier to distribute the app as a Windows Service.
    * Agent provides the following basic (built-in) services
        * Basic APIs (Rest)
            * IsAlive
			* GeoCode and reverse GeoCode
        * Built-in cache for write-through storage of data, using Redis
		* Connection to a MySql database, that has User and Role tables to support the concept of authorized users.
			
    * Agent uses a Plugin architecture to provide extensibility
    * Agent implements configuration data, loaded from default compiled-in values, or from a text file, for both base services and Plugins.
    * Agent implements configuration data, loaded from default compiled-in values, or from a text file.
    * Basic implementation of Gateways for the Agent to get information from other 3rd party Web APIs.
    * Blazor GUI for the Agent written as an application to be run in any browser that supports WASM.

# Features actively under development
     * Addition of ServiceStack JSONHttpClient to Blazor GUI
	 * Addition of ServiceStack User and Roles tables to the MySQL database.
	 * Addition of an implementation of configuration data loaded from Environment variables, specifically an API key
	 * The use of usersecrets.json in a VS solution folder to set environment variables when the app is started by VS.
	 * The addition to Gateways and GatewayEntries code to implement the concept of authorization to the 3rd party REST services. Initial implementation is to provide an APIKey for the Google Maps API when invoked to service the GeoCode and Reverse GeCode features of the Agent's basic Services. 

# Current implementation details
    * Entire solution is built with Visual Studio 2017 Version 15.8 
	* Agent is running under .Net Framework 4.7.1.
	* GUI is built against the .Net Standard 2.0  framework
	* Common DTOs between GUI and Agent are built against both frameworks
	
# Future Goals
Creating a large full capability application that will reach millions of people is a grand goal that will require the help and hard work of hundreds of people. This project is just the very start of such an ambitious project. There are a lot of areas in this project that still need to be designed and implemented. A partial list includes.
* The core piece of software that will run on every kind of computer and mobile device.
* The peer-to-peer mesh that will connect these processes to each other.
* The plug-ins and data structures that will extend the core abilities.
* The GUI design to interact with the core, and the GUI plug-ins that will interact with the extended features provided by the plug-ins.
* Version Control, CI, CD, and auto-updates
* Identity, Authentication, Authorization, Verification, all in a distributed ledger.
* Rapid distributed ledger hashing using a hash guessing algorithm that adds value to the world, not just burning electricity.
* Peer-reviewed widespread reputation scores for people and entities.
* Communication discoverability between agents, reputation-based data sharing.
* A fun game that teaches the principals of cooperation as a way to enhance reputation scores of in-game characters.
* Security, Security, Security - and the controls necessary to ensure information herein cannot be unscrupulously manipulated, nor can it be falsified in a way to cause harm.

Each of these areas will need thoughtful design and implementation - All folks who would like to contribute to this are welcome!

In the Docs subfolder are documents with information on designing these future goals.

# Dependencies
## List of Open Source Libraries used by AceCommander
- ServiceStack Version 5.1.1 (Community Edition) URL TBD
- TopShelf TBD
- MedallionShell
- TimePeriodLibrary.NET
- ATAP.Utilities.* Various packages from ATAP technology.





