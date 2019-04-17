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
using ATAP.Utilities.Filesystem;
using System.Linq;
using System.Security.Cryptography;

namespace ATAP.Utilities.FileSystem {
    public static class ExceptionErrorMessages {
        // ToDo: eventually localize these
        public const string RootDirectoryNotFound = "Directory {root} not found";
    }

    public class FilesystemAnalysis {
        //public FilesystemAnalysis(ILog log, int asyncFileReadBlocksize, MD5 mD5) {
        public FilesystemAnalysis(ILog log, int asyncFileReadBlocksize) {
            Log.Debug($"starting FilesystemAnalysis ctor (asyncFileReadBlocksize = {asyncFileReadBlocksize})");
            Log=log??throw new ArgumentNullException(nameof(log));
            AsyncFileReadBlocksize=asyncFileReadBlocksize;
            // MD5=mD5??throw new ArgumentNullException(nameof(mD5));
            Log.Debug("leaving FilesystemAnalysis ctor");
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

        public async Task WalkFileSystem(string root, IWalkFilesystemResultContainer walkFileSystemResultContainer, Action<string> recordRoot = null, Action<string[]> recordSubdir = null) {
            Log.Debug($"starting WalkFileSystem: root = {root}");

            if (!Directory.Exists(root)) {
                throw new ArgumentException(ExceptionErrorMessages.RootDirectoryNotFound);
            }

            // Data structure to hold names of subfolders to be examined for files.
            Stack<string> dirs = new Stack<string>();
            string currentDir = root;

            WalkFilesystemResult walkFilesystemResult = walkFileSystemResultContainer.WalkFilesystemResult as WalkFilesystemResult;
            // ToDo: Initialize the fields of the walkFilesystemResult
            walkFilesystemResult.NumberOfDirectories=1;
            walkFilesystemResult.NumberOfFiles=1;
            walkFilesystemResult.DeepestDirectoryTree=1;
            walkFilesystemResult.LargestFile=0;
            // Store root dir in the DB's node table as type directory
            recordRoot.Invoke(root);
            dirs.Push(root);
            // go down all the dirs, doing every file in each dir first, then the next dir
            //  dirs are done in the order that they are returned by Directory.GetDirectories(currentDir)
            while (dirs.Count>0) {
                currentDir=dirs.Pop();
                string[] subDirs = { };
                string[] files = { };

                try {
                    subDirs=Directory.GetDirectories(currentDir);

                }
                catch (Exception e) when (e is UnauthorizedAccessException||e is DirectoryNotFoundException) {
                    // Thrown if we do not have discovery permission on the directory.
                    // Thrown if another process has deleted the directory after we retrieved its name.
                    // Store permission error on the current dir in the node table and accumulate errors to report back to GUI;
                    // if (updateCurrentDirWithException!=null){ updateCurrentDirWithException.Invoke(currentDir); }
                    // Add this exception to the results
                    walkFilesystemResult.Exceptions.Add(e);
                    continue;
                }
                // update the results
                walkFilesystemResult.NumberOfDirectories+=subDirs.Length;
                // update the with node and edge information about all subdirs
                if (recordSubdir!=null) { recordSubdir.Invoke(subDirs); }

                try {
                    files=Directory.GetFiles(currentDir);
                }
                catch (Exception e) when (e is UnauthorizedAccessException||e is DirectoryNotFoundException||e is IOException) {
                    // Thrown if we do not have discovery permission on the directory.
                    // Thrown if another process has deleted the directory after we retrieved its name.
                    // Thrown for a generic IO exception
                    // Store permission error on the current dir in the node table and accumulate errors to report back to GUI;
                    // if (updateCurrentDirWithException!=null){ updateCurrentDirWithException.Invoke(currentDir); }
                    // Add this exception to the results
                    walkFilesystemResult.Exceptions.Add(e);
                    continue;
                }
                // update the results
                walkFilesystemResult.NumberOfFiles+=files.Length;
                // Exception handling gets tricky. files is an array of path strings
                //  each has to be FileIO opened, information about each file extracted, and the file read an hashed
                //  This should be done with async tasks.
                //  when all of the files have been processed, only then should we try to update the database.

                // Get FileInfo and Hash Files here. Create as many tasks and FileInfoEx containers as there are files
                List<Task<FileInfoEx>> taskList = new List<Task<FileInfoEx>>();
                foreach (var f in files) {
                    taskList.Add(PopulateFileInfoExASync(f, AsyncFileReadBlocksize));
                }
                // wait for all to finish
                await Task.WhenAll(taskList);

                // best to pass the tasklist into the Action, and let it analyze the tasklist and create the
                //  appropriate database statements to insert the Node and Edge information about all files.
                // ToDO: if (recordFiles!=null) { recordFiles.Invoke(currentdir, taskList); }

                // here, get the information from the tasklist needed to populate the walkFilesystemResult
                foreach (var task in taskList) {
                    // append all exceptions from each task to the walkFilesystemResult
                    if (task.Result.Exceptions.Count>0) {
                        walkFilesystemResult.Exceptions.AddRange(task.Result.Exceptions);
                    } else {
                        // increment the walkFilesystemResult.LargestFile if any file is larger
                        if (task.Result.FileInfo.Length>walkFilesystemResult.LargestFile)
                            walkFilesystemResult.LargestFile=task.Result.FileInfo.Length;
                    }
                }

                // Push the subdirectories onto the stack for traversal.
                foreach (string str in subDirs) {
                    // ToDo: Get DirectoryInfo for each directory
                    // ToDo: only push if there is no exception
                    // ToDo: Insert the Node and Edge information about all directories into the DB
                    // ToDo: update the DirectoryInfoEx in subDirs with the nodeID for each subdir as returned by the insert
                    dirs.Push(str);
                }
            }
        }

        public async Task<FileInfoEx> PopulateFileInfoExASync(string path, int blocksize) {
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
        int AsyncFileReadBlocksize { get; set; }
        #endregion
        // the hasher
        // ToDo: Find a MD5 algorithm that is thread-safe and can be reused; the one below throws a cryptographic exception when called a second time (after transformFinalBlock)
        // ToDo: Make this into a list of hash functions that can be used on a filestream, and make it possible for any method in this class to select one from the list. Allows the user to select the hash function to be used.
        //MD5= System.Security.Cryptography.MD5.Create();
        #region Properties:Hasher MD5
        //System.Security.Cryptography.MD5 MD5 { get; set; }
        #endregion
        #endregion

        #region Disposable
        // ToDo: Dispose of any hashers that were created

        #endregion
    }


    public class AnalyzeOneDiskDriveResult {
        public AnalyzeOneDiskDriveResult() : this(new DiskInfoEx(), new List<PartitionInfoEx>(), new List<DirectoryInfoEx>(),
          new List<FileInfoEx>(), new List<string>(), new List<string>(), new List<Exception>(), new TimeSpan()) { }

        public AnalyzeOneDiskDriveResult(DiskInfoEx diskInfoEx, List<PartitionInfoEx> partitionInfoExs, List<DirectoryInfoEx> directoryInfoExs, List<FileInfoEx> fileInfoExs,
          List<string> nodeInserts, List<string> edgeInserts, List<Exception> exceptions, TimeSpan elapsedTime) {
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
        public IList<PartitionInfoEx> PartitionInfoExs { get; set; }
        public IList<DirectoryInfoEx> DirectoryInfoExs { get; set; }
        public IList<FileInfoEx> FileInfoExs { get; set; }
        public IList<string> NodeInserts { get; set; }
        public IList<string> EdgeInserts { get; set; }
        public IList<Exception> Exceptions { get; set; }
        public TimeSpan ElapsedTime { get; set; }
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

