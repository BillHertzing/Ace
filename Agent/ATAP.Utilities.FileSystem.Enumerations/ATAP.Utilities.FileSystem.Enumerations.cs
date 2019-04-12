using System.ComponentModel;

namespace ATAP.Utilities.FileSystem.Enumerations
{
  public enum HashAlgorithm
  {
    //ToDo: Add [LocalizedDescription("CRC32", typeof(Resource))]
    [Description("CRC32")]
    CRC32,
    [Description("MD5")]
    MD5
  }
}
