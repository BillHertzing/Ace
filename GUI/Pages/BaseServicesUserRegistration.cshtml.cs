// Required for the HttpClient
using System.Net.Http;
using System.Threading.Tasks;
using Ace.Agent.BaseServices;
// Required for the logger/logging
using Blazor.Extensions.Logging;
// Required for Blazor
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
// Required for the logger/logging
using Microsoft.Extensions.Logging;
// This is the area common in master branch for rebasing branches for adding ServiceStack text and ServiceStack JsonHttpClient
//using ServiceStack;
//using ServiceStack.Text
//using ServiceStack.JsonHttpClient;
//using ServiceStack.Auth

namespace Ace.AceGUI.Pages {
  public class BaseServicesUserRegistrationCodeBehind : ComponentBase
  {
   
    #region string constants
    // Eventually replace with localization
    #region Configuration Data (string constants)
    public const string labelForUserName = "User name:";
    public const string placeHolderForUserName = "enter a valid user name";
    public const string labelForUserRegistrationSubmitButton = "Register New User";
    #endregion
    #endregion

    #region Access Objects registered in the DI container
    // This syntax adds to the class a Method that accesses the DI container, and retrieves the instance having the specified type from the DI container.
    // Access the builtin Blazor service that has registered a pre-configured and extended object as a HTTPClient type registered in the DI container
    [Inject]
    HttpClient HttpClient {
      get;
      set;
    }

    // Access the Logging extensions registered in the DI container
    [Inject]
    public ILogger<BaseServicesCodeBehind> Logger {
      get;
      set;
    }

    #endregion

    #region Page Initialization Handler
    protected override async Task OnInitAsync() {
      // There is currently no need to initialize state or call the Agent during this components initialization, but keep the logging statements here to better understand component/page flow
      Logger.LogDebug($"Starting OnInitAsync");
      Logger.LogDebug($"Leaving OnInitAsync");
    }
    public BaseServicesInitializationRspPayload BaseServicesInitializationRspPayload { get; set; }
    #endregion

    #region User Registration Methods and Properties
    protected async Task UserRegistrationSubmit()
    {
      Logger.LogDebug($"Starting UserRegistrationSubmit");
      //ServiceStack.Register registerDTO = new ServiceStack.Register();
      Register registerDTO = new Register();
      registerDTO.Name = UserName;
      Logger.LogDebug($"Calling PostJsonAsync<BaseServicesUserRegistrationRspDTO> with registerDTO ={registerDTO}");
      //RegisterResponseDTO = await HttpClient.PostJsonAsync<ServiceStack.RegisterResponse>("Register", registerDTO);
      RegisterResponseDTO = await HttpClient.PostJsonAsync<RegisterResponse>("Register", registerDTO);
      Logger.LogDebug($"Returned from PostJsonAsync<ServiceStack.RegisterResponse>, RegisterResponseDTO = {RegisterResponseDTO}");
      BaseServicesUserRegistrationRspDTOExpanded = RegisterResponseDTO.ToString();
      Logger.LogDebug($"Leaving UserRegistrationSubmit");
    }
    //ServiceStack.RegisterResponse RegisterResponseDTO { get; set; }
    RegisterResponse RegisterResponseDTO { get; set; }
    public string BaseServicesUserRegistrationRspDTOExpanded { get; set; }
    public string UserName { get; set; }
    #endregion
    // The following Register and Response classes are just placeholder, eventually the GUI will use the classes defined in the ServiceStack Auth namespace
    class Register
    {
      public string Name { get; set; }
    }
    class RegisterResponse
    {
      public string Result { get; set; }
    }
  }



  }
