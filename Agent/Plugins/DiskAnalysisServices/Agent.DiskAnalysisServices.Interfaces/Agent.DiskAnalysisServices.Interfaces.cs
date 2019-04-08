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
      DiskAnalysisServicesInitializationDataRequestData diskAnalysisServicesInitializationDataRequestData = request.DiskAnalysisServicesInitializationDataRequestData;
      //Log.Debug($"You sent me DiskAnalysisServicesInitializationDataRequestData = {DiskAnalysisServicesInitializationDataRequestData}");
      //Log.Debug($"You sent me DiskAnalysisServicesInitializationData = {DiskAnalysisServicesInitializationDataRequestData.DiskAnalysisServicesInitializationData}");
      // Initialize the plugin's datastructure for this service/user/session/connection
      DiskAnalysisServicesInitializationData diskAnalysisServicesInitializationData = diskAnalysisServicesInitializationDataRequestData.DiskAnalysisServicesInitializationData;

      // ToDo: populate the plugin's datastructure's post-initialization data fields
      // Create and populate an instance of the DiskAnalysisServicesConfigurationData
      //string placeholderConfigurationData = (DiskAnalysisServicesConfigurationData.Placeholder != null) ? DiskAnalysisServicesConfigurationData.Placeholder : "placeholderConfigurationNotDefined";
      string placeholderConfigurationData = string.Empty;
      DiskAnalysisServicesConfigurationData diskAnalysisServicesConfigurationData = new DiskAnalysisServicesConfigurationData(placeholderConfigurationData);
      // Create and populate an instance of the DiskAnalysisServicesUserData
      string placeholderUserData = string.Empty;
      //string placeholderUserData = (DiskAnalysisServicesUserData.Placeholder != null) ? DiskAnalysisServicesUserData.Placeholder : "PlaceholderUserData needed";
      DiskAnalysisServicesUserData diskAnalysisServicesUserData = new DiskAnalysisServicesUserData(placeholderUserData);

      // Create and populate the Response data structure
      DiskAnalysisServicesInitializationResponseData DiskAnalysisServicesInitializationResponseData = new DiskAnalysisServicesInitializationResponseData(diskAnalysisServicesConfigurationData, diskAnalysisServicesUserData);
      DiskAnalysisServicesInitializationResponse DiskAnalysisServicesInitializationResponse = new DiskAnalysisServicesInitializationResponse(DiskAnalysisServicesInitializationResponseData);
      // return information about this service/user/session
      Log.Debug($"leaving Post(DiskAnalysisServicesInitializationRequest request), returning {DiskAnalysisServicesInitializationResponse}");
      return DiskAnalysisServicesInitializationResponse;
    }

    public object Post(SetDiskAnalysisServicesConfigurationDataRequest request)
    {
      Log.Debug("starting Post(SetDiskAnalysisServicesConfigurationDataRequest request)");
      SetDiskAnalysisServicesConfigurationDataRequestData setDiskAnalysisServicesConfigurationDataRequestData = request.SetDiskAnalysisServicesConfigurationDataRequestData;
      DiskAnalysisServicesConfigurationData diskAnalysisServicesConfigurationData = setDiskAnalysisServicesConfigurationDataRequestData.DiskAnalysisServicesConfigurationData;
      Log.Debug($"You sent me DiskAnalysisServicesConfigurationData = {diskAnalysisServicesConfigurationData}");
      // Update the DiskAnalysisServicesConfigurationData in the Data assembly
      //DiskAnalysisServicesConfigurationData.Placeholder = diskAnalysisServicesConfigurationData.Placeholder;
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
      DiskAnalysisServicesUserData.Placeholder = DiskAnalysisServicesUserData.Placeholder;
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
      ReadDiskResponseData ReadDiskResponseData = new ReadDiskResponseData("Placeholder");
      ReadDiskResponsePayload ReadDiskResponsePayload = new ReadDiskResponsePayload(ReadDiskResponseData);
      ReadDiskResponse ReadDiskResponse = new ReadDiskResponse();
      ReadDiskResponse.ReadDiskResponsePayload = ReadDiskResponsePayload;
      Log.Debug("Leaving Post(ReadDiskRequest)");
      return ReadDiskResponse;
    }
    public DiskAnalysisServicesData DiskAnalysisServicesData { get; set; }
  }


}
