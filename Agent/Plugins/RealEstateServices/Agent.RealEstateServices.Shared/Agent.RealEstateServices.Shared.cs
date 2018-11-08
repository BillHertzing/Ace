using System;
using System.Collections.Generic;

namespace Ace.Agent.RealEstateServices
{
  #region RealEstateServicesConfigurationData
    public class RealEstateServicesConfigurationData {
        public RealEstateServicesConfigurationData() : this(string.Empty, string.Empty) { }
        public RealEstateServicesConfigurationData(string google_API_URI, string homeAway_API_URI) {
            Google_API_URI = google_API_URI;
            HomeAway_API_URI = homeAway_API_URI;
        }

        public string Google_API_URI { get; set; }

        public string HomeAway_API_URI { get; set; }
    }

  #endregion RealEstateServicesConfigurationData

  #region RealEstateServicesUserData
  public class RealEstateServicesUserData {
      public RealEstateServicesUserData() : this(string.Empty, string.Empty, string.Empty, string.Empty) { }
      public RealEstateServicesUserData(string googleAPIKeyEncrypted, string homeAwayAPIKeyEncrypted, string googleAPIKeyPassPhrase, string homeAwayAPIKeyPassPhrase) {
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
  public class SetRealEstateServicesConfigurationDataRequestData {
      public SetRealEstateServicesConfigurationDataRequestData() : this(new RealEstateServicesConfigurationData(),
                                                                              false)
      { }
      public SetRealEstateServicesConfigurationDataRequestData(RealEstateServicesConfigurationData realEstateServicesConfigurationData, bool saveConfigurationData) {
          RealEstateServicesConfigurationData = realEstateServicesConfigurationData;
          SaveConfigurationData = saveConfigurationData;
      }

      public RealEstateServicesConfigurationData RealEstateServicesConfigurationData { get; set; }

      public bool SaveConfigurationData { get; set; }
  }

    public class SetRealEstateServicesConfigurationDataResponse {
        public SetRealEstateServicesConfigurationDataResponse() : this(string.Empty) { }
        public SetRealEstateServicesConfigurationDataResponse(string result) { Result = result; }

        public string Result { get; set; }
    }

  #endregion RealEstateServicesSetConfigurationData

  #region RealEstateServicesGetConfigurationData
  public class GetRealEstateServicesConfigurationDataRequestData {
      public GetRealEstateServicesConfigurationDataRequestData() : this(string.Empty) { }
      public GetRealEstateServicesConfigurationDataRequestData(string placeholder) { Placeholder = placeholder; }

      public string Placeholder { get; set; }
  }

    public class GetRealEstateServicesConfigurationDataResponse {
        public GetRealEstateServicesConfigurationDataResponse() : this(new RealEstateServicesConfigurationData())
        { }
        public GetRealEstateServicesConfigurationDataResponse(RealEstateServicesConfigurationData realEstateServicesConfigurationData)
        { RealEstateServicesConfigurationData = realEstateServicesConfigurationData; }

        public RealEstateServicesConfigurationData RealEstateServicesConfigurationData { get; set; }
    }

  #endregion RealEstateServicesGetConfigurationData

  #region RealEstateServicesSetUserData
  public class SetRealEstateServicesUserDataRequestData {
      public SetRealEstateServicesUserDataRequestData() : this(new RealEstateServicesUserData(), false) { }

      public SetRealEstateServicesUserDataRequestData(RealEstateServicesUserData realEstateServicesUserData, bool userDataSave) {
          RealEstateServicesUserData = realEstateServicesUserData;
          UserDataSave = userDataSave;
      }

      public RealEstateServicesUserData RealEstateServicesUserData { get; set; }

      public bool UserDataSave { get; set; }
  }

    public class SetRealEstateServicesUserDataResponse {
        public string Result { get; set; }
    }

  #endregion RealEstateServicesSetUserData

  #region RealEstateServicesGetUserData
  public class GetRealEstateServicesUserDataRequestData {
      public GetRealEstateServicesUserDataRequestData() : this(string.Empty) { }
      public GetRealEstateServicesUserDataRequestData(string placeholder) { Placeholder = placeholder; }

      public string Placeholder { get; set; }
  }

    public class GetRealEstateServicesUserDataResponse {
        public GetRealEstateServicesUserDataResponse() : this(new RealEstateServicesUserData()) { }
        public GetRealEstateServicesUserDataResponse(RealEstateServicesUserData RealEstateServicesUserData)
        { this.RealEstateServicesUserData = RealEstateServicesUserData; }

        public RealEstateServicesUserData RealEstateServicesUserData { get; set; }
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
  public class RealEstateServicesInitializationData {
      public RealEstateServicesInitializationData() : this(string.Empty) { }

      public RealEstateServicesInitializationData(string placeholder)
      { Placeholder = placeholder; }

      public string Placeholder { get; set; }
  }

    public class RealEstateServicesInitializationDataRequestData {
        public RealEstateServicesInitializationDataRequestData() : this(new RealEstateServicesInitializationData())
        { }
        public RealEstateServicesInitializationDataRequestData(RealEstateServicesInitializationData realEstateServicesInitializationData)
        { RealEstateServicesInitializationData = realEstateServicesInitializationData; }

        public RealEstateServicesInitializationData RealEstateServicesInitializationData { get; set; }
    }

    public class RealEstateServicesInitializationResponseData {
        public RealEstateServicesInitializationResponseData() : this(new RealEstateServicesConfigurationData(),
                                                                           new RealEstateServicesUserData())
        { }
        public RealEstateServicesInitializationResponseData(RealEstateServicesConfigurationData realEstateServicesConfigurationData, RealEstateServicesUserData realEstateServicesUserData) {
            RealEstateServicesConfigurationData = realEstateServicesConfigurationData;
            RealEstateServicesUserData = realEstateServicesUserData;
        }

        public RealEstateServicesConfigurationData RealEstateServicesConfigurationData { get; set; }

        public RealEstateServicesUserData RealEstateServicesUserData { get; set; }
    }
  #endregion RealEstateServicesDoInitialization
}

