using System;
using System.Collections.Generic;

namespace Ace.Agent.DiskAnalysisServices
{
  #region ConfigurationData
  public class ConfigurationData
  {
    public ConfigurationData() : this(string.Empty) { }

    public ConfigurationData(string placeholder)
    {
      Placeholder = placeholder;
    }

    public string Placeholder { get; set; }
  }
  #endregion ConfigurationData

  #region UserData
  public class UserData
  {
    public UserData() : this(string.Empty) { }
    public UserData(string placeholder)
    {
      Placeholder = placeholder;
    }

    public string Placeholder { get; set; }
  }
  #endregion UserData

  #region InitializationData
  public class InitializationData
  {
    public InitializationData() : this(string.Empty) { }

    public InitializationData(string placeholder)
    { Placeholder = placeholder; }

    public string Placeholder { get; set; }
  }
  #endregion InitializationData

  #region ReadDiskRequestData and ReadDiskResponseData
  public class ReadDiskRequestData
  {
    public ReadDiskRequestData(): this(string.Empty) { }
    public ReadDiskRequestData(string placeholder)
    {
      Placeholder = placeholder;
    }

    public string Placeholder { get; set; }
  }

  public class ReadDiskResponseData
  {
    public ReadDiskResponseData() : this(-1) { }
    public ReadDiskResponseData(int longRunningTaskID)
    {
      LongRunningTaskID = longRunningTaskID;
    }

    public int LongRunningTaskID { get; set; }
  }
  #endregion ReadDiskRequestData and ReadDiskResponseData

  #region SetDiskAnalysisServicesConfigurationDataRequestPayload and SetDiskAnalysisServicesConfigurationDataResponsePayload
  #region SetConfigurationDataRequestPayload
  public class SetConfigurationDataRequestPayload
  {
    public SetConfigurationDataRequestPayload() : this(new ConfigurationData(),
                                                                            false)
    { }
    public SetConfigurationDataRequestPayload(ConfigurationData configurationData, bool saveConfigurationData)
    {
      ConfigurationData = configurationData;
      SaveConfigurationData = saveConfigurationData;
    }

    public ConfigurationData ConfigurationData { get; set; }

    public bool SaveConfigurationData { get; set; }
  }
  #endregion SetDiskAnalysisServicesConfigurationDataRequestPayload
  #region SetDiskAnalysisServicesConfigurationDataResponsePayload

  public class SetConfigurationDataResponsePayload
  {
    public SetConfigurationDataResponsePayload() : this(string.Empty) { }
    public SetConfigurationDataResponsePayload(string result) { Result = result; }

    public string Result { get; set; }
  }

  #endregion SetConfigurationDataResponsePayload
  #endregion SetConfigurationDataRequestPayload and SetConfigurationDataResponsePayload

  #region GetConfigurationDataRequestPayload and GetConfigurationDataResponsePayload
  #region GetConfigurationDataRequestPayload
  public class GetConfigurationDataRequestPayload
  {
    public GetConfigurationDataRequestPayload() : this(string.Empty) { }
    public GetConfigurationDataRequestPayload(string placeholder) { Placeholder = placeholder; }

    public string Placeholder { get; set; }
  }
  #endregion GetConfigurationDataRequestPayload

  #region GetConfigurationDataResponsePayload
  public class GetConfigurationDataResponsePayload
  {
    public GetConfigurationDataResponsePayload() : this(new ConfigurationData())
    { }
    public GetConfigurationDataResponsePayload(ConfigurationData configurationData)
    { ConfigurationData = configurationData; }

    public ConfigurationData ConfigurationData { get; set; }
  }

  #endregion GetConfigurationDataResponsePayload
  #endregion GetConfigurationDataRequestPayload and GetConfigurationDataResponsePayload

  #region SetUserDataRequestPayload and SetUserDataResponsePayload

  #region SetUserDataRequestPayload
  public class SetUserDataRequestPayload
  {
    public SetUserDataRequestPayload() : this(new UserData(), false) { }

    public SetUserDataRequestPayload(UserData userData, bool userDataSave)
    {
      UserData = userData;
      UserDataSave = userDataSave;
    }

    public UserData UserData { get; set; }

    public bool UserDataSave { get; set; }
  }
  #endregion SetUserDataRequestPayload

  #region SetUserDataResponsePayload
  public class SetUserDataResponsePayload
  {
    public SetUserDataResponsePayload() : this(string.Empty) { }
    public SetUserDataResponsePayload(string result) { Result = result; }

    public string Result { get; set; }
  }

  #endregion SetUserDataResponsePayload

  #endregion SetUserDataRequestPayload and SetUserDataResponsePayload

  #region GetUserDataRequestPayload and GetUserDataResponsePayload
  #region GetUserDataRequestPayload
  public class GetUserDataRequestPayload
  {
    public GetUserDataRequestPayload() : this(string.Empty) { }
    public GetUserDataRequestPayload(string placeholder) { Placeholder = placeholder; }

    public string Placeholder { get; set; }
  }
  #endregion GetUserDataRequestPayload

  #region GetUserDataResponsePayload
  public class GetUserDataResponsePayload
  {
    public GetUserDataResponsePayload() : this(new UserData()) { }
    public GetUserDataResponsePayload(UserData userData)
    { this.UserData = userData; }

    public UserData UserData { get; set; }
  }
  #endregion GetUserDataResponsePayload
  #endregion GetUserDataRequestPayload and GetUserDataResponsePayload

  #region InitializationRequestPayload and InitializationResponsePayload
  public class InitializationRequestPayload
  {
    public InitializationRequestPayload() : this(new InitializationData())
    { }
    public InitializationRequestPayload(InitializationData initializationData)
    { InitializationData = initializationData; }

    public InitializationData InitializationData { get; set; }
  }

  public class InitializationResponsePayload
  {
    public InitializationResponsePayload() : this(new ConfigurationData(),
                                                                       new UserData())
    { }
    public InitializationResponsePayload(ConfigurationData configurationData, UserData userData)
    {
      ConfigurationData = configurationData;
      UserData = userData;
    }

    public ConfigurationData ConfigurationData { get; set; }

    public UserData UserData { get; set; }
  }
  #endregion InitializationRequestPayload and InitializationResponsePayload

  #region ReadDiskRequestPayload and ReadDiskResponsePayload
  public class ReadDiskRequestPayload
  {
    public ReadDiskRequestPayload() : this(new ReadDiskRequestData()) { }
    public ReadDiskRequestPayload(ReadDiskRequestData readDiskRequestData)
    {
      ReadDiskRequestData = readDiskRequestData;
    }

    public ReadDiskRequestData ReadDiskRequestData { get; set; }
  }

  public class ReadDiskResponsePayload
  {
    public ReadDiskResponsePayload() : this(new ReadDiskResponseData()) { }
    public ReadDiskResponsePayload(ReadDiskResponseData readDiskResponseData)
    { ReadDiskResponseData = readDiskResponseData; }

    public ReadDiskResponseData ReadDiskResponseData { get; set; }
  }

  #endregion ReadDiskRequestPayload and ReadDiskResponsePayload


}
