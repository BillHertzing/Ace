using System;
using System.Collections.Generic;
using System.Text;

namespace Ace.Agent.BaseServices
{
    #region InitializationData
    public interface IInitializationData {
        string BaseVersion { get; set; }
        string MachineID { get; set; }
        string UserID { get; set; }
    }
    public class InitializationData : IInitializationData {
        public InitializationData() : this(string.Empty, string.Empty, string.Empty) { }

        public InitializationData(string baseVersion, string machineID, string userID) {
            BaseVersion=baseVersion??throw new ArgumentNullException(nameof(baseVersion));
            MachineID=machineID??throw new ArgumentNullException(nameof(machineID));
            UserID=userID??throw new ArgumentNullException(nameof(userID));
        }

        // ToDo: make this a "common" kind of structure that includes Base version, machine id, and user id, and Plugin version if applicable
        public string BaseVersion { get; set; }
        public string MachineID { get; set; }
        public string UserID { get; set; }
    }
    #endregion
}
