// Required for the HttpClient
using System.Net.Http;
using System.Threading.Tasks;
// using Ace.Agent.GUIServices;
// Required for the logger/logging
//using Blazor.Extensions.Logging;
using Microsoft.AspNetCore.Components;
// Required for the logger/logging

namespace Ace.AceGUI.Pages {
    public class GUIServicesCodeBehind : ComponentBase
  {
    // Displayed on the .cshtml page
    public string verifyGUIResult;
    // Access the builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type registered in the DI container 
    [Inject]
    HttpClient HttpClient {
      get; set;
    }
    protected override async Task OnInitAsync()
    {

      //Logger.LogDebug($"starting GUIServices.OnInitAsync");
      //VerifyGUIResponse verifyGUIResponse = await HttpClient.GetJsonAsync<VerifyGUIResponse>("/VerifyGUI?format=json");
      //Logger.LogDebug($"GUIServices.OnInitAsyncGetJsonAsync returned {verifyGUIResponse.Result}");
      //verifyGUIResult = verifyGUIResponse.Result;
      //Logger.LogDebug($"leaving GUIServices.OnInitAsync");
    }
    // Access the Logging extensions registered in the DI container
    /*
    [Inject]
    protected ILogger<BaseServicesCodeBehind> Logger {
      get; set;
    }
        */
  }
}
