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

    public object Post(InitializationRequest request)
    {
      Log.Debug("starting Post(InitializationRequest request)");
      InitializationRequestPayload initializationRequestPayload = request.InitializationRequestPayload;
      Log.Debug($"You sent me InitializationRequestPayload = {initializationRequestPayload}");
      Log.Debug($"You sent me InitializationData = {initializationRequestPayload.InitializationData}");
      // Initialize the plugin's data structures for this service/user/session/connection
      // ToDo: Figure out if the Initialization request from the GUI has any impact on the configuration or user data structures
      InitializationData initializationData = initializationRequestPayload.InitializationData;

      // Copy the Plugin's current ConfigurationData structure to the response
      //ToDo: this is merly a placeholder until ConfigurationData is figured out
      ConfigurationData configurationData = new ConfigurationData(DiskAnalysisServicesData.PluginRootCOD.Keys.ToString());
      // Copy the Plugin's current UserData structure to the response
      //ToDo: this is merly a placeholder until UserData  is figured out
      UserData userData = new UserData(DiskAnalysisServicesData.PluginRootCOD.Values.ToString());

      // Create and populate the Response data structure
      InitializationResponsePayload initializationResponsePayload = new InitializationResponsePayload(configurationData, userData);
      InitializationResponse initializationResponse = new InitializationResponse(initializationResponsePayload);
      // return information about this service/user/session
      Log.Debug($"leaving Post(DiskAnalysisServicesInitializationRequest request), returning {initializationResponse}");
      return initializationResponse;
    }

    public object Post(SetConfigurationDataRequest request)
    {
      Log.Debug("starting Post(SetConfigurationDataRequest request)");
      Log.Debug($"You sent me SetConfigurationDataRequest = {request}");
      SetConfigurationDataRequestPayload setConfigurationDataRequestPayload = request.SetConfigurationDataRequestPayload;
      Log.Debug($"You sent me SetConfigurationDataRequestPayload = {setConfigurationDataRequestPayload}");
      ConfigurationData configurationData = setConfigurationDataRequestPayload.ConfigurationData;
      // ToDo: action to take if "save" is false
      // ToDo: Action to ttake if "save" is true
      // ToDo: Update the DiskAnalysisServicesConfigurationData in the Data assembly
      // DiskAnalysisServicesConfigurationData.Placeholder = diskAnalysisServicesConfigurationData.Placeholder;
      // return information about this service/user/session
      string result = "OK";
      Log.Debug($"leaving Any(SetConfigurationDataRequest request), returning {result}");
      return new SetConfigurationDataResponse(new SetConfigurationDataResponsePayload(result));
    }
    public object Post(SetUserDataRequest request)
    {
      Log.Debug("starting Post(SetUserDataRequest request)");
      Log.Debug($"You sent me SetUserDataRequest = {request}");
      SetUserDataRequestPayload setUserDataRequestPayload = request.SetUserDataRequestPayload;
      Log.Debug($"You sent me SetUserDataRequestPayload = {setUserDataRequestPayload}");
      UserData userData = setUserDataRequestPayload.UserData;
      Log.Debug($"You sent me UserData = {userData}");
      // ToDo: action to take if "save" is false
      // ToDo: Action to ttake if "save" is true
      // ToDo: Update the DiskAnalysisServicesConfigurationData (COD and its subscribers) in the Data assembly
      // return information about this service/user/session
      string result = "OK";
      Log.Debug($"leaving Post(SetUserDataRequest request), returning {result}");
      return new SetUserDataResponse(new SetUserDataResponsePayload(result));
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
