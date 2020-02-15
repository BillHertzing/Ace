using System;
using ServiceStack;
using ServiceStack.Text;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Collections.Generic;
using ATAP.Utilities.ComputerInventory;
using ATAP.Utilities.ComputerHardware.Enumerations;
using ATAP.Utilities.FileSystem;
using ATAP.Utilities.FileSystem.Enumerations;
using ATAP.Utilities.Database.Enumerations;
using System.Threading.Tasks;
using Funq;
using System.Threading;
using Ace.Agent.BaseServices;
using ATAP.Utilities.LongRunningTasks;
using ATAP.Utilities.TypedGuids;
using ATAP.Utilities.DiskDriveAnalysis;
using ATAP.Utilities.DiskDrive;
using System.Text;
using System.Net.Http;
using Serilog;
using ATAP.Utilities.ETW;
using System.IO;
using System.Security.Permissions;
using ATAP.Utilities.Persistence.FileSystem;
using ATAP.Utilities.Persistence;

namespace Ace.Plugin.DiskAnalysisServices {
#if TRACE
    [ETWLogAttribute]
#endif
    public partial class DiskAnalysisServices : Service {

        class DBInsertResults {
            public bool Success;
        }

        // ToDo make this pass an FSEntities collection instead of a string
        class DBInsertCollection {
            public IEnumerable<string> DList;

        }

        public async Task<object> Post(AnalyzeFileSystemRequest request) {
            // Housekeeping setup for the task to be created
            // Create new Id for this LongRunningTask
            Id<LongRunningTaskInfo> longRunningTaskID = new Id<LongRunningTaskInfo>(Guid.NewGuid());
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationTokenSourceId = new Id<CancellationTokenSource>(Guid.NewGuid());
            var cancellationToken = cancellationTokenSource.Token;
            Log.Debug("in Post(AnalyzeFileSystemRequest) 1");
            // Get the BaseServicesData and diskAnalysisServicesData instances that were injected into the DI container
            var baseServicesData = HostContext.TryResolve<BaseServicesData>();
            var diskAnalysisServicesData = HostContext.TryResolve<DiskAnalysisServicesData>();

            // Setup the instance. Use Configuration Data if the request payload is null
            var blockSize = request.AnalyzeFileSystemRequestPayload.AsyncFileReadBlockSize>=0? request.AnalyzeFileSystemRequestPayload.AsyncFileReadBlockSize : diskAnalysisServicesData.ConfigurationData.BlockSize;
            var fileSystemAnalysis = new FileSystemAnalysis(Log.Logger, diskAnalysisServicesData.ConfigurationData.BlockSize);
            Log.Debug("in Post(AnalyzeFileSystemRequest) 2");

            // Create storage for the results and progress
            var analyzeFileSystemResult =new AnalyzeFileSystemResult();
            diskAnalysisServicesData.AnalyzeFileSystemResultsCOD.Add(longRunningTaskID, analyzeFileSystemResult);
            var analyzeFileSystemProgress = new AnalyzeFileSystemProgress();
            diskAnalysisServicesData.AnalyzeFileSystemProgressCOD.Add(longRunningTaskID, analyzeFileSystemProgress);

            /*
            // Create instances for Persistence
            // ToDo: select the kind of persistence from the ConfigurationRoot
            // Create a trio of Functions that stores intermediate data to a local file
            // ToDo: Add CancellationToken
            // ToDo: Make Setup return a thread safe structure to the local file
            var PersistenceViaFileSetup = new Func<PersistenceViaFileSetupInitializationData, PersistenceViaFileSetupResults>((persistenceSetupInitializationData) => {
                bool success = false;
                //ToDo: lots of tests on the Filename passed to validate it is OK to be opened
                // open The Filename in a thread-safe manner
                // ToDo: more specific file access and sharing modes
                switch (persistenceSetupInitializationData) {
                    case PersistenceViaFileSetupInitializationData PVFSID:
                        FileStream wFS = new System.IO.FileStream(PVFSID.FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, 4096, FileOptions.None);
                        StreamWriter wSW = new StreamWriter(wFS);
                        return new PersistenceViaFileSetupResults() { Success=success, FileStream=wFS, StreamWriter=wSW };
                    default:
                        throw new ArgumentException(
                                        message: "persistenceSetupInitializationData is not a recognized derived type of PersistenceSetupInitializationData",
                                        paramName: nameof(persistenceSetupInitializationData));
                }
            });
            // ToDo: Make Insert write in a thread-safe manner to the local file
            //ToDo: Add cancellationToken
            Func<string, PersistenceSetupResults, PersistenceViaFileInsertResults> PersistNode = new Func<string, PersistenceSetupResults, PersistenceViaFileInsertResults>((nodeStr, persistenceSetupResults) => {
                //ToDo:  test for cancellation token
                PersistenceViaFileInsertResults persistenceViaFileInsertResults = ValidateNodeStr(nodeStr);
                //ToDo:  test for cancellation token
                persistenceViaFileInsertResults = persistenceViaFileInsertResults.WriteTofSW(nodeStr);
                return persistenceViaFileInsertResults;
            });
            Func<PersistenceInsertData, PersistenceSetupResults, PersistenceInsertResults> PersistenceViaFileInsert = new Func<PersistenceViaFileInsertData, PersistenceViaFileSetupResults, PersistenceViaFileInsertResults>((persistenceInsertData, persistenceSetupResults) => { bool success = true; return new PersistenceViaFileInsertResults() { Success = success }; }) as Func<PersistenceInsertData, PersistenceInsertResults>;
            Func<PersistenceTearDownInitializationData, PersistenceSetupResults,  PersistenceTearDownResults> PersistenceViaFileTearDown = new Func<PersistenceViaFileTearDownInitializationData, PersistenceViaFileSetupResults, PersistenceViaFileTearDownResults>((persistenceSetupInitializationData) => { bool success = true; return new PersistenceViaFileTearDownResults() { Success=success }; }) as Func<PersistenceTearDownInitializationData, PersistenceTearDownResults>;

            PersistenceViaFile persistenceStructure = new PersistenceViaFile() {
                PersistenceSetupInitializationData=new PersistenceViaFileSetupInitializationData() {
                },
                PersistenceSetupResults=new PersistenceViaFileSetupResults(),
                Func<PersistenceSetupInitializationData, PersistenceSetupResults> PersistenceSetup = PersistenceViaFileSetup,
                PersistenceInsertData=new PersistenceViaFileInsertData(),
                PersistenceInsertResults=new PersistenceViaFileInsertResults(),
                PersistenceInsert=PersistenceViaFileInsert,
                PersistenceTearDownInitializationData=new PersistenceViaFileTearDownInitializationData(),
                PersistenceTearDownResults=new PersistenceViaFileTearDownResults(),
                PersistenceTearDown=PersistenceViaFileTearDown
            };

            // Create a Function that stores intermediate data in OrmLite
            // This is where the IORM branch will branch from Develop
            // DBInsertResults dBInsertResult;
            //DBInsertCollection dBInsertCollection;
            //dBInsertCollection=new DBInsertCollection() { DList=new List<FSentities>() ; }
            // var F1 = new Func<DBInsertCollection, DBInsertResults>((dBInsertCollection) => { bool success = true; return new DBInsertResults() { Success = success }; });

            // Create a function that calls the MQ to pass intermediate data to a Function that stores the intermediate data

            // Based on Request or Configuration select which Function will be used to store the intermediate data
            */

            // Temporary for purpose of clean compile no functionality 

            PersistenceViaFile persistenceStructure = new PersistenceViaFile() {
                PersistenceSetupInitializationData=new PersistenceViaFileSetupInitializationData() {
                },
                PersistenceSetupResults=new PersistenceViaFileSetupResults(),
                //Func<PersistenceSetupInitializationData, PersistenceSetupResults>PersistenceSetup=PersistenceViaFileSetup,
                PersistenceInsertData=new PersistenceViaFileInsertData(),
                PersistenceInsertResults=new PersistenceViaFileInsertResults(),
                //PersistenceInsert=PersistenceViaFileInsert,
                PersistenceTearDownInitializationData=new PersistenceViaFileTearDownInitializationData(),
                PersistenceTearDownResults=new PersistenceViaFileTearDownResults(),
                //PersistenceTearDown=PersistenceViaFileTearDown
            };


            // Define the lambda that describes the FileSystemAnalysis task
            var task = new Task(() => {
                fileSystemAnalysis.AnalyzeFileSystem(
                    request.AnalyzeFileSystemRequestPayload.Root,
                    analyzeFileSystemResult, analyzeFileSystemProgress,
                    cancellationToken,
                    persistenceStructure
                ).ConfigureAwait(false);
            });

            LongRunningTaskInfo longRunningTaskInfo = new LongRunningTaskInfo(longRunningTaskID, task, cancellationTokenSource) ;
            // Record this task (plus additional information about it) in the longRunningTasks dictionary in the BaseServicesData found in the Container
            baseServicesData.LongRunningTasks.Add(longRunningTaskID, longRunningTaskInfo);
            // record the TaskID and task info into the LookupDiskDriveAnalysisResultsCOD
            diskAnalysisServicesData.LookupFileSystemAnalysisResultsCOD.Add(longRunningTaskID, longRunningTaskInfo);

            // ToDo: Setup the SSE receiver that will monitor the long-running task
            // ToDo: return to the caller the callback URL and the longRunningTaskID to allow the caller to connect to the SSE that monitors the task and the data structures it updates
            // Start the task running
            try {
                baseServicesData.LongRunningTasks[longRunningTaskID].LRTask.Start();
            }
            catch (Exception e) when (e is InvalidOperationException||e is ObjectDisposedException) {
                Log.Debug($"Exception when trying to start the AnalyzeDiskDrive task, message is {e.Message}");
                // ToDo: need to be sure that the when.any loop and GetLongRunningTasksStatus can handle a taskinfo in these states;

            }
            var analyzeFileSystemResponsePayload = new AnalyzeFileSystemResponsePayload(new List<Id<LongRunningTaskInfo>>() { longRunningTaskID });
            var analyzeFileSystemResponse = new AnalyzeFileSystemResponse(analyzeFileSystemResponsePayload);
            Log.Debug($"in AnalyzeFileSystemRequest analyzeFileSystemResponse = {analyzeFileSystemResponse.Dump()}");
            // testing
            //var analyzeFileSystemResponseSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(analyzeFileSystemResponse);
            //Log.Debug($"in AnalyzeFileSystemRequest analyzeFileSystemResponseSerialized = {analyzeFileSystemResponseSerialized.Dump()}");
            //var stringContent = new StringContent(analyzeFileSystemResponseSerialized, Encoding.UTF8, "application/json");
            //Log.Debug($"in AnalyzeFileSystemRequest stringContent = {stringContent.Dump()}");
            // end testing 
            await Task.Yield(); // ToDo: figure out if this is the right way to make the method async.
            Log.Debug($"Leaving Post(AnalyzeFileSystemRequest), analyzeFileSystemResponse = {analyzeFileSystemResponse}");
            return analyzeFileSystemResponse;
        }

    }


}
