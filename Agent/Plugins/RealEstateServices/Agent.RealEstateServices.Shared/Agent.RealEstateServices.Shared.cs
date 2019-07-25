using System;
using System.Collections.Generic;

namespace Ace.Plugin.RealEstateServices
{
 
    #region ConfigurationData
    public class ConfigurationData {
        public ConfigurationData() : this(string.Empty, string.Empty) { }
        public ConfigurationData(string google_API_URI, string homeAway_API_URI) {
            Google_API_URI = google_API_URI;
            HomeAway_API_URI = homeAway_API_URI;
        }

        public string Google_API_URI { get; set; }

        public string HomeAway_API_URI { get; set; }
    }

    #endregion

    #region UserData
    public class UserData {
      public UserData() : this(string.Empty, string.Empty, string.Empty, string.Empty) { }
      public UserData(string googleAPIKeyEncrypted, string homeAwayAPIKeyEncrypted, string googleAPIKeyPassPhrase, string homeAwayAPIKeyPassPhrase) {
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

    #endregion

  

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



  
}

