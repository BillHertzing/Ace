using System;
using ServiceStack;
using ServiceStack.Logging;
using Swordfish.NET.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Ace.Agent.DiskAnalysisServices

{
  public class DiskAnalysisServices : Service
  {
    public static ILog Log = LogManager.GetLogger(typeof(DiskAnalysisServices));
    
     public object Post(DiskAnalysisServicesInitializationRequest request)
    {
      Log.Debug("starting Post(DiskAnalysisServicesInitializationRequest request)");
      DiskAnalysisServicesInitializationDataRequestData DiskAnalysisServicesInitializationDataRequestData = request.DiskAnalysisServicesInitializationDataRequestData;
      //Log.Debug($"You sent me DiskAnalysisServicesInitializationDataRequestData = {DiskAnalysisServicesInitializationDataRequestData}");
      //Log.Debug($"You sent me DiskAnalysisServicesInitializationData = {DiskAnalysisServicesInitializationDataRequestData.DiskAnalysisServicesInitializationData}");
      // Initialize a machine datastructure for this service/user/session/connection
      // Initialize a user datastructure for this service/user/session/connection

      // populate the ConfigurationData response structures
      string placeholder = (DiskAnalysisServicesData.Placeholder != null) ? DiskAnalysisServicesData.Placeholder : "placeholderNotDefined";
      DiskAnalysisServicesConfigurationData DiskAnalysisServicesConfigurationData = new DiskAnalysisServicesConfigurationData(placeholder);
      // populate the UserData response structures
      string placeholderUserData = (DiskAnalysisServicesData.Placeholder != null) ? DiskAnalysisServicesData.Placeholder : "PlaceholderUserData needed";
      DiskAnalysisServicesUserData DiskAnalysisServicesUserData = new DiskAnalysisServicesUserData(placeholderUserData);

      // Create and populate the Response data structure
      DiskAnalysisServicesInitializationResponseData DiskAnalysisServicesInitializationResponseData = new DiskAnalysisServicesInitializationResponseData(DiskAnalysisServicesConfigurationData, DiskAnalysisServicesUserData);
      DiskAnalysisServicesInitializationResponse DiskAnalysisServicesInitializationResponse = new DiskAnalysisServicesInitializationResponse(DiskAnalysisServicesInitializationResponseData);
      // return information about this service/user/session
      Log.Debug($"leaving Post(DiskAnalysisServicesInitializationRequest request), returning {DiskAnalysisServicesInitializationResponse}");
      return DiskAnalysisServicesInitializationResponse;
    }

    public object Post(SetDiskAnalysisServicesConfigurationDataRequest request)
    {
      Log.Debug("starting Post(SetDiskAnalysisServicesConfigurationDataRequest request)");
      SetDiskAnalysisServicesConfigurationDataRequestData setDiskAnalysisServicesConfigurationDataRequestData = request.SetDiskAnalysisServicesConfigurationDataRequestData;
      DiskAnalysisServicesConfigurationData DiskAnalysisServicesConfigurationData = setDiskAnalysisServicesConfigurationDataRequestData.DiskAnalysisServicesConfigurationData;
      Log.Debug($"You sent me DiskAnalysisServicesConfigurationData = {DiskAnalysisServicesConfigurationData}");
      DiskAnalysisServicesData.Placeholder = DiskAnalysisServicesConfigurationData.Placeholder;
      // return information about this service/user/session
      string result = "OK";
      Log.Debug($"leaving Any(SetDiskAnalysisServicesConfigurationDataRequest request), returning {result}");
      return new SetDiskAnalysisServicesConfigurationDataResponse { Result = result };
    }
    public object Post(SetDiskAnalysisServicesUserDataRequest request)
    {
      Log.Debug("starting Post(SetDiskAnalysisServicesUserDataRequest request)");
      SetDiskAnalysisServicesUserDataRequestData setDiskAnalysisServicesUserDataRequestData = request.SetDiskAnalysisServicesUserDataRequestData;
      DiskAnalysisServicesUserData DiskAnalysisServicesUserData = setDiskAnalysisServicesUserDataRequestData.DiskAnalysisServicesUserData;
      Log.Debug($"You sent me DiskAnalysisServicesUserData = {DiskAnalysisServicesUserData}");
      DiskAnalysisServicesData.Placeholder = DiskAnalysisServicesUserData.Placeholder;
      // return information about this service/user/session
      string result = "OK";
      Log.Debug($"leaving Post(SetDiskAnalysisServicesUserDataRequest request), returning {result}");
      return new SetDiskAnalysisServicesConfigurationDataResponse { Result = result };
    }
     public object Post(ReadDiskRequest request)
    {
      Log.Debug("starting Post(ReadDiskRequest)");
      ReadDiskRequestPayload ReadDiskRequestPayload = request.ReadDiskRequestPayload;
      ReadDiskRequestData ReadDiskRequestData = ReadDiskRequestPayload.ReadDiskRequestData;
      bool saveReadDiskData = ReadDiskRequestPayload.SaveReadDiskData;
      ReadDiskParameters readDiskParameters = ReadDiskRequestData.ReadDiskParameters;
      // Async Call the PowerShell script to read the disk
      // ASync Update the SQL Database with the results
      // ToDo: figure out how to integrate a CancellationToken

      // update the Plugin Data Structure with the data from the response
      //DiskAnalysisServicesData.PluginRootCOD.Add("test1", 100);
      Log.Debug("leaving Post(ReadDiskRequest)");
      ReadDiskResponseData ReadDiskResponseData = new ReadDiskResponseData("Placeholder);
      ReadDiskResponsePayload ReadDiskResponsePayload = new ReadDiskResponsePayload(ReadDiskResponseData);
      ReadDiskResponse ReadDiskResponse = new ReadDiskResponse();
      ReadDiskResponse.ReadDiskResponsePayload = ReadDiskResponsePayload;
      Log.Debug("Leaving Post(ReadDiskRequest)");
      return ReadDiskResponse;
    }
    public DiskAnalysisServicesData DiskAnalysisServicesData { get; set; }
  }


}
