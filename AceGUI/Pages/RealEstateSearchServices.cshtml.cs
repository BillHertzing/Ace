using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ace.AceService.RealEstateSearchServices.Models;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Blazor.Extensions.Logging;
// Required for the logger/logging
using Microsoft.Extensions.Logging;
// Required for the HttpClient
using System.Net.Http;

namespace Ace.AceGUI.Pages
{
  public class RealEstateSearchServicesCodeBehind : BlazorComponent
  {

    // Displayed on the .cshtml page
    public PropertySearchResponse propertySearchResponse;

    // This syntax adds to the class a Method that accesses the DI container, and retrieves the instance having the specified type from the DI container. In this case, we are accessing a builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type. This method call returns that object from the DI container  
    [Inject]
    HttpClient HttpClient {
      get; set;
    }

    protected override async Task OnInitAsync()
    {
      Logger.LogDebug($"Starting OnInitAsync");
      Logger.LogDebug($"Calling GetJsonAsync<IsAliveResponse>");
      PropertySearchResponse _propertySearchResponse = await HttpClient.GetJsonAsync<PropertySearchResponse>("/PropertySearch/?format=json");
      Logger.LogDebug($"Returned from GetJsonAsync<IsAliveResponse>");
      propertySearchResponse = _propertySearchResponse;
      Logger.LogDebug($"Leaving OnInitAsync");
    }

    // Access the Logging extensions registered in the DI container
    [Inject]
    public ILogger<RealEstateSearchServicesCodeBehind> Logger {
      get; set;
    }
  }
}
