using System;

namespace Ace.AceCommon.Plugin.RealEstateSearchServices {
    public class RealEstateSearchServicesConfigurationData {
        public RealEstateSearchServicesConfigurationData() : this(string.Empty, string.Empty) { }
        public RealEstateSearchServicesConfigurationData(string google_API_URI, string homeAway_API_URI) {
            Google_API_URI = google_API_URI;
            HomeAway_API_URI = homeAway_API_URI;
        }

        public string Google_API_URI { get; set; }

        public string HomeAway_API_URI { get; set; }
    }

    public class RealEstateSearchServicesUserData {
        public RealEstateSearchServicesUserData() : this(string.Empty, string.Empty, string.Empty, string.Empty) { }
        public RealEstateSearchServicesUserData(string googleAPIKeyEncrypted, string homeAwayAPIKeyEncrypted, string googleAPIKeyPassPhrase, string homeAwayAPIKeyPassPhrase) {
            GoogleAPIKeyEncrypted = googleAPIKeyEncrypted;
      HomeAwayAPIKeyEncrypted = homeAwayAPIKeyEncrypted;
            GoogleAPIKeyPassPhrase = googleAPIKeyPassPhrase;
            HomeAwayAPIKeyPassPhrase = homeAwayAPIKeyPassPhrase;
        }

        public string GoogleAPIKeyEncrypted { get; set; }

        public string GoogleAPIKeyPassPhrase { get; set; }

        public string HomeAwayAPIKeyEncrypted { get; set; }

        public string HomeAwayAPIKeyPassPhrase { get; set; }
    }

    public class SetRealEstateSearchServicesConfigurationDataRequestData
  {
    public SetRealEstateSearchServicesConfigurationDataRequestData() : this(new RealEstateSearchServicesConfigurationData(),false) { }
    public SetRealEstateSearchServicesConfigurationDataRequestData(RealEstateSearchServicesConfigurationData realEstateSearchServicesConfigurationData, bool saveConfigurationData) {
      RealEstateSearchServicesConfigurationData = realEstateSearchServicesConfigurationData;
      SaveConfigurationData = saveConfigurationData;
    }

        public RealEstateSearchServicesConfigurationData RealEstateSearchServicesConfigurationData { get; set; }

        public bool SaveConfigurationData { get; set; }
    }

    public class SetRealEstateSearchServicesConfigurationDataResponse {
    public SetRealEstateSearchServicesConfigurationDataResponse(): this ("") {}
    public SetRealEstateSearchServicesConfigurationDataResponse(string result) { Result = result; }
        public string Result { get; set; }
    }

    public class GetRealEstateSearchServicesConfigurationDataRequestData {
    public GetRealEstateSearchServicesConfigurationDataRequestData() : this("") { }
    public GetRealEstateSearchServicesConfigurationDataRequestData(string placeholder) { Placeholder = placeholder; }
    public string Placeholder { get; set; }
    }

    public class GetRealEstateSearchServicesConfigurationDataResponse {
    public GetRealEstateSearchServicesConfigurationDataResponse() : this(new RealEstateSearchServicesConfigurationData()) { }
    public GetRealEstateSearchServicesConfigurationDataResponse(RealEstateSearchServicesConfigurationData realEstateSearchServicesConfigurationData) { RealEstateSearchServicesConfigurationData = realEstateSearchServicesConfigurationData; }
        public RealEstateSearchServicesConfigurationData RealEstateSearchServicesConfigurationData { get; set; }
    }

    public class SetRealEstateSearchServicesUserDataRequestData {
        public SetRealEstateSearchServicesUserDataRequestData() :this(new RealEstateSearchServicesUserData (),false) { }

    public SetRealEstateSearchServicesUserDataRequestData(RealEstateSearchServicesUserData realEstateSearchServicesUserData, bool userDataSave) { RealEstateSearchServicesUserData = realEstateSearchServicesUserData;
      UserDataSave = userDataSave;
    }
    public RealEstateSearchServicesUserData RealEstateSearchServicesUserData { get; set; }

        public bool UserDataSave { get; set; }
    }

    public class SetRealEstateSearchServicesUserDataResponse {
        public string Result { get; set; }
    }

    public class GetRealEstateSearchServicesUserDataRequestData {
        public GetRealEstateSearchServicesUserDataRequestData() : this("") { }
    public GetRealEstateSearchServicesUserDataRequestData(string placeholder) { Placeholder = placeholder; }
    public string Placeholder { get; set; }
  }

    public class GetRealEstateSearchServicesUserDataResponse {
        public GetRealEstateSearchServicesUserDataResponse() : this(new RealEstateSearchServicesUserData()) { }
        public GetRealEstateSearchServicesUserDataResponse(RealEstateSearchServicesUserData RealEstateSearchServicesUserData)
        { this.RealEstateSearchServicesUserData = RealEstateSearchServicesUserData; }

        public RealEstateSearchServicesUserData RealEstateSearchServicesUserData { get; set; }
    }

    public class RealEstateSearchServicesInitializationData { 
        public RealEstateSearchServicesInitializationData() :this("") { }
  
    public RealEstateSearchServicesInitializationData(string placeholder)
        { Placeholder = placeholder;    }
        public string Placeholder { get; set; }
    }

  public class RealEstateSearchServicesInitializationDataRequestData
  {
    public RealEstateSearchServicesInitializationDataRequestData() : this(new RealEstateSearchServicesInitializationData()) { }
    public RealEstateSearchServicesInitializationDataRequestData(RealEstateSearchServicesInitializationData realEstateSearchServicesInitializationData) { RealEstateSearchServicesInitializationData = realEstateSearchServicesInitializationData; }
    public RealEstateSearchServicesInitializationData RealEstateSearchServicesInitializationData { get; set; }
  }

  public class RealEstateSearchServicesInitializationResponseData {
        public RealEstateSearchServicesInitializationResponseData() : this(new RealEstateSearchServicesConfigurationData(),
                                                                           new RealEstateSearchServicesUserData())
        { }
        public RealEstateSearchServicesInitializationResponseData(RealEstateSearchServicesConfigurationData realEstateSearchServicesConfigurationData, RealEstateSearchServicesUserData realEstateSearchServicesUserData) {
            RealEstateSearchServicesConfigurationData = realEstateSearchServicesConfigurationData;
            RealEstateSearchServicesUserData = realEstateSearchServicesUserData;
        }

        public RealEstateSearchServicesConfigurationData RealEstateSearchServicesConfigurationData { get; set; }

        public RealEstateSearchServicesUserData RealEstateSearchServicesUserData { get; set; }

    }
}

