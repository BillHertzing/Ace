using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using ATAP.Utilities.FileSystem.Enumerations;
namespace ATAP.Utilities.FileSystem
{
  public static class ExceptionErrorMessages
  {
    // ToDo: eventually localize these
    public const string RootDirectoryNotFound = "Directory {root} not found";
  }

  public class DiskInfoEx
  {
    public DiskInfoEx() : this(-1, Guid.Empty, string.Empty, string.Empty, new List<PartitionInfoEx>(), new List<Exception>()) { }
    public DiskInfoEx(long diskIdentityId, Guid diskGuid, string mfgr, string serialNumber, List<PartitionInfoEx> partitionInfoExs, List<Exception> exceptions)
    {
      DiskIdentityId = diskIdentityId;
      DiskGuid = diskGuid;
      Mfgr = mfgr;
      SerialNumber = serialNumber;
      Exceptions = exceptions;
      PartitionInfoExs = partitionInfoExs;
    }
    public long DiskIdentityId;
    public Guid DiskGuid;
    public string Mfgr { get; set; }
    public string SerialNumber { get; set; }
    public List<PartitionInfoEx> PartitionInfoExs;
    public List<Exception> Exceptions;
  }

  public class PartitionInfoEx
  {
    public PartitionInfoEx() : this(-1, Guid.Empty, new List<string>(), new List<Exception>()) { }
    public PartitionInfoEx(long partitionIdentityId, Guid partitionGuid, List<string> driveLetters, List<Exception> exceptions)
    {
      PartitionIdentityId = partitionIdentityId;
      PartitionGuid = partitionGuid;
      DriveLetters = driveLetters;
      Exceptions = exceptions;
    }
    public long PartitionIdentityId;
    public Guid PartitionGuid;
    public List<string> DriveLetters { get; set; }
    public List<Exception> Exceptions;
  }

  public class DirectoryInfoEx
  {
    public DirectoryInfoEx() : this(-1, Guid.Empty, new DirectoryInfo(string.Empty), new List<Exception>()) { }
    public DirectoryInfoEx(long directoryInfoId, Guid directoryInfoGuid, DirectoryInfo directoryInfo, List<Exception> exceptions)
    {
      DirectoryInfoId = directoryInfoId;
      DirectoryInfoGuid = directoryInfoGuid;
      DirectoryInfo = directoryInfo;
      Exceptions = exceptions;
    }
    public long DirectoryInfoId;
    public Guid DirectoryInfoGuid;
    public DirectoryInfo DirectoryInfo { get; set; }
    public List<Exception> Exceptions;
  }

  public class FileInfoEx
  {
    public FileInfoEx() : this(-1, Guid.Empty, new FileInfo(string.Empty), string.Empty, new List<Exception>()) { }
    public FileInfoEx(long fileInfoId, Guid fileInfoGuid, FileInfo fileInfo, string hash, List<Exception> exceptions)
    {
      FileInfoId = fileInfoId;
      FileInfoGuid = fileInfoGuid;
      FileInfo = fileInfo;
      Hash = hash;
      Exceptions = exceptions;
    }
    long FileInfoId { get; set; }
    Guid FileInfoGuid { get; set; }
    public FileInfo FileInfo { get; set; }
    public string Hash { get; set; }
    public List<Exception> Exceptions;
  }


  public class ReadDiskResult
  {
    public ReadDiskResult() : this(new DiskInfoEx(), new List<PartitionInfoEx>(), new List<DirectoryInfoEx>(),
      new List<FileInfoEx>(), new List<string>(), new List<string>(), new List<Exception>(), new TimeSpan())
    { }
    public ReadDiskResult(DiskInfoEx diskInfoEx, List<PartitionInfoEx> partitionInfoExs, List<DirectoryInfoEx> directoryInfoExs, List<FileInfoEx> fileInfoExs,
      List<string> nodeInserts, List<string> edgeInserts, List<Exception> exceptions, TimeSpan elapsedTime)
    {
      DiskInfoEx = diskInfoEx;
      PartitionInfoExs = partitionInfoExs;
      DirectoryInfoExs = directoryInfoExs;
      FileInfoExs = fileInfoExs;
      NodeInserts = nodeInserts;
      EdgeInserts = edgeInserts;
      Exceptions = exceptions;
      ElapsedTime = elapsedTime;
    }
    public DiskInfoEx DiskInfoEx { get; set; }
    public List<PartitionInfoEx> PartitionInfoExs { get; set; }
    public List<DirectoryInfoEx> DirectoryInfoExs { get; set; }
    public List<FileInfoEx> FileInfoExs { get; set; }
    public List<string> NodeInserts { get; set; }
    public List<string> EdgeInserts { get; set; }
    public List<Exception> Exceptions { get; set; }
    public TimeSpan ElapsedTime { get; set; }
  }

  public class AnalyzeDisk
  {
    public AnalyzeDisk() { }

    public DiskInfoEx GetDiskInfoEx(string cRUD)
    {
      DiskInfoEx diskInfoEx = new DiskInfoEx();
      diskInfoEx.Mfgr = "DummyDiskMfrg"; // ToDo: read disk mfgr via WMI
      diskInfoEx.SerialNumber = "DummyDiskSerialNumber"; // ToDo: read disk serial number via WMI
      // Todo: see if the Mfgr and SerialNumber already exist in the DB
      if (false)
      {
        // already exist in DB, get ID and GUID from DB
        diskInfoEx.DiskIdentityId = 0; //ToDo: repalce with SQL fetch
        diskInfoEx.DiskGuid = new Guid(); //ToDo: repalce with SQL fetch
        diskInfoEx.PartitionInfoExs = new List<PartitionInfoEx>(); //ToDo: repalce with SQL fetch
      }
      else
      {
        diskInfoEx.DiskIdentityId = 0;
        diskInfoEx.DiskGuid = new Guid();
      }
      return diskInfoEx;
    }
    public List<PartitionInfoEx> GetPartitionInfoExs(string cRUD, DiskInfoEx diskInfoEx)
    {
      List<PartitionInfoEx> partitionInfoExs = new List<PartitionInfoEx>();
      PartitionInfoEx partitionInfoEx = new PartitionInfoEx();
      // ToDo: Get the list of partitions from the Disk hardware
      var hwPartitions = new List<PartitionInfoEx>();
      // Until real partitiosn are available, assume one partitons, drive letter E
      hwPartitions.Add(new PartitionInfoEx() { DriveLetters = new List<string>() { "E" } });
      // ToDo: see if the disk already has paartitions in the DB
      var dBPartitions = new List<PartitionInfoEx>();
      // ToDo: depending on cRUD, do diffente things with the list
      // if cRUD is Create
      // make a partition list for every partition on the disk hw
      // ToDo: startig with the assumption there is only one partiton, and only one drive associated E:
      foreach (var p in hwPartitions)
      {
        partitionInfoEx.PartitionIdentityId = 0;
        partitionInfoEx.PartitionGuid = new Guid();
        partitionInfoEx.DriveLetters = p.DriveLetters;
        //ToDo: If cRUD is replace, update or delete
        // Todo: see if the Mfgr and SerialNumber already exist in the DB
        //if (false)
        //{
        //  // already exist in DB, get ID and GUID from DB
        //  diskInfoEx.DiskIdentityId = 0; //ToDo: repalce with SQL fetch
        //  diskInfoEx.DiskGuid = new Guid(); //ToDo: repalce with SQL fetch
        //  diskInfoEx.PartitionInfoExs = new List<PartitionInfoEx>(); //ToDo: repalce with SQL fetch
        //}
        //else
        //{
        //  diskInfoEx.DiskIdentityId = 0;
        //  diskInfoEx.DiskGuid = new Guid();
        //}
        partitionInfoExs.Add(partitionInfoEx);
      }
      return partitionInfoExs;
    }
    public ReadDiskResult ReadDisk(string cRUD)
    {
      ReadDiskResult readDiskResult = new ReadDiskResult();
      readDiskResult.DiskInfoEx = GetDiskInfoEx(cRUD);
      readDiskResult.PartitionInfoExs = GetPartitionInfoExs(cRUD, readDiskResult.DiskInfoEx);
      return readDiskResult;
    }
  }

}
// The ideas and code for ByteArrayHasher came from
// The ideas and some code for an ansync version of "read file and hash it" came from https://stackoverflow.com/questions/49858310/how-to-async-md5-calculate-c-sharp
// The ideas and some code for a parallel version to process files came from https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-iterate-file-directories-with-the-parallel-class


/* public class ByteArrayHasher
{
  public ByteArrayHasher(HashAlgorithm hashAlgorithm)
  {
    HashAlgorithm = hashAlgorithm;
    System.Security.Cryptography.MD5Cng hasherMD5;
    //Force.Crc32.Crc32Algorithm hasherCRC32;
    switch (hashAlgorithm)
    {
      case HashAlgorithm.CRC32:
      {
        //Force.Crc32.Crc32Algorithm hasherCRC32 = new Force.Crc32.Crc32Algorithm();
        throw new NotImplementedException();
        break;
      }
      case HashAlgorithm.MD5:
      {
        hasherMD5 = new System.Security.Cryptography.MD5Cng();
        break;
      }
      default:
      {
        throw new ArgumentException();
      }
    }
    // Create the HashFunction that can be called to incrementally 
    Func < HashFunction = new Func<byte[], string>(ba)    {
    string hashresult;
    switch (HashAlgorithm)
    {
      case HashAlgorithm.CRC32:
      {
        //hashresult = BitConverter.ToUInt32(hasherCRC32.ComputeHash(ba), 0).ToString("X8");
        break;
      }
      case HashAlgorithm.MD5:
      {
        hashresult = Convert.ToBase64String(hasherMD5.ComputeHash(ba));
        break;
      }
      default: { throw new ArgumentException(); }
    }
    return hashresult;
  }
  public HashAlgorithm HashAlgorithm;
  public Func<byte[], string> HashFunction;
}
*/

/*
public class ParallelEnumerateDiskPartitionFileSystemEntities
{

  ParallelEnumerateDiskPartitionFileSystemEntities()
  {

  }
  public ReadDiskResult ReadDisk(string root, HashAlgorithm ha, int buffersize, CancellationToken cancellationToken)
  {

    // Create the structure that will accumulate the results (sync locked so that it can be updated in parallel) and how long it took
    ReadDiskResult readDiskResult = new ReadDiskResult() { DirectoriesCount = 0, ArchivesCount = 0, FilesCount = 0, AllExceptionsList = new List<Exception>(), NodeInserts = new List<string>() };
    var sw = Stopwatch.StartNew();

    // Create the Action that hashes a file , based on the value of ha
    //ByteArrayHasher byteArrayHasher = new ByteArrayHasher(ha);
    //Func<byte[], string> HashFunction = byteArrayHasher.HashFunction;
    try
    {
      TraverseTreeParallelForEach(root, readDiskResult, cancellationToken, async (path) =>
      {
        // Exceptions are no-ops
        FSEntityInfo fSEntityInfo = new FSEntityInfo() { Name = path };
        try
        {
          // read all bytes and generate the hash for the file
          using (var md5 = System.Security.Cryptography.MD5.Create())
          {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, buffersize, true)) // true means use IO async operations
            {
              byte[] buffer = new byte[buffersize];
              int bytesRead;
              do
              {
                bytesRead = await stream.ReadAsync(buffer, 0, buffersize);
                if (bytesRead > 0)
                {
                  md5.TransformBlock(buffer, 0, bytesRead, null, 0);
                }
              } while (bytesRead > 0);

              md5.TransformFinalBlock(buffer, 0, 0);
              fSEntityInfo.Hash = BitConverter.ToString(md5.Hash).Replace("-", "").ToUpperInvariant();
            }
          }
        }
        catch (FileNotFoundException e) { fSEntityInfo.Hash = string.Empty;fSEntityInfo.Ex = e;  }
        catch (IOException e) { fSEntityInfo.Hash = string.Empty; fSEntityInfo.Ex = e; }
        catch (UnauthorizedAccessException e) { fSEntityInfo.Hash = string.Empty; fSEntityInfo.Ex = e; }
        catch (SecurityException e) { fSEntityInfo.Hash = string.Empty; fSEntityInfo.Ex = e; }
        // return the fsEntityInfo for this particular file;
        return fSEntityInfo;
      });
    }
    catch (ArgumentException)
    {
      //Console.WriteLine(@"The directory {root} does not exist.");
    }

    readDiskResult.ElapsedTime = new TimeSpan(sw.ElapsedTicks);
    return readDiskResult;

  }

  public static ReadDiskResult TraverseTreeParallelForEach(string root, ReadDiskResult readDiskResult, CancellationToken cancellationToken, Func<string, Task<FSEntityInfo>> GetFSEntityInfo)
  {

    // Determine whether to parallelize file processing on each folder based on processor count.
    int procCount = System.Environment.ProcessorCount;

    // Data structure to hold names of subfolders to be examined for files.
    Stack<string> dirs = new Stack<string>();

    if (!Directory.Exists(root))
    {
      throw new ArgumentException(ExceptionErrorMessages.RootDirectoryNotFound);
    }
    string parentdir = "";
    string currentDir = root;
    dirs.Push(root);
    // go down all the dirs, doing every file in each dir first, then the next dir
    //  dirs are done in the order that they are returned by Directory.GetDirectories(currentDir)
    while (dirs.Count > 0)
    {
      currentDir = dirs.Pop();
      //uuid = new
      // Store current dir in node table as type directory
      readDiskResult.NodeInserts.Add($"insert into FSEntityNodes (Name='{currentDir}')");
      string[] subDirs = { };
      string[] files = { };

      try
      {
        subDirs = Directory.GetDirectories(currentDir);
        // maybe not here, but somewhere, store each subdir in node table as type directory and add an edge for each back to current dir
      }
      // Thrown if we do not have discovery permission on the directory.
      catch (UnauthorizedAccessException e)
      {
        // Store permission error on the current dir in the node table and accumulate errors to report back to GUI;
        readDiskResult.NodeInserts.Add($"update FSEntityNodes set ExceptionText = '{e.Message}' where Name='{currentDir}')");
        // Add this exception to the exceptions list
        readDiskResult.AllExceptionsList.Add(e);
        continue;
      }
      // Thrown if another process has deleted the directory after we retrieved its name.
      catch (DirectoryNotFoundException e)
      {
        // Handle an unexpected exception, store in DB and accumulate errors to report back to GUI;
        readDiskResult.NodeInserts.Add($"update FSEntityNodes set ExceptionText = '{e.Message}' where Name='{currentDir}')");
        // Add this exception to the exceptions list
        readDiskResult.AllExceptionsList.Add(e);
        continue;
      }

      try
      {
        files = Directory.GetFiles(currentDir);
        // store each filename in node table as type file or archive and add an edge for each back to currentdir
      }
      catch (UnauthorizedAccessException e)
      {
        // Store permission error on the current file or archive in the node table and accumulate errors to report back to GUI;
        readDiskResult.NodeInserts.Add($"update FSEntityNodes set ExceptionText = '{e.Message}' where Name='{currentDir}')");
        // Add this exception to the exceptions list
        readDiskResult.AllExceptionsList.Add(e);
        continue;
      }
      catch (DirectoryNotFoundException e)
      {
        // Handle an unexpected exception, store in DB and accumulate errors to report back to GUI;
        readDiskResult.NodeInserts.Add($"update FSEntityNodes set ExceptionText = '{e.Message}' where Name='{currentDir}')");
        // Add this exception to the exceptions list
        readDiskResult.AllExceptionsList.Add(e);
        continue;
      }
      catch (IOException e)
      {
        // Handle an unexpected exception, store in DB and accumulate errors to report back to GUI;;
        readDiskResult.NodeInserts.Add($"update FSEntityNodes set ExceptionText = '{e.Message}' where Name='{currentDir}')");
        // Add this exception to the exceptions list
        readDiskResult.AllExceptionsList.Add(e);
        continue;
      }

      // Execute in parallel if there are enough files in the directory.
      // Otherwise, execute sequentially.Files are opened and processed
      // synchronously but this could be modified to perform async I/O.
      try
      {
        if (files.Length < procCount)
        {
          foreach (var file in files)
          {
            FSEntityInfo fSEntityInfo = GetFSEntityInfo(file);
            // store in DB and accumulate results to report back to GUI
            // ToDo: update DB
            // insert into node( Disk/partition key, filename, filetypeenum, creation date, last modification date, length, and hash)
            // insert into edge ( Disk/partition key, currentDirID, fileID, "file belongs to dir"              
            // ToDo: increment file or archive, not just file
            // ToDo: put a Interlock around the update
            readDiskResult.FilesCount++;
          }
        }
        else
        {
          // process the files array in a parallel ForEach loop

          // see https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/how-to-iterate-file-directories-with-the-parallel-class
          Parallel.ForEach<string, ReadDiskResult>(files, () => { return new ReadDiskResult(); }, (file, loopState, lclrdr) =>
              {
                FSEntityInfo fSEntityInfo = GetFSEntityInfo(file);
                // store in DB and accumulate results to report back to GUI
                // ToDo: update DB
                // insert into node( Disk/partition key, filename, filetypeenum, creation date, last modification date, length, and hash)
                // insert into edge ( Disk/partition key, currentDirID, fileID, "file belongs to dir"              
                // ToDo: increment file or archive, not just localcount
                lclrdr.NodeInserts.Add($"testing {fSEntityInfo.Name} , {fSEntityInfo.Hash}");
                lclrdr.FilesCount++;
                return lclrdr;
              },
          (final) => { readDiskResult.NodeInserts.AddRange(lclrdr.NodeInserts); }
          );

          Parallel.ForEach(files, (rdr) => { rdr = new ReadDiskResult(); }, (file, loopState, localCount) =>
                                       {
                                         FSEntityInfo fSEntityInfo = GetFSEntityInfo(file);
                                         // store in DB and accumulate results to report back to GUI
                                         // ToDo: update DB
                                         // insert into node( Disk/partition key, filename, filetypeenum, creation date, last modification date, length, and hash)
                                         // insert into edge ( Disk/partition key, currentDirID, fileID, "file belongs to dir"              
                                         // ToDo: increment file or archive, not just localcount
                                         return 1;
                                       },
                           (hv) =>
                           {
                             // ToDo: increment file or archive, not just localcount
                             // Interlocked.Add(ref readDiskResult.FilesCount, c);
                             //Interlocked.Add(ref readDiskResult.NodeInserts.Add($"add file {hv} with hashvalue {hv}"), c);
                             Console.WriteLine($"hv.Name = {hv.Name}");
                           });

        }
      }
      catch (AggregateException ae)
      {
        // ToDo: furhter investigation, as I believe the action will record the exceptions in the exception list
        ae.Handle((ex) =>
        {
          if (ex is UnauthorizedAccessException)
          {
            // Here we just output a message and go on.
            //Console.WriteLine(ex.Message);
            return true;
          }
          // Handle other exceptions here if necessary...

          return false;
        });
      }

      // Push the subdirectories onto the stack for traversal.
      // This could also be done before handing the files.
      foreach (string str in subDirs)
        dirs.Push(str);
    }

  }
}
*/

