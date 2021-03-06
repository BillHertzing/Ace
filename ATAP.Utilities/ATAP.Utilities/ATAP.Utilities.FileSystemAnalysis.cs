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
using ATAP.Utilities.ComputerHardware.Enumerations;
using ATAP.Utilities.Database.Enumerations;
using ATAP.Utilities.DiskDrive;

// ToDo: figure out logging for the ATAP libraires, this is only temporary
using ServiceStack.Logging;
using ATAP.Utilities.FileSystem;
using System.Linq;
using System.Security.Cryptography;
using ATAP.Utilities.TypedGuids;

namespace ATAP.Utilities.FileSystem {
    public static class ExceptionErrorMessages {
        // ToDo: eventually localize these
        public const string RootDirectoryNotFound = "Directory {root} not found";
    }

    public class FileSystemAnalysis {

        //public FileSystemAnalysis(ILog log, int asyncFileReadBlockSize, MD5 mD5) {
        public FileSystemAnalysis(ILog log, int asyncFileReadBlockSize) {
            Log=log??throw new ArgumentNullException(nameof(log));
            Log.Debug($"starting FileSystemAnalysis ctor (asyncFileReadBlockSize = {asyncFileReadBlockSize})");
            // ToDo: make the exception message a constant localizable string)
            AsyncFileReadBlocksize =(asyncFileReadBlockSize>=0)? asyncFileReadBlockSize:throw new ArgumentOutOfRangeException($"asyncFileReadBlockSize must be greater than 0, received {asyncFileReadBlockSize}");
            // MD5=mD5??throw new ArgumentNullException(nameof(mD5));
            Log.Debug("leaving FileSystemAnalysis ctor");
        }
        /*  Move this stuff to teh Expression for the Action that will validate the DB
         *  The block below is somewhat out of date, the structures carry two, 
         *  separate GUIDs for DB id and in-memory id
        if (DBFetch==null) {
            diskInfoEx.DiskIdentityId=0;
            diskInfoEx.DiskGuid=Guid.NewGuid();
            Log.Debug($"in PopulateDiskInfoExs: awaiting PopulatePartitionInfoExs,  diskInfoEx = {diskInfoEx}");
            await PopulatePartitionInfoExs(cRUD, diskInfoEx);
            Log.Debug($"in PopulateDiskInfoExs: PopulatePartitionInfoExs has completed,  diskInfoEx = {diskInfoEx}");
        } else {
            // Todo: see if the DiskDriveMaker and SerialNumber already exist in the DB
            // async (cRUD, diskInfoEx) => { await Task.Yield(); }
            // Task< DiskDriveInfoEx> t = await DBFetch.Invoke(cRUD, diskInfoEx);
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

        public async Task AnalyzeFileSystem(string root, IAnalyzeFileSystemResult analyzeFileSystemResults, IAnalyzeFileSystemProgress analyzeFileSystemProgress, CancellationToken cancellationToken, Action<CrudType, string> recordRoot = null, Action<CrudType, string[]> recordSubdir = null) {
            Log.Debug($"starting AnalyzeFileSystem: root = {root}");

            if (!Directory.Exists(root)) {
                throw new ArgumentException(ExceptionErrorMessages.RootDirectoryNotFound);
            }

            // Data structure to hold names of subfolders to be examined for files.
            Stack<string> dirs = new Stack<string>();
            string currentDir = root;

            // ToDo: Initialize the fields of the analyzeFileSystemProgress
            analyzeFileSystemProgress.Completed=false;
            analyzeFileSystemProgress.NumberOfDirectories=0;
            analyzeFileSystemProgress.NumberOfFiles=0;
            analyzeFileSystemProgress.DeepestDirectoryTree=0;
            analyzeFileSystemProgress.LargestFile=0;
            // check CancellationToken to see if this task is cancelled
            if (cancellationToken.IsCancellationRequested) {
                // nothing to cleanup here
                Log.Debug($"in AnalyzeFileSystem: Cancellation requested (1st checkpoint)");
                cancellationToken.ThrowIfCancellationRequested();
            }
            // After this point, there may be cleanup to do if the task is cancelled
            // Store root dir in the DB's node table as type directory
            if (recordRoot!=null) { recordRoot.Invoke(CrudType.Create, root); }
            // check CancellationToken to see if this task is cancelled
            if (cancellationToken.IsCancellationRequested) {
                // database cleanup is needed
                Log.Debug($"in AnalyzeFileSystem: Cancellation requested (2nd checkpoint)");
                // DatabaseRollback();
                cancellationToken.ThrowIfCancellationRequested();
            }
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
                    analyzeFileSystemProgress.Exceptions.Add(e);
                    continue;
                }
                // update the results
                analyzeFileSystemProgress.NumberOfDirectories+=subDirs.Length;
                // update the with node and edge information about all subdirs
                if (recordSubdir!=null) { recordSubdir.Invoke(CrudType.Create, subDirs); }
                // check CancellationToken to see if this task is cancelled
                if (cancellationToken.IsCancellationRequested) {
                    // database cleanup is needed
                    Log.Debug($"in AnalyzeFileSystem: Cancellation requested (2nd checkpoint)");
                    // DatabaseRollback();
                    cancellationToken.ThrowIfCancellationRequested();
                }
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
                    analyzeFileSystemProgress.Exceptions.Add(e);
                    continue;
                }
                // update the results
                analyzeFileSystemProgress.NumberOfFiles+=files.Length;
                // Exception handling gets tricky. files is an array of path strings
                //  each has to be FileIO opened, information about each file extracted, and the file read an hashed
                //  This should be done with async tasks.
                //  when all of the files have been processed, only then should we try to update the database.

                // Get FileInfo and Hash Files here. Create as many tasks and FileInfoEx containers as there are files
                List<Task<FileInfoEx>> taskList = new List<Task<FileInfoEx>>();
                foreach (var f in files) {
                    taskList.Add(PopulateFileInfoExAsync(f, AsyncFileReadBlocksize, cancellationToken));
                }
                // wait for all to finish
                await Task.WhenAll(taskList);
                // check CancellationToken to see if this task is cancelled
                if (cancellationToken.IsCancellationRequested) {
                    // ToDo: investigate how best to handle the aggregate exceptions that may bubble up
                    // database cleanup is needed
                    Log.Debug($"in AnalyzeFileSystem: Cancellation requested (3rd checkpoint)");
                    // DatabaseRollback();
                    cancellationToken.ThrowIfCancellationRequested();
                }

                // best to pass the tasklist into the Action, and let it analyze the tasklist and create the
                //  appropriate database statements to insert the Node and Edge information about all files.
                // ToDO: if (recordFiles!=null) { recordFiles.Invoke(currentdir, taskList); }
                if (cancellationToken.IsCancellationRequested) {
                    // ToDo: investigate how best to handle the aggregate exceptions that may bubble up
                    // database cleanup is needed
                    Log.Debug($"in AnalyzeFileSystem: Cancellation requested (3rd checkpoint)");
                    // DatabaseRollback();
                    cancellationToken.ThrowIfCancellationRequested();
                }

                // here, get the information from the tasklist needed to populate the AnalyzeFileSystem
                foreach (var task in taskList) {
                    // append all exceptions from each task to the AnalyzeFileSystem and the AnalyzeFileProgress
                    if (task.Result.Exceptions.Count>0) {
                        foreach (var e in task.Result.Exceptions) {
                            analyzeFileSystemResults.Exceptions.Add(e);
                            analyzeFileSystemProgress.Exceptions.Add(e);
                        }
                    } else {
                        // increment the analyzeFileProgress.LargestFile if any file is larger
                        if (task.Result.FileInfo.Length>analyzeFileSystemProgress.LargestFile) {
                            analyzeFileSystemProgress.LargestFile=task.Result.FileInfo.Length;
                        }
                    }
                    // Add to the file node this file
                    // add to the edge node this file and the currentdir
                }
                if (cancellationToken.IsCancellationRequested) {
                    // ToDo: investigate how best to handle the aggregate exceptions that may bubble up
                    // database cleanup is needed
                    Log.Debug($"in AnalyzeFileSystem: Cancellation requested (4th checkpoint)");
                    // DatabaseRollback();
                    cancellationToken.ThrowIfCancellationRequested();
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

        public async Task<FileInfoEx> PopulateFileInfoExAsync(string path, int blocksize, CancellationToken cancellationToken) {
            FileInfoEx fileInfoEx = new FileInfoEx() {
                Path=path,
                FileInfoId=new Id<FileInfoEx>(Guid.NewGuid()),
                FileInfoDbId=new Id<FileInfoEx>(Guid.Empty),
                Exceptions=new List<Exception>()
            };
            if (cancellationToken.IsCancellationRequested) {
                Log.Debug($"in PopulateFileInfoExAsync: Cancellation requested (1st checkpoint)");
                cancellationToken.ThrowIfCancellationRequested();
            }
            try {
                // read all bytes and generate the hash for the file
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, blocksize, true)) // true means use IO async operations
                {
                    // ToDo: move the instance of the md5 hasher out of the task, but ensure the implementation of the instance is thread-safe and can be reused
                    // ToDo: The MD5 hasher found in the AnalyzeDisk properties cannot be reused after the call to TransformFinalBlock
                    // ToDo: The MD5Cng implementation is not available on netstandard2.0
                    using (var md5 = System.Security.Cryptography.MD5.Create()) {
                        byte[] buffer = new byte[blocksize];
                        int bytesRead;
                        do {
                            bytesRead=await stream.ReadAsync(buffer, 0, blocksize);
                            if (cancellationToken.IsCancellationRequested) {
                                Log.Debug($"in PopulateFileInfoExAsync: Cancellation requested (2nd checkpoint)");
                                // dispose of the hasher
                                md5.Dispose();
                                cancellationToken.ThrowIfCancellationRequested();
                            }
                            if (bytesRead>0) {
                                md5.TransformBlock(buffer, 0, bytesRead, null, 0);
                                if (cancellationToken.IsCancellationRequested) {
                                    Log.Debug($"in PopulateFileInfoExAsync: Cancellation requested (3rd checkpoint)");
                                    // dispose of the hasher
                                    md5.Dispose();
                                    cancellationToken.ThrowIfCancellationRequested();
                                }
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

