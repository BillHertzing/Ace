using System;
using System.Collections.Generic;

namespace Ace.Agent.DiskAnalysisServices
{
  #region DiskAnalysisServicesConfigurationData
  public class DiskAnalysisServicesConfigurationData
  {
    public DiskAnalysisServicesConfigurationData() : this(string.Empty) { }

    public DiskAnalysisServicesConfigurationData(string placeholder)
    {
      Placeholder = placeholder;
    }

    public string Placeholder { get; set; }
  }
  #endregion DiskAnalysisServicesConfigurationData

  #region DiskAnalysisServicesUserData
  public class DiskAnalysisServicesUserData
  {
    public DiskAnalysisServicesUserData() : this(string.Empty) { }
    public DiskAnalysisServicesUserData(string placeholder)
    {
      Placeholder = placeholder;
    }

    public string Placeholder { get; set; }
  }
  #endregion DiskAnalysisServicesUserData

  #region DiskAnalysisPayload
  public class ReadDiskRequestData
  {
    public ReadDiskRequestData() { }
    public ReadDiskRequestData(ReadDiskParameters readDiskParameters)
    {
      ReadDiskParameters = readDiskParameters;
    }

    public ReadDiskParameters ReadDiskParameters { get; set; }
  }

  public class ReadDiskParameters
  {
    public ReadDiskParameters() { }
    public ReadDiskParameters(string placeholder)
    {
      Placeholder = placeholder;
    }

    string Placeholder { get; set; }
  }

  public class ReadDiskResponseData
  {
    public ReadDiskResponseData() { }
    public ReadDiskResponseData(string result)
    {
      Result = result;
    }

    string Result { get; set; }
  }
  #endregion DiskAnalysisServicesReadDiskData

  #region DiskAnalysisServicesSetConfigurationData
  public class SetDiskAnalysisServicesConfigurationDataRequestData
  {
    public SetDiskAnalysisServicesConfigurationDataRequestData() : this(new DiskAnalysisServicesConfigurationData(),
                                                                            false)
    { }
    public SetDiskAnalysisServicesConfigurationDataRequestData(DiskAnalysisServicesConfigurationData diskAnalysisServicesConfigurationData, bool saveConfigurationData)
    {
      DiskAnalysisServicesConfigurationData = diskAnalysisServicesConfigurationData;
      SaveConfigurationData = saveConfigurationData;
    }

    public DiskAnalysisServicesConfigurationData DiskAnalysisServicesConfigurationData { get; set; }

    public bool SaveConfigurationData { get; set; }
  }

  public class SetDiskAnalysisServicesConfigurationDataResponse
  {
    public SetDiskAnalysisServicesConfigurationDataResponse() : this(string.Empty) { }
    public SetDiskAnalysisServicesConfigurationDataResponse(string result) { Result = result; }

    public string Result { get; set; }
  }

  #endregion DiskAnalysisServicesSetConfigurationData

  #region DiskAnalysisServicesGetConfigurationData
  public class GetDiskAnalysisServicesConfigurationDataRequestData
  {
    public GetDiskAnalysisServicesConfigurationDataRequestData() : this(string.Empty) { }
    public GetDiskAnalysisServicesConfigurationDataRequestData(string placeholder) { Placeholder = placeholder; }

    public string Placeholder { get; set; }
  }

  public class GetDiskAnalysisServicesConfigurationDataResponse
  {
    public GetDiskAnalysisServicesConfigurationDataResponse() : this(new DiskAnalysisServicesConfigurationData())
    { }
    public GetDiskAnalysisServicesConfigurationDataResponse(DiskAnalysisServicesConfigurationData DiskAnalysisServicesConfigurationData)
    { DiskAnalysisServicesConfigurationData = DiskAnalysisServicesConfigurationData; }

    public DiskAnalysisServicesConfigurationData DiskAnalysisServicesConfigurationData { get; set; }
  }

  #endregion DiskAnalysisServicesGetConfigurationData

  #region DiskAnalysisServicesSetUserData
  public class SetDiskAnalysisServicesUserDataRequestData
  {
    public SetDiskAnalysisServicesUserDataRequestData() : this(new DiskAnalysisServicesUserData(), false) { }

    public SetDiskAnalysisServicesUserDataRequestData(DiskAnalysisServicesUserData DiskAnalysisServicesUserData, bool userDataSave)
    {
      DiskAnalysisServicesUserData = DiskAnalysisServicesUserData;
      UserDataSave = userDataSave;
    }

    public DiskAnalysisServicesUserData DiskAnalysisServicesUserData { get; set; }

    public bool UserDataSave { get; set; }
  }

  public class SetDiskAnalysisServicesUserDataResponse
  {
    public string Result { get; set; }
  }

  #endregion DiskAnalysisServicesSetUserData

  #region DiskAnalysisServicesGetUserData
  public class GetDiskAnalysisServicesUserDataRequestData
  {
    public GetDiskAnalysisServicesUserDataRequestData() : this(string.Empty) { }
    public GetDiskAnalysisServicesUserDataRequestData(string placeholder) { Placeholder = placeholder; }

    public string Placeholder { get; set; }
  }

  public class GetDiskAnalysisServicesUserDataResponse
  {
    public GetDiskAnalysisServicesUserDataResponse() : this(new DiskAnalysisServicesUserData()) { }
    public GetDiskAnalysisServicesUserDataResponse(DiskAnalysisServicesUserData DiskAnalysisServicesUserData)
    { this.DiskAnalysisServicesUserData = DiskAnalysisServicesUserData; }

    public DiskAnalysisServicesUserData DiskAnalysisServicesUserData { get; set; }
  }

  #endregion DiskAnalysisServicesGetUserData

  #region DiskAnalysisServicesReadDisk
  public class ReadDiskRequestPayload
  {
    public ReadDiskRequestPayload() { }
    public ReadDiskRequestPayload(ReadDiskRequestData ReadDiskRequestData, bool saveReadDiskData)
    {
      ReadDiskRequestData = ReadDiskRequestData;
      SaveReadDiskData = saveReadDiskData;
    }

    public ReadDiskRequestData ReadDiskRequestData { get; set; }

    public bool SaveReadDiskData { get; set; }
  }

  public class ReadDiskResponsePayload
  {
    public ReadDiskResponsePayload() { }
    public ReadDiskResponsePayload(ReadDiskResponseData ReadDiskResponseData)
    { ReadDiskResponseData = ReadDiskResponseData; }

    public ReadDiskResponseData ReadDiskResponseData { get; set; }
  }

  #endregion DiskAnalysisServicesReadDisk

  #region DiskAnalysisServicesDoInitialization
  public class DiskAnalysisServicesInitializationData
  {
    public DiskAnalysisServicesInitializationData() : this(string.Empty) { }

    public DiskAnalysisServicesInitializationData(string placeholder)
    { Placeholder = placeholder; }

    public string Placeholder { get; set; }
  }

  public class DiskAnalysisServicesInitializationDataRequestData
  {
    public DiskAnalysisServicesInitializationDataRequestData() : this(new DiskAnalysisServicesInitializationData())
    { }
    public DiskAnalysisServicesInitializationDataRequestData(DiskAnalysisServicesInitializationData DiskAnalysisServicesInitializationData)
    { DiskAnalysisServicesInitializationData = DiskAnalysisServicesInitializationData; }

    public DiskAnalysisServicesInitializationData DiskAnalysisServicesInitializationData { get; set; }
  }

  public class DiskAnalysisServicesInitializationResponseData
  {
    public DiskAnalysisServicesInitializationResponseData() : this(new DiskAnalysisServicesConfigurationData(),
                                                                       new DiskAnalysisServicesUserData())
    { }
    public DiskAnalysisServicesInitializationResponseData(DiskAnalysisServicesConfigurationData DiskAnalysisServicesConfigurationData, DiskAnalysisServicesUserData DiskAnalysisServicesUserData)
    {
      DiskAnalysisServicesConfigurationData = DiskAnalysisServicesConfigurationData;
      DiskAnalysisServicesUserData = DiskAnalysisServicesUserData;
    }

    public DiskAnalysisServicesConfigurationData DiskAnalysisServicesConfigurationData { get; set; }

    public DiskAnalysisServicesUserData DiskAnalysisServicesUserData { get; set; }
  }
  #endregion DiskAnalysisServicesDoInitialization

}
