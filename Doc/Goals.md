# Ace Goals

There are many kinds of goals to be documented. As the project grows and matures, goals will be achieved, the documentation should evolve as well, moving items from "goals", to "being developed, to "completed features".

"<needs editing below this"

However, I've been looking for a way to leverage the browser as a GUI for an application that runs on multiple operating systems. Most application that need a GUI have to create the GUI specifically for an OS. Blazor brings the ability for developers to write their GUI in C#, publish it to a set of static files, and have any any browser render and run the GUI. An application that leverages .Net, .Net Standard, and .Net Core to run on multipleOS, combined with a Blazor-based GUI, promises to greatly reduce the platform-specific portions of any multi-OS application.


The Agent basically supplies RESTful APIs on a listening port.

The human interface/display of the data supplied by the Agent is done with a Blazor application, called GUI. The GUI, like any Blazor application is written in C#, targets the .Net Standard framework, and builds to a set of static files. 

Any process that can serve static files,and can perform a redirect if an unknown URL comes in on the listening port, can deliver the files necessary to run the GUI to any browser, and provide the necessary support for the Blazor router. Any browser that supports WebAssembly can run the Agent Blazor GUI. One of the Agent PlugIns is designed to provide the necessary support needed to run the Agent Blazor GUI.

Taken all together, the Agent, GUI, and other PlugIns should get closer to the "write once, run everywhere" nirvana that software developers have striven for since the early 1980s 

This example is derived from and the full Ace repository. Agent in its full-blown form is eventually intended to be a node of a peer-to-peer distributed network that provides a large number of features.

Agent should be capable of deploying to extremely tiny memory/process space footprint, and scale up its footprint as features/PlugIns are added. An Agent can be deployed to a device without the GUI feature PlugIn if the footprint must be kept minimal, or if the device does not have a need to provide a GUI. When Agent is deployed with the the Blazor GUI PlugIn loaded, the browser on the device can provide a (hopefully rich) interactive GUI to control the Agent node, and interact with the full network of nodes.

Feature request:
personal fitness:
Take picture of food/dinner from phone, estimate calories, add to daily food intake log.
