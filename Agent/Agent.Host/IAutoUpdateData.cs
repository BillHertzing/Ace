using System;
//using Squirrel;

namespace Ace.Agent.Host {
    interface IAutoUpdateData
    {
        string CurrentVersion { get; set; }
        string NextVersion { get; set; }
        string SelfUpdatingServiceDistributionLocation { get; set; }
        bool ShowTheWelcomeWizard { get; set; }
        //Squirrel.UpdateManager UpdateManager { get; set; }
    }
}