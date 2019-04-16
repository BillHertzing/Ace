using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
// To get access to the .Dump utility for logging
using ServiceStack.Text;
// Local feed of packages still in development is not working, so put the FileSystem types into a local package
using ATAP.Utilities.FileSystem.Enumerations;
// Local feed of packages still in development is not working, so put the ComputerInventory types into FileSystem.Enumerations
using ATAP.Utilities.ComputerInventory;
using ATAP.Utilities.ComputerInventory.Enumerations;
using ATAP.Utilities.Database.Enumerations;

// ToDo: figure out logging for the ATAP libraires, this is only temporary
using ServiceStack.Logging;

namespace ATAP.Utilities.FileSystem {
    public static class ExceptionErrorMessages {
        // ToDo: eventually localize these
        public const string RootDirectoryNotFound = "Directory {root} not found";
    }

    public class DirectoryInfoEx {
        public DirectoryInfoEx() : this(-1, Guid.Empty, new DirectoryInfo(string.Empty), new List<Exception>()) { }
        public DirectoryInfoEx(long directoryInfoId, Guid directoryInfoGuid, DirectoryInfo directoryInfo, IEnumerable<Exception> exceptions) {
            DirectoryInfoId=directoryInfoId;
            DirectoryInfoGuid=directoryInfoGuid;
            DirectoryInfo=directoryInfo;
            Exceptions=exceptions;
        }
        public long DirectoryInfoId;
        public Guid DirectoryInfoGuid;
        public DirectoryInfo DirectoryInfo { get; set; }
        public IEnumerable<Exception> Exceptions;
    }

    public class FileInfoEx {
        public FileInfoEx() : this(string.Empty, -1, Guid.Empty, new FileInfo(string.Empty), string.Empty, new List<Exception>()) { }
        public FileInfoEx(string path, long fileInfoId, Guid fileInfoGuid, IEnumerable<Exception> exceptions) {
            Path=path;
            FileInfoId=fileInfoId;
            FileInfoGuid=fileInfoGuid;
            Exceptions=exceptions;
        }
        public FileInfoEx(string path, long fileInfoId, Guid fileInfoGuid, FileInfo fileInfo, string hash, IEnumerable<Exception> exceptions) {
            //ToDo: Add validation during construction that Path and FileInfo are mutually exclusive
            Path=path;
            FileInfoId=fileInfoId;
            FileInfoGuid=fileInfoGuid;
            FileInfo=fileInfo;
            Hash=hash;
            Exceptions=exceptions;
        }
        // Path will be string.Empty if FileInfo is populated
        public string Path { get; set; }
        public long FileInfoId { get; set; }
        public Guid FileInfoGuid { get; set; }
        // FileInfo will be null if Path is not string.Empty
        public FileInfo FileInfo { get; set; }
        public string Hash { get; set; }
        public IEnumerable<Exception> Exceptions;
    }



    public class AnalyzeOneDiskDriveResult {
        public AnalyzeOneDiskDriveResult() : this(new DiskInfoEx(), new List<PartitionInfoEx>(), new List<DirectoryInfoEx>(),
          new List<FileInfoEx>(), new List<string>(), new List<string>(), new List<Exception>(), new TimeSpan()) { }
        public AnalyzeOneDiskDriveResult(DiskInfoEx diskInfoEx, IEnumerable<PartitionInfoEx> partitionInfoExs, IEnumerable<DirectoryInfoEx> directoryInfoExs, IEnumerable<FileInfoEx> fileInfoExs,
          IEnumerable<string> nodeInserts, IEnumerable<string> edgeInserts, IEnumerable<Exception> exceptions, TimeSpan elapsedTime) {
            DiskInfoEx=diskInfoEx;
            PartitionInfoExs=partitionInfoExs;
            DirectoryInfoExs=directoryInfoExs;
            FileInfoExs=fileInfoExs;
            NodeInserts=nodeInserts;
            EdgeInserts=edgeInserts;
            Exceptions=exceptions;
            ElapsedTime=elapsedTime;
        }
        public DiskInfoEx DiskInfoEx { get; set; }
        public IEnumerable<PartitionInfoEx> PartitionInfoExs { get; set; }
        public IEnumerable<DirectoryInfoEx> DirectoryInfoExs { get; set; }
        public IEnumerable<FileInfoEx> FileInfoExs { get; set; }
        public IEnumerable<string> NodeInserts { get; set; }
        public IEnumerable<string> EdgeInserts { get; set; }
        public IEnumerable<Exception> Exceptions { get; set; }
        public TimeSpan ElapsedTime { get; set; }
    }

    public class AnalyzeDisk {

        #region StringConstants
        #endregion

        // temp can't have a parameterless constructor unless evey log call gets wrapped in a null detector, ugh
        // public AnalyzeDisk() : this(4095, ) {
        //    Log.Debug("starting AnalyzeDisk ctor (parameterless)");
        //    Log.Debug("leaving AnalyzeDisk ctor (parameterless)");
        //}

        // temp: until library logging is solved; Passing in a ILog implementation from the calling class
        public AnalyzeDisk(int asyncFileReadBlocksize, ILog log) {
            Log=log;
            Log.Debug($"starting AnalyzeDisk ctor (asyncFileReadBlocksize = {asyncFileReadBlocksize})");

            AsyncFileReadBlocksize=asyncFileReadBlocksize;
            // Create the hasher
            // ToDo: Find a MD5 algorithm that is thread-safe and can be reused; the one below throws a cryptographic exception when called a second time (after transformFinalBlock)
            // ToDo: Make this into a list of hash functions that can be used on a filestream, and make it possible for any method in this class to select one from the list. Allows the user to select the hash function to be used.
            MD5=System.Security.Cryptography.MD5.Create();

            Log.Debug("leaving AnalyzeDisk ctor");
        }

        // C/R/U/D a list of DiskInfoEx with data in a DB
        // pass in an Action that will interact with the DB
        public async Task DiskInfoExsToDB(List<DiskInfoEx> diskInfoExs, CrudType cRUD,  Action<CrudType, DiskInfoEx> interact) {
            Log.Debug($"starting DiskInfoExsToDB: cRUD = {cRUD.ToString()}, diskInfoExs = {diskInfoExs.Dump()}");
            foreach (var diskInfoEx in diskInfoExs) {
                // invoke the Action delegate to C/R/U/D each DiskInfoEx to the DB
                await interact.Invoke(cRUD, diskInfoEx);
            }
            Log.Debug($"leaving DiskInfoExsToDB");
        }



        /*  Move this stuff to teh Expression for the Action that will validate the DB
        if (DBFetch==null) {
            diskInfoEx.DiskIdentityId=0;
            diskInfoEx.DiskGuid=Guid.NewGuid();
            Log.Debug($"in PopulateDiskInfoExs: awaiting PopulatePartitionInfoExs,  diskInfoEx = {diskInfoEx}");
            await PopulatePartitionInfoExs(cRUD, diskInfoEx);
            Log.Debug($"in PopulateDiskInfoExs: PopulatePartitionInfoExs has completed,  diskInfoEx = {diskInfoEx}");
        } else {
            // Todo: see if the DiskDriveMaker and SerialNumber already exist in the DB
            // async (cRUD, diskInfoEx) => { await Task.Yield(); }
            // Task< DiskInfoEx> t = await DBFetch.Invoke(cRUD, diskInfoEx);
            // diskInfoEx = await DBFetch.Invoke(cRUD, diskInfoEx);
            if (false) {
                // already exist in DB, get ID and GUID from DB
                diskInfoEx.DiskIdentityId=0; //ToDo: repalce with SQL fetch
                diskInfoEx.DiskGuid=Guid.NewGuid(); //ToDo: repalce with SQL fetch
                diskInfoEx.PartitionInfoExs=new List<PartitionInfoEx>(); //ToDo: repalce with SQL fetch
            }
        }
                //ToDo: If cRUD is replace, update or delete
                // Todo: see if the DiskDriveMaker and SerialNumber already exist in the DB
                //if (false)
                //{
                //  // already exist in DB, get ID and GUID from DB
                //  diskInfoEx.DiskDriveDBIdentityId = 0; //ToDo: repalce with SQL fetch
                //  diskInfoEx.DiskDriveGuid = Guid.NewGuid(); //ToDo: repalce with SQL fetch
                //  diskInfoEx.PartitionInfoExs = new List<PartitionInfoEx>(); //ToDo: repalce with SQL fetch
                //}
                //else
                //{
                //  diskInfoEx.DiskDriveDBIdentityId = 0;
                //  diskInfoEx.DiskDriveGuid = Guid.NewGuid();
                //}

                                // ToDo: depending on cRUD, do diffente things with the list
                    // if cRUD is Create
                    // make a partition list for every partition on the disk hw
                    // ToDo: starting with the assumption there is only one partiton, and only one drive associated E:
                    foreach (var p in hwPartitions) {
                        partitionInfoEx.PartitionIdentityId=0;
                        partitionInfoEx.PartitionGuid=Guid.NewGuid();
                        partitionInfoEx.DriveLetters=p.DriveLetters;
                        partitionInfoExs.Add(partitionInfoEx);
                    }
        */

        public async Task<AnalyzeOneDiskDriveResult> GetFSEntities(string root, Action<string> recordRoot = null) {
            Log.Debug($"starting GetFSEntities: root = {root}");

            if (!Directory.Exists(root)) {
                throw new ArgumentException(ExceptionErrorMessages.RootDirectoryNotFound);
            }

            // Record the root directory into the DB
            if (recordRoot!=null)
                recordRoot.Invoke(root);

            AnalyzeOneDiskDriveResult readDiskResult = new AnalyzeOneDiskDriveResult();
            // Data structure to hold names of subfolders to be examined for files.
            Stack<string> dirs = new Stack<string>();

            string currentDir = root;
            dirs.Push(root);
            // go down all the dirs, doing every file in each dir first, then the next dir
            //  dirs are done in the order that they are returned by Directory.GetDirectories(currentDir)
            while (dirs.Count>0) {
                currentDir=dirs.Pop();
                // Store current dir in node table as type directory
                readDiskResult.NodeInserts.Add($"insert into FSEntityNodes (Name='{currentDir}')");
                string[] subDirs = { };
                string[] files = { };

                try {
                    subDirs=Directory.GetDirectories(currentDir);
                    // maybe not here, but somewhere, store each subdir in node table as type directory and add an edge for each back to current dir
                }
                // Thrown if we do not have discovery permission on the directory.
                catch (UnauthorizedAccessException e) {
                    // Store permission error on the current dir in the node table and accumulate errors to report back to GUI;
                    readDiskResult.NodeInserts.Add($"update FSEntityNodes set ExceptionText = '{e.Message}' where Name='{currentDir}')");
                    // Add this exception to the exceptions list
                    readDiskResult.Exceptions.Add(e);
                    continue;

                }
                // Thrown if another process has deleted the directory after we retrieved its name.
                catch (DirectoryNotFoundException e) {
                    // Handle an unexpected exception, store in DB and accumulate errors to report back to GUI;
                    readDiskResult.NodeInserts.Add($"update FSEntityNodes set ExceptionText = '{e.Message}' where Name='{currentDir}')");
                    // Add this exception to the exceptions list
                    readDiskResult.Exceptions.Add(e);
                    continue;
                }

                try {
                    files=Directory.GetFiles(currentDir);
                }
                catch (UnauthorizedAccessException e) {
                    // Store permission error on the current file or archive in the node table and accumulate errors to report back to GUI;
                    readDiskResult.NodeInserts.Add($"update FSEntityNodes set ExceptionText = '{e.Message}' where Name='{currentDir}')");
                    // Add this exception to the exceptions list
                    readDiskResult.Exceptions.Add(e);
                    continue;
                }
                catch (DirectoryNotFoundException e) {
                    // Handle an unexpected exception, store in DB and accumulate errors to report back to GUI;
                    readDiskResult.NodeInserts.Add($"update FSEntityNodes set ExceptionText = '{e.Message}' where Name='{currentDir}')");
                    // Add this exception to the exceptions list
                    readDiskResult.Exceptions.Add(e);
                    continue;
                }
                catch (IOException e) {
                    // Handle an unexpected exception, store in DB and accumulate errors to report back to GUI;;
                    readDiskResult.NodeInserts.Add($"update FSEntityNodes set ExceptionText = '{e.Message}' where Name='{currentDir}')");
                    // Add this exception to the exceptions list
                    readDiskResult.Exceptions.Add(e);
                    continue;
                }

                // Hash Files here. Create as many tasks as there are files
                List<Task<FileInfoEx>> taskList = new List<Task<FileInfoEx>>();
                foreach (var f in files) {
                    taskList.Add(GetFileInfoEx(f, AsyncFileReadBlocksize));
                }
                // wait for all to finish
                await Task.WhenAll(taskList);

                foreach (var fileInfoEx in taskList) {
                    // Aggregate the information about each file into the readDiskResult results
                    readDiskResult.NodeInserts.Add($"insert into Node {fileInfoEx.Result.FileInfo.Name}");
                }

                // Insert the Node and Edge information about all files into the DB

                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (string str in subDirs) {
                    // Get DirectoryInfo for each directory
                    // ToDo: only push if there is no exception
                    // Insert the Node and Edge information about all directories into the DB
                    // update the DirectoryInfoEx in subDirs with the nodeID for each subdir as returned by the insert
                    dirs.Push(str);
                }
            }
            return readDiskResult;
        }

        public async Task<FileInfoEx> GetFileInfoEx(string path, int blocksize) {
            FileInfoEx fileInfoEx = new FileInfoEx(path, -1, Guid.Empty, new List<Exception>());
            try {
                // read all bytes and generate the hash for the file
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, blocksize, true)) // true means use IO async operations
                {
                    // ToDo: move the instance of the md5 hasher out of the task, but ensure the implementation of the instance is thread-safeand can be reused
                    // ToDo: The MD5 hasher found in the AnalyzeDisk properties cannot be reused after the call to TransformFinalBlock
                    using (var md5 = System.Security.Cryptography.MD5.Create()) {
                        byte[] buffer = new byte[blocksize];
                        int bytesRead;
                        do {
                            bytesRead=await stream.ReadAsync(buffer, 0, blocksize);
                            if (bytesRead>0) {
                                md5.TransformBlock(buffer, 0, bytesRead, null, 0);
                            }
                        } while (bytesRead>0);

                        md5.TransformFinalBlock(buffer, 0, 0);
                        fileInfoEx.Hash=BitConverter.ToString(md5.Hash).Replace("-", string.Empty).ToUpperInvariant();
                    }
                    fileInfoEx.FileInfo=new FileInfo(path);
                    fileInfoEx.Path=string.Empty;
                }
            }
            catch (FileNotFoundException e) { fileInfoEx.Hash=string.Empty; fileInfoEx.Exceptions.Add(e); }
            catch (IOException e) { fileInfoEx.Hash=string.Empty; fileInfoEx.Exceptions.Add(e); }
            catch (UnauthorizedAccessException e) { fileInfoEx.Hash=string.Empty; fileInfoEx.Exceptions.Add(e); }
            catch (SecurityException e) { fileInfoEx.Hash=string.Empty; fileInfoEx.Exceptions.Add(e); }
            // return the fileInfoEx for this particular file;
            return fileInfoEx;
        }

        #region Properties
        #region Properties:class logger
        public ILog Log;
        #endregion
        #region Properties:AsyncFileReadBlocksize
        int AsyncFileReadBlocksize;
        #endregion
        #region Properties:Hasher MD5
        System.Security.Cryptography.MD5 MD5 { get; set; }
        #endregion
        #endregion

        #region Disposable
        // ToDo: Dispose of any hashers that were created

        #endregion
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

