#Overview of the ACE software project

Ace is an interface to computing resources. It runs 24/7 on every computer it is installed onto, and communicates with other instances of Ace on other computers, with humans via the GUI and via reports, and with 3rd-party web services.

The Agent consist of many smaller assemblies, many from the ATAP.Utilities NuGet package libraries. These are MIT-licensed assemblies with utility methods and properties that have been released to OSS and MIT-licensed status by ATAP Technology, Inc. as part of their commitment to open software.

The Agent also seeks to maximize code reuseability by putting as much as possible into .Net Standard and .Net Core libraries. One goal of the project is to create the  smallest amount of code that has to be targeted to Net Full, Linux, iOS, or Android to make the Agent run on each osS.

The data that flows between the GUI and the Agent is defined in Data Transfer objects (DTOs), which are .net classes shared between the GUI assemblies and the Agent executable.

The Project architecture (the various components used in each sub-project) is documented here:

Building notes, on how to build and debug the projects, is documented here: <Link to BuildingNotes.md>

Goals, including the blue-sky goals, are documented here: <link to Goals.md>

Attribution, links to blogs and web resources that contributed materially to the design of this project, is here: <link to attribution.md>


*Needs editing below here*
Starting with the core piece, called the Agent, at this time (May 2018), there are efforts by language and OS vendors to 'write once, run everywhere", as there have been since computers were invented. The initial projects for Agent will be on Windows .Net, using Core and Standard as much as possible, and using code specific to Windows (AceService), Linux (AceDaemon), Android (AceTBD), and iOS (AceTBD) as necessary to when the needed functionality is not present in Core or Standard. Ports of the Agent's machine/OS dependent pieces to less common devices and OSs are encouraged.

The peer-to-peer mesh as of May 2018 is a set of REST APIs that allow the agents to communicate with each other and to the GUI. Data Transfer Objects (DTOs) define the format of the data as it is communicated over these API endpoints. Messaging between the Agent and the GUI will support normal REST APIs and Publish/Subscribe architectures as well. Ports of the peer-to-peer Publish/Subscribe mesh to multiple messaging brokers are encouraged.

The GUI design can be done many different ways, and the project encourages many different implementations. The first GUI implementation and current primary GUI  is written in C# and JS, using the Blazor project from Microsoft. As Blazor is experimental at this time, there may be a chance the technology will change or be superseded. But as Blazor holds forth the promise that all of the GUI can be written on C#, and that object definitions can be shared verbatim across the server and client sides of a communication channel, as well as define the DTOs that transport data across process and computer boundaries, this will be the technology for the initial implementation of the GUI. 

Ace on GitHub has a "Features Request" project. It is moderated to prevent abuse. One of the features currently requested for Ace is that the "Features Request" ability be added to Ace itself. Also requested is that Feature Requests provide a clear set of guidelines for what should be considered "abuse" of the feature,

One of the core tenants of this project is to quantify entities' reputation. Reputation should be enhanced by positive behaviors. Reputation should be reduced by negative behaviors

As part of Reputation, bullying is certainly considered negative behavior. "Abuse" of any entity should be considered negative behavior. So it is important that Ace reputation scores should react quickly to such allegations. However the allegations must be validated. The reputation system needs to time-weight behavior so that reputations might be smirched, and yet reputations must recover if allegations prove unfounded.

Another part of the core is the game that rewards cooperative action. Intrinsic in this game is the reputation of the player's in-game avatars. It is a goal of the project that game creation should become a collaborative effort among users interested in this feature.

