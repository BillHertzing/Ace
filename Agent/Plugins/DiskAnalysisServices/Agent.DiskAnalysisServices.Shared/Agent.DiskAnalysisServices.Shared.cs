using System;
using System.Collections.Generic;


namespace Ace.Agent.DiskAnalysisServices
{
  #region ConfigurationData
  public class ConfigurationData
  {
    public ConfigurationData() : this(4095) { }

    public ConfigurationData(int blockSize)
    {
            BlockSize=blockSize;
    }

    public int BlockSize { get; set; }
  }
  #endregion

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
    public InitializationData() : this(new ConfigurationData(), new UserData()) { }

        public InitializationData(ConfigurationData configurationData, UserData userData) {
            ConfigurationData=configurationData;
            UserData=userData;
        }

        public ConfigurationData ConfigurationData { get; set; }
        public UserData UserData { get; set; }
    }
    #endregion InitializationData


    /*
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
    */

}
