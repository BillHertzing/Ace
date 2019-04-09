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
      DiskAnalysisServicesInitializationRequestPayload diskAnalysisServicesInitializationRequestPayload = request.DiskAnalysisServicesInitializationRequestPayload;
      //Log.Debug($"You sent me DiskAnalysisServicesInitializationRequestPayload = {diskAnalysisServicesInitializationRequestPayload}");
      //Log.Debug($"You sent me DiskAnalysisServicesInitializationData = {diskAnalysisServicesInitializationRequestPayload.DiskAnalysisServicesInitializationData}");
      // Initialize the plugin's data structures for this service/user/session/connection
      DiskAnalysisServicesInitializationData diskAnalysisServicesInitializationData = diskAnalysisServicesInitializationRequestPayload.DiskAnalysisServicesInitializationData;

      // ToDo: populate the plugin's data structure's post-initialization data fields
      // Create and populate an instance of the DiskAnalysisServicesConfigurationData
      //string placeholderConfigurationData = (DiskAnalysisServicesConfigurationData.Placeholder != null) ? DiskAnalysisServicesConfigurationData.Placeholder : "placeholderConfigurationNotDefined";
      string placeholderConfigurationData = string.Empty;
      DiskAnalysisServicesConfigurationData diskAnalysisServicesConfigurationData = new DiskAnalysisServicesConfigurationData(placeholderConfigurationData);
      // Create and populate an instance of the DiskAnalysisServicesUserData
      string placeholderUserData = string.Empty;
      //string placeholderUserData = (DiskAnalysisServicesUserData.Placeholder != null) ? DiskAnalysisServicesUserData.Placeholder : "PlaceholderUserData needed";
      DiskAnalysisServicesUserData diskAnalysisServicesUserData = new DiskAnalysisServicesUserData(placeholderUserData);

      // Create and populate the Response data structure
      DiskAnalysisServicesInitializationResponsePayload diskAnalysisServicesInitializationResponseData = new DiskAnalysisServicesInitializationResponsePayload(diskAnalysisServicesConfigurationData, diskAnalysisServicesUserData);
      DiskAnalysisServicesInitializationResponse DiskAnalysisServicesInitializationResponse = new DiskAnalysisServicesInitializationResponse(diskAnalysisServicesInitializationResponseData);
      // return information about this service/user/session
      Log.Debug($"leaving Post(DiskAnalysisServicesInitializationRequest request), returning {DiskAnalysisServicesInitializationResponse}");
      return DiskAnalysisServicesInitializationResponse;
    }

    public object Post(SetDiskAnalysisServicesConfigurationDataRequest request)
    {
      Log.Debug("starting Post(SetDiskAnalysisServicesConfigurationDataRequest request)");
      SetDiskAnalysisServicesConfigurationDataRequestPayload setDiskAnalysisServicesConfigurationDataRequestPayload = request.SetDiskAnalysisServicesConfigurationDataRequestPayload;
      DiskAnalysisServicesConfigurationData diskAnalysisServicesConfigurationData = setDiskAnalysisServicesConfigurationDataRequestPayload.DiskAnalysisServicesConfigurationData;
      Log.Debug($"You sent me DiskAnalysisServicesConfigurationData = {diskAnalysisServicesConfigurationData}");
      // Update the DiskAnalysisServicesConfigurationData in the Data assembly
      // DiskAnalysisServicesConfigurationData.Placeholder = diskAnalysisServicesConfigurationData.Placeholder;
      // return information about this service/user/session
      string result = "OK";
      Log.Debug($"leaving Any(SetDiskAnalysisServicesConfigurationDataRequest request), returning {result}");
      return new SetDiskAnalysisServicesConfigurationDataResponsePayload { Result = result };
    }
    public object Post(SetDiskAnalysisServicesUserDataRequest request)
    {
      Log.Debug("starting Post(SetDiskAnalysisServicesUserDataRequest request)");
      SetDiskAnalysisServicesUserDataRequestPayload setDiskAnalysisServicesUserDataRequestPayload = request.SetDiskAnalysisServicesUserDataRequestPayload;
      DiskAnalysisServicesUserData diskAnalysisServicesUserData = setDiskAnalysisServicesUserDataRequestPayload.DiskAnalysisServicesUserData;
      Log.Debug($"You sent me DiskAnalysisServicesUserData = {diskAnalysisServicesUserData}");
      // return information about this service/user/session
      string result = "OK";
      Log.Debug($"leaving Post(SetDiskAnalysisServicesUserDataRequest request), returning {result}");
      return new SetDiskAnalysisServicesConfigurationDataResponsePayload { Result = result };
    }
     public object Post(ReadDiskRequest request)
    {
      Log.Debug("starting Post(ReadDiskRequest)");
      ReadDiskRequestPayload readDiskRequestPayload = request.ReadDiskRequestPayload;
      ReadDiskRequestData readDiskRequestData = readDiskRequestPayload.ReadDiskRequestData;
      // Setup the long-running task that will read the disk and update the plugin's COD data structure
      // Setup the SSE receiver that will monitor the long-running task
      // return to the caller the callback URL and the ID to allow the caller to connect to teh SSE that monitors the taks and the data 
      // Long running task: Async Call the PowerShell script to read the disk
      // Long running task: ASync Update the SQL Database with the results
      // ToDo: figure out how to integrate a CancellationToken
      // Long running task: update the Plugin Data Structure with the data from the response
      // Long running task: setup the DiskAnalysisServicesData.PluginRootCOD.Add("test1", 100);
      ReadDiskResponseData readDiskResponseData = new ReadDiskResponseData("Placeholder");
      ReadDiskResponsePayload readDiskResponsePayload = new ReadDiskResponsePayload(readDiskResponseData);
      ReadDiskResponse ReadDiskResponse = new ReadDiskResponse();
      ReadDiskResponse.ReadDiskResponsePayload = readDiskResponsePayload;
      Log.Debug("Leaving Post(ReadDiskRequest)");
      return ReadDiskResponse;
    }
    public DiskAnalysisServicesData DiskAnalysisServicesData { get; set; }
  }


}
