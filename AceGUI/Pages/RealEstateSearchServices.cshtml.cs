// Required for the HttpClient
using System.Net.Http;
using System.Threading.Tasks;
using Ace.AceService.RealEstateSearchServices.Models;
// Required for the logger/logging
using Blazor.Extensions.Logging;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.Extensions.Logging;

namespace Ace.AceGUI.Pages {
    public class RealEstateSearchServicesCodeBehind : BlazorComponent {
    // Eventually replace with localization
    public const string labelForGoogleAPIKeyPassPhrase = "Enter Google passphrase to decrypt Google APIKey stored in iCacheProvider";
    public const string labelForSavingGoogleAPIKeyPassPhrase = "save the Google APIKey passphrase on this machine?";
    public const string placeHolderForGoogleAPIKeyPassPhrase = "GoogleAPI Key Passphrase";
    public const string labelForGoogleAPIKey = "Enter Google APIKey";
    public const string labelForSavingGoogleAPIKey = "save the Google APIKey for this application?";
    public const string placeHolderForGoogleAPIKey = "GoogleAPI Key";
    public const string labelForHomeAwayAPIKeyPassPhrase = "Enter HomeAway passphrase to decrypt HomeAway APIKey stored in iCacheProvider";
    public const string labelForSavingHomeAwayAPIKeyPassPhrase = "save the HomeAway APIKey passphrase on this machine?";
    public const string placeHolderForHomeAwayAPIKeyPassPhrase = "HomeAwayAPI Key Passphrase";
    public const string labelForHomeAwayAPIKey = "Enter HomeAway APIKey";
    public const string labelForSavingHomeAwayAPIKey = "save the HomeAway APIKey for this application?";
    public const string placeHolderForHomeAwayAPIKey = "HomeAwayAPI Key";

    // This syntax adds to the class a Method that accesses the DI container, and retrieves the instance having the specified type from the DI container. In this case, we are accessing a builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type. This method call returns that object from the DI container  
    [Inject]
    HttpClient HttpClient
    {
        get;
        set;
    }

        protected override async Task OnInitAsync() {
            Logger.LogDebug($"Starting OnInitAsync");
            Logger.LogDebug($"Calling GetJsonAsync<RealEstateSearchServicesInitializationResponse>");
            RealEstateSearchServicesInitializationResponse = await HttpClient.GetJsonAsync<RealEstateSearchServicesInitializationResponse>($"/RealEstateSearchServicesInitialization/{RealEstateSearchServicesInitializationRequestParameters}?format=json");
            Logger.LogDebug($"Returned from GetJsonAsync<RealEstateSearchServicesInitializationResponse>");
            Logger.LogDebug($"Leaving OnInitAsync");
        }

    public async Task SubmitAPIInformation()
    {
      Logger.LogDebug($"Starting SubmitAPIInformation");
      Logger.LogDebug($"Calling GetJsonAsync<SubmitGoogleAPIKeyPassPhraseResponse>");
      SubmitGoogleAPIKeyPassPhraseResponse _submitGoogleAPIKeyPassPhraseResponse =
          await HttpClient.GetJsonAsync<SubmitGoogleAPIKeyPassPhraseResponse>($"/SubmitGoogleAPIKeyPassPhrase/{GoogleAPIKeyPassPhrase}?format=json");
      Logger.LogDebug($"Returned from GetJsonAsync<SubmitGoogleAPIKeyPassPhraseResponse>");
      SubmitGoogleAPIKeyPassPhraseResponse = _submitGoogleAPIKeyPassPhraseResponse;
      // Notify the parent page that data needs to be updated (??)
      //OnSelectionMade?.Invoke(countries);
      Logger.LogDebug($"Leaving SubmitAPIInformation");
    }

    // Access the Logging extensions registered in the DI container
    [Inject]
    public ILogger<RealEstateSearchServicesCodeBehind> Logger
    {
        get;
        set;
    }

    public RealEstateSearchServicesInitializationResponse RealEstateSearchServicesInitializationResponse { get; set; }
    public bool RealEstateSearchServicesInitializationResponseOK = false;
    public string RealEstateSearchServicesInitializationRequestParameters = "None";
    public string GoogleAPIKeyPassPhrase { get; set; }
    public string GoogleAPIKeyPassPhrasePlaceHolder { get; set; } = placeHolderForGoogleAPIKeyPassPhrase;
    public bool GoogleAPIKeyPassPhraseSave { get; set; }
    public string GoogleAPIKey { get; set; }
    public string GoogleAPIKeyPlaceHolder { get; set; } = placeHolderForGoogleAPIKey;
    public bool GoogleAPIKeySave { get; set; }
    public string HomeAwayAPIKeyPassPhrase { get; set; }
    public string HomeAwayAPIKeyPassPhrasePlaceHolder { get; set; } = placeHolderForHomeAwayAPIKeyPassPhrase;
    public bool HomeAwayAPIKeyPassPhraseSave { get; set; }
    public string HomeAwayAPIKey { get; set; }
    public string HomeAwayAPIKeyPlaceHolder { get; set; } = placeHolderForHomeAwayAPIKey;
    public bool HomeAwayAPIKeySave { get; set; }

    public SubmitGoogleAPIKeyPassPhraseResponse SubmitGoogleAPIKeyPassPhraseResponse { get; set; }
  }
}
