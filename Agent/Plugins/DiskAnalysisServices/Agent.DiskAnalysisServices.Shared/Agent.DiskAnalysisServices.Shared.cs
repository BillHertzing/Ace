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

  #region DiskAnalysisServicesInitializationData
  public class DiskAnalysisServicesInitializationData
  {
    public DiskAnalysisServicesInitializationData() : this(string.Empty) { }

    public DiskAnalysisServicesInitializationData(string placeholder)
    { Placeholder = placeholder; }

    public string Placeholder { get; set; }
  }
  #endregion DiskAnalysisServicesInitializationData

  #region ReadDiskRequestData and ReadDiskResponseData
  public class ReadDiskRequestData
  {
    public ReadDiskRequestData() { }
    public ReadDiskRequestData(string placeholder)
    {
      Placeholder = placeholder;
    }

    public string Placeholder { get; set; }
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
  #endregion ReadDiskRequestData and ReadDiskResponseData

  #region SetDiskAnalysisServicesConfigurationDataRequestPayload and SetDiskAnalysisServicesConfigurationDataResponsePayload
  #region SetDiskAnalysisServicesConfigurationDataRequestPayload
  public class SetDiskAnalysisServicesConfigurationDataRequestPayload
  {
    public SetDiskAnalysisServicesConfigurationDataRequestPayload() : this(new DiskAnalysisServicesConfigurationData(),
                                                                            false)
    { }
    public SetDiskAnalysisServicesConfigurationDataRequestPayload(DiskAnalysisServicesConfigurationData diskAnalysisServicesConfigurationData, bool saveConfigurationData)
    {
      DiskAnalysisServicesConfigurationData = diskAnalysisServicesConfigurationData;
      SaveConfigurationData = saveConfigurationData;
    }

    public DiskAnalysisServicesConfigurationData DiskAnalysisServicesConfigurationData { get; set; }

    public bool SaveConfigurationData { get; set; }
  }
  #endregion SetDiskAnalysisServicesConfigurationDataRequestPayload
  #region SetDiskAnalysisServicesConfigurationDataResponsePayload

  public class SetDiskAnalysisServicesConfigurationDataResponsePayload
  {
    public SetDiskAnalysisServicesConfigurationDataResponsePayload() : this(string.Empty) { }
    public SetDiskAnalysisServicesConfigurationDataResponsePayload(string result) { Result = result; }

    public string Result { get; set; }
  }

  #endregion SetDiskAnalysisServicesConfigurationDataResponsePayload
#endregion SetDiskAnalysisServicesConfigurationDataRequestPayload and SetDiskAnalysisServicesConfigurationDataResponsePayload

  #region GetDiskAnalysisServicesConfigurationDataRequestPayload and GetDiskAnalysisServicesConfigurationDataResponsePayload
  #region GetDiskAnalysisServicesConfigurationDataRequestPayload
  public class GetDiskAnalysisServicesConfigurationDataRequestPayload
  {
    public GetDiskAnalysisServicesConfigurationDataRequestPayload() : this(string.Empty) { }
    public GetDiskAnalysisServicesConfigurationDataRequestPayload(string placeholder) { Placeholder = placeholder; }

    public string Placeholder { get; set; }
  }
  #endregion GetDiskAnalysisServicesConfigurationDataRequestPayload

  #region GetDiskAnalysisServicesConfigurationDataResponsePayload

  public class GetDiskAnalysisServicesConfigurationDataResponsePayload
  {
    public GetDiskAnalysisServicesConfigurationDataResponsePayload() : this(new DiskAnalysisServicesConfigurationData())
    { }
    public GetDiskAnalysisServicesConfigurationDataResponsePayload(DiskAnalysisServicesConfigurationData diskAnalysisServicesConfigurationData)
    { DiskAnalysisServicesConfigurationData = diskAnalysisServicesConfigurationData; }

    public DiskAnalysisServicesConfigurationData DiskAnalysisServicesConfigurationData { get; set; }
  }

  #endregion GetDiskAnalysisServicesConfigurationDataResponsePayload
  #endregion GetDiskAnalysisServicesConfigurationDataRequestPayload and GetDiskAnalysisServicesConfigurationDataResponsePayload

  #region SetDiskAnalysisServicesUserDataRequestPayload and SetDiskAnalysisServicesUserDataResponsePayload

  #region SetDiskAnalysisServicesUserDataRequestPayload
  public class SetDiskAnalysisServicesUserDataRequestPayload
  {
    public SetDiskAnalysisServicesUserDataRequestPayload() : this(new DiskAnalysisServicesUserData(), false) { }

    public SetDiskAnalysisServicesUserDataRequestPayload(DiskAnalysisServicesUserData diskAnalysisServicesUserData, bool userDataSave)
    {
      DiskAnalysisServicesUserData = diskAnalysisServicesUserData;
      UserDataSave = userDataSave;
    }

    public DiskAnalysisServicesUserData DiskAnalysisServicesUserData { get; set; }

    public bool UserDataSave { get; set; }
  }
  #endregion SetDiskAnalysisServicesUserDataRequestPayload

  #region SetDiskAnalysisServicesUserDataResponsePayload
  public class SetDiskAnalysisServicesUserDataResponsePayload
  {
    public string Result { get; set; }
  }

  #endregion SetDiskAnalysisServicesUserDataResponsePayload

  #endregion SetDiskAnalysisServicesUserDataRequestPayload and SetDiskAnalysisServicesUserDataResponsePayload

  #region GetDiskAnalysisServicesUserDataRequestPayload and GetDiskAnalysisServicesUserDataResponsePayload
  #region GetDiskAnalysisServicesUserDataRequestPayload
  public class GetDiskAnalysisServicesUserDataRequestPayload
  {
    public GetDiskAnalysisServicesUserDataRequestPayload() : this(string.Empty) { }
    public GetDiskAnalysisServicesUserDataRequestPayload(string placeholder) { Placeholder = placeholder; }

    public string Placeholder { get; set; }
  }
  #endregion GetDiskAnalysisServicesUserDataRequestPayload

  #region GetDiskAnalysisServicesUserDataResponsePayload
  public class GetDiskAnalysisServicesUserDataResponsePayload
  {
    public GetDiskAnalysisServicesUserDataResponsePayload() : this(new DiskAnalysisServicesUserData()) { }
    public GetDiskAnalysisServicesUserDataResponsePayload(DiskAnalysisServicesUserData DiskAnalysisServicesUserData)
    { this.DiskAnalysisServicesUserData = DiskAnalysisServicesUserData; }

    public DiskAnalysisServicesUserData DiskAnalysisServicesUserData { get; set; }
  }
  #endregion GetDiskAnalysisServicesUserDataResponsePayload
  #endregion GetDiskAnalysisServicesUserDataRequestPayload and GetDiskAnalysisServicesUserDataResponsePayload

  #region DiskAnalysisServicesInitializationRequestPayload and DiskAnalysisServicesInitializationResponsePayload
  public class DiskAnalysisServicesInitializationRequestPayload
  {
    public DiskAnalysisServicesInitializationRequestPayload() : this(new DiskAnalysisServicesInitializationData())
    { }
    public DiskAnalysisServicesInitializationRequestPayload(DiskAnalysisServicesInitializationData diskAnalysisServicesInitializationData)
    { DiskAnalysisServicesInitializationData = diskAnalysisServicesInitializationData; }

    public DiskAnalysisServicesInitializationData DiskAnalysisServicesInitializationData { get; set; }
  }

  public class DiskAnalysisServicesInitializationResponsePayload
  {
    public DiskAnalysisServicesInitializationResponsePayload() : this(new DiskAnalysisServicesConfigurationData(),
                                                                       new DiskAnalysisServicesUserData())
    { }
    public DiskAnalysisServicesInitializationResponsePayload(DiskAnalysisServicesConfigurationData diskAnalysisServicesConfigurationData, DiskAnalysisServicesUserData diskAnalysisServicesUserData)
    {
      DiskAnalysisServicesConfigurationData = diskAnalysisServicesConfigurationData;
      DiskAnalysisServicesUserData = diskAnalysisServicesUserData;
    }

    public DiskAnalysisServicesConfigurationData DiskAnalysisServicesConfigurationData { get; set; }

    public DiskAnalysisServicesUserData DiskAnalysisServicesUserData { get; set; }
  }

  #endregion DiskAnalysisServicesInitializationRequestPayload and DiskAnalysisServicesInitializationResponsePayload

  #region ReadDiskRequestPayload and ReadDiskResponsePayload
  public class ReadDiskRequestPayload
  {
    public ReadDiskRequestPayload() { }
    public ReadDiskRequestPayload(ReadDiskRequestData readDiskRequestData)
    {
      ReadDiskRequestData = readDiskRequestData;
    }

    public ReadDiskRequestData ReadDiskRequestData { get; set; }
  }

  public class ReadDiskResponsePayload
  {
    public ReadDiskResponsePayload() { }
    public ReadDiskResponsePayload(ReadDiskResponseData ReadDiskResponseData)
    { ReadDiskResponseData = ReadDiskResponseData; }

    public ReadDiskResponseData ReadDiskResponseData { get; set; }
  }

  #endregion ReadDiskRequestPayload and ReadDiskResponsePayload


}
