using System;
using System.Collections.Generic;

namespace Ace.AceCommon.Plugin.RealEstateSearchServices {
  #region RealEstateServicesConfigurationData
    public class RealEstateSearchServicesConfigurationData {
        public RealEstateSearchServicesConfigurationData() : this(string.Empty, string.Empty) { }
        public RealEstateSearchServicesConfigurationData(string google_API_URI, string homeAway_API_URI) {
            Google_API_URI = google_API_URI;
            HomeAway_API_URI = homeAway_API_URI;
        }

        public string Google_API_URI { get; set; }

        public string HomeAway_API_URI { get; set; }
    }

  #endregion RealEstateServicesConfigurationData

  #region RealEstateServicesUserData
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

  #endregion RealEstateServicesUserData

  #region PropertySearchPayload
  public class PropertySearchRequestData {
      public PropertySearchRequestData() { }
      public PropertySearchRequestData(SearchParameters searchParameters, ListingParameters listingParameters) {
          SearchParameters = searchParameters;
          ListingParameters = listingParameters;
      }

      public SearchParameters SearchParameters { get; set; }

      public ListingParameters ListingParameters { get; set; }
  }

    public class SearchParameters {
        public SearchParameters() { }
        public SearchParameters(int minBedrooms, int maxBedrooms, decimal minBathrooms, decimal maxBathrooms, decimal centerPointLatitude, decimal centerPointLongitude, decimal distanceInKm) {
            MinBedrooms = minBedrooms;
            MaxBedrooms = maxBedrooms;
            MinBathrooms = minBathrooms;
            MaxBathrooms = maxBathrooms;
            CenterPointLatitude = centerPointLatitude;
            CenterPointLongitude = centerPointLongitude;
            DistanceInKm = distanceInKm;
        }

        decimal CenterPointLatitude { get; set; }

        decimal CenterPointLongitude { get; set; }

        decimal DistanceInKm { get; set; }

        decimal MaxBathrooms { get; set; }

        int MaxBedrooms { get; set; }

        decimal MinBathrooms { get; set; }

        int MinBedrooms { get; set; }
    }

    public class ListingParameters {
        public ListingParameters() { }
        public ListingParameters(bool availability, bool details, bool location, bool rates, bool sites) {
            AVAILABILITY = availability;
            DETAILS = details;
            LOCATION = location;
            RATES = rates;
            SITES = sites;
        }

        bool AVAILABILITY { get; set; }

        bool DETAILS { get; set; }

        bool LOCATION { get; set; }

        bool RATES { get; set; }

        bool SITES { get; set; }
    }

    public class PropertySearchResponseData {
        public PropertySearchResponseData() { }
        public PropertySearchResponseData(List<ListingSearchHit> listingSearchHits)
        {
            ListingSearchHits = listingSearchHits;
        }

        List<ListingSearchHit> ListingSearchHits { get; set; }
    }

    public class ListingSearchHit {
        public ListingSearchHit() { }
        public ListingSearchHit(string listingId, string headline, ListingLocation listingLocation) {
            ListingId = listingId;
            Headline = headline;
            ListingLocation = listingLocation;
        }

        string Headline { get; set; }

        string ListingId { get; set; }

        ListingLocation ListingLocation { get; set; }
    }

    public class ListingLocation {
        public ListingLocation() { }
        public ListingLocation(decimal lat, decimal lng) {
            Lat = lat;
            Lng = lng;
        }

        decimal Lat { get; set; }

        decimal Lng { get; set; }
    }

  #endregion RealEstateServicesPropertySearchData

  #region RealEstateServicesSetConfigurationData
  public class SetRealEstateSearchServicesConfigurationDataRequestData {
      public SetRealEstateSearchServicesConfigurationDataRequestData() : this(new RealEstateSearchServicesConfigurationData(),
                                                                              false)
      { }
      public SetRealEstateSearchServicesConfigurationDataRequestData(RealEstateSearchServicesConfigurationData realEstateSearchServicesConfigurationData, bool saveConfigurationData) {
          RealEstateSearchServicesConfigurationData = realEstateSearchServicesConfigurationData;
          SaveConfigurationData = saveConfigurationData;
      }

      public RealEstateSearchServicesConfigurationData RealEstateSearchServicesConfigurationData { get; set; }

      public bool SaveConfigurationData { get; set; }
  }

    public class SetRealEstateSearchServicesConfigurationDataResponse {
        public SetRealEstateSearchServicesConfigurationDataResponse() : this(string.Empty) { }
        public SetRealEstateSearchServicesConfigurationDataResponse(string result) { Result = result; }

        public string Result { get; set; }
    }

  #endregion RealEstateServicesSetConfigurationData

  #region RealEstateServicesGetConfigurationData
  public class GetRealEstateSearchServicesConfigurationDataRequestData {
      public GetRealEstateSearchServicesConfigurationDataRequestData() : this(string.Empty) { }
      public GetRealEstateSearchServicesConfigurationDataRequestData(string placeholder) { Placeholder = placeholder; }

      public string Placeholder { get; set; }
  }

    public class GetRealEstateSearchServicesConfigurationDataResponse {
        public GetRealEstateSearchServicesConfigurationDataResponse() : this(new RealEstateSearchServicesConfigurationData())
        { }
        public GetRealEstateSearchServicesConfigurationDataResponse(RealEstateSearchServicesConfigurationData realEstateSearchServicesConfigurationData)
        { RealEstateSearchServicesConfigurationData = realEstateSearchServicesConfigurationData; }

        public RealEstateSearchServicesConfigurationData RealEstateSearchServicesConfigurationData { get; set; }
    }

  #endregion RealEstateServicesGetConfigurationData

  #region RealEstateServicesSetUserData
  public class SetRealEstateSearchServicesUserDataRequestData {
      public SetRealEstateSearchServicesUserDataRequestData() : this(new RealEstateSearchServicesUserData(), false) { }

      public SetRealEstateSearchServicesUserDataRequestData(RealEstateSearchServicesUserData realEstateSearchServicesUserData, bool userDataSave) {
          RealEstateSearchServicesUserData = realEstateSearchServicesUserData;
          UserDataSave = userDataSave;
      }

      public RealEstateSearchServicesUserData RealEstateSearchServicesUserData { get; set; }

      public bool UserDataSave { get; set; }
  }

    public class SetRealEstateSearchServicesUserDataResponse {
        public string Result { get; set; }
    }

  #endregion RealEstateServicesSetUserData

  #region RealEstateServicesGetUserData
  public class GetRealEstateSearchServicesUserDataRequestData {
      public GetRealEstateSearchServicesUserDataRequestData() : this(string.Empty) { }
      public GetRealEstateSearchServicesUserDataRequestData(string placeholder) { Placeholder = placeholder; }

      public string Placeholder { get; set; }
  }

    public class GetRealEstateSearchServicesUserDataResponse {
        public GetRealEstateSearchServicesUserDataResponse() : this(new RealEstateSearchServicesUserData()) { }
        public GetRealEstateSearchServicesUserDataResponse(RealEstateSearchServicesUserData RealEstateSearchServicesUserData)
        { this.RealEstateSearchServicesUserData = RealEstateSearchServicesUserData; }

        public RealEstateSearchServicesUserData RealEstateSearchServicesUserData { get; set; }
    }

  #endregion RealEstateServicesGetUserData

  #region RealEstateServicesPropertySearch
  public class PropertySearchRequestPayload {
      public PropertySearchRequestPayload() { }
      public PropertySearchRequestPayload(PropertySearchRequestData propertySearchRequestData, bool savePropertySearchData) {
      PropertySearchRequestData = propertySearchRequestData;
          SavePropertySearchData = savePropertySearchData;
      }

      public PropertySearchRequestData PropertySearchRequestData { get; set; }

      public bool SavePropertySearchData { get; set; }
  }

    public class PropertySearchResponsePayload {
        public PropertySearchResponsePayload() { }
        public PropertySearchResponsePayload(PropertySearchResponseData propertySearchResponseData)
        { PropertySearchResponseData = propertySearchResponseData; }

        public PropertySearchResponseData PropertySearchResponseData { get; set; }
    }

  #endregion RealEstateServicesPropertySearch


  #region RealEstateServicesDoInitialization
  public class RealEstateSearchServicesInitializationData {
      public RealEstateSearchServicesInitializationData() : this(string.Empty) { }

      public RealEstateSearchServicesInitializationData(string placeholder)
      { Placeholder = placeholder; }

      public string Placeholder { get; set; }
  }

    public class RealEstateSearchServicesInitializationDataRequestData {
        public RealEstateSearchServicesInitializationDataRequestData() : this(new RealEstateSearchServicesInitializationData())
        { }
        public RealEstateSearchServicesInitializationDataRequestData(RealEstateSearchServicesInitializationData realEstateSearchServicesInitializationData)
        { RealEstateSearchServicesInitializationData = realEstateSearchServicesInitializationData; }

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
  #endregion RealEstateServicesDoInitialization
}

