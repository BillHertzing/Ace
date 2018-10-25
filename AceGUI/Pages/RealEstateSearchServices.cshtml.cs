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
    public bool realEstateSearchServicesInitializationResponseOK = false;
    public string RealEstateSearchServicesInitializationRequestParameters = "None";

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
            realEstateSearchServicesInitializationResponseOK = true;
            Logger.LogDebug($"Leaving OnInitAsync");
        }

    public async Task SubmitGoogleAPIKeyPassPhrase()
    {
      Logger.LogDebug($"Starting SubmitGoogleAPIKeyPassPhrase");
      Logger.LogDebug($"Calling GetJsonAsync<SubmitGoogleAPIKeyPassPhraseResponse>");
      SubmitGoogleAPIKeyPassPhraseResponse _submitGoogleAPIKeyPassPhraseResponse =
          await HttpClient.GetJsonAsync<SubmitGoogleAPIKeyPassPhraseResponse>($"/SubmitGoogleAPIKeyPassPhrase/{GoogleAPIKeyPassPhrase}?format=json");
      Logger.LogDebug($"Returned from GetJsonAsync<SubmitGoogleAPIKeyPassPhraseResponse>");
      SubmitGoogleAPIKeyPassPhraseResponse = _submitGoogleAPIKeyPassPhraseResponse;
      // Notify the parent page that data needs to be updated (??)
      //OnSelectionMade?.Invoke(countries);
      Logger.LogDebug($"Leaving SubmitGoogleAPIKeyPassPhrase");
    }

    // Access the Logging extensions registered in the DI container
    [Inject]
    public ILogger<RealEstateSearchServicesCodeBehind> Logger
    {
        get;
        set;
    }

    public RealEstateSearchServicesInitializationResponse RealEstateSearchServicesInitializationResponse { get; set; }

    public string GoogleAPIKeyPassPhrase { get; set; }
    public string GoogleAPIKeyPassPhrasePlaceHolder { get; set; } = "Resource.GoogleAPIKeyPassPhrasePlaceHolder";
    public SubmitGoogleAPIKeyPassPhraseResponse SubmitGoogleAPIKeyPassPhraseResponse { get; set; }
  }
}
