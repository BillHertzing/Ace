# Ace
Computer Control Center

This is a project for people who want to use technology to make  a difference for good in the world.

<grandiose>
ToDo: Insert lyrical passionate moving passage about how this software project will evolve over time into a world spanning application that will make the lives of everyone on earth better :-)
</grandiose>

At the moment, it is a lot of ideas and some few lines of code in this repository.

There are a lot of areas to be designed and implemented. A partial list includes.

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

Starting with the core piece, called AceAgent, at this time (May 2018), there are efforts by language and OS vendors to 'write once, run everywhere", as there have been since computers were invented. The initial projects for AceAgent will be on Windows .Net, using Core and Standard as much as possible, and using code specific to Windows (AceService), Linux (AceDaemon), Android (AceTBD), and iOS (AceTBD) as necessary to when the needed functionality is not present in Core or Standard. Ports of the AceAgent's machine/OS dependent pieces to less common devices and OSs are encouraged.

The peer-to-peer mesh as of May 22018 is a set of REST APIs that allow the agents to communicate with each other and to the GUI. Data Transfer Objects (DTOs) define the format of the data as it is communicated over these API endpoints. Messaging between the AceAgents and the AceGUIs will support normal REST APIs and Publish/Subscribe architectures as well. Ports of the peer-to-peer Publish/Subscribe mesh to multiple messaging brokers are encouraged.

The GUI design can be done many different ways, and the project encourages many different implementations. The first GUI implementation and current primary GUI (May 2018) is written in C# and JS, using the Blazor project from Microsoft. As Blazor is experimental at this time, there may be a chance the technology will change or be superseded. But as Blazor holds forth the promise that all of the GUI can be written on C#, and that object definitions can be shared verbatim across the server and client sides of a communication channel, as well as define the DTOs that transport data across process and computer boundaries, this will be the technology for the initial implementation of the AceGUI. 

Feature request:
personal fitness:
Take picture of food/dinner from phone, estimate calories, add to daily food intake log.
