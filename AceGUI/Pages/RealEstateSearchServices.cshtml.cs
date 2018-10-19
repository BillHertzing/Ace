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
        // Displayed on the .cshtml page
        private PropertySearchResponse propertySearchResponse;

    // This syntax adds to the class a Method that accesses the DI container, and retrieves the instance having the specified type from the DI container. In this case, we are accessing a builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type. This method call returns that object from the DI container  
    [Inject]
    HttpClient HttpClient {
        get;
        set;
    }

        protected override async Task OnInitAsync() {
            Logger.LogDebug($"Starting OnInitAsync");
            Logger.LogDebug($"Calling GetJsonAsync<PropertySearchResponse>");
            PropertySearchResponse _propertySearchResponse = await HttpClient.GetJsonAsync<PropertySearchResponse>("/PropertySearch/AFilter?format=json");
            Logger.LogDebug($"Returned from GetJsonAsync<PropertySearchResponse>");
            propertySearchResponse = _propertySearchResponse;
            Logger.LogDebug($"Leaving OnInitAsync");
        }
    public PropertySearchResponse PropertySearchResponse => propertySearchResponse;

    // Access the Logging extensions registered in the DI container
    [Inject]
    public ILogger<RealEstateSearchServicesCodeBehind> Logger {
        get;
        set;
    }
    }
}
