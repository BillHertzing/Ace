using System;
using System.Collections.Generic;
using System.Text;


namespace Ace.Agent.DiskAnalysisServices {

    public partial class DiskAnalysisServicesData {

        // ToDo: Make the creation of All of the COD's herein Lazy

        public ConfigurationData ConfigurationData { get; set; }
        public UserData UserData { get; set; }

        void ConstructConfigurationData () {
            ConfigurationData=new DiskAnalysisServices.ConfigurationData(4096);
        }
        void ConstructUserData() {
            UserData=new DiskAnalysisServices.UserData();
        }
    }
}
