using System.ComponentModel;

namespace ATAP.Utilities.WebHostBuilders.Enumerations {
    // Create an enumeration for the kinds of WebHostBuilders this program knows how to support
    public enum SupportedWebHostBuilders {
        //ToDo: Add [LocalizedDescription("IntegratedIISInProcessWebHostBuilder", typeof(Resource))]
        [Description("IntegratedIISInProcessWebHostBuilder")]
        IntegratedIISInProcessWebHostBuilder,
        [Description("KestrelAloneWebHostBuilder")]
        KestrelAloneWebHostBuilder
    }

}

