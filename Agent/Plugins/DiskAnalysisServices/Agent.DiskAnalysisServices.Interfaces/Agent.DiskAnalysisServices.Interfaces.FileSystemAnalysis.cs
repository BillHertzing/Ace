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
using ATAP.Utilities.Shared;
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
using ServiceStack.Messaging;

namespace Ace.Plugin.DiskAnalysisServices {
#if TRACE
    [ETWLogAttribute]
#endif
    public partial class DiskAnalysisServices : Service {



        public async Task<object> Post(AnalyzeFileSystemRequest request) {
            // Housekeeping setup for the task to be created
            // Create new Id for this LongRunningTask
            Id<LongRunningTaskInfo> longRunningTaskID = new Id<LongRunningTaskInfo>(Guid.NewGuid());
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationTokenSourceId = new Id<CancellationTokenSource>(Guid.NewGuid());
            var cancellationToken = cancellationTokenSource.Token;
            Log.Debug("in Post(AnalyzeFileSystemRequest) 1");
            // Get the BaseServicesData and diskAnalysisServicesData instances that were injected into the DI container
            // ToDo: wrap in a try / catch
            var baseServicesData = HostContext.TryResolve<BaseServicesData>();
            // ToDo: wrap in a try / catch
            var diskAnalysisServicesData = HostContext.TryResolve<DiskAnalysisServicesData>();

            // Get the AMQPService instances that are stored in the diskAnalysisServicesData instance
            // Ensure that this instance of AceCommander has the appropriate capabilities to record a FileSystemAnalysis to persistence using a message queue

            // ToDo: query a data structure that has features and capabilities
            // ToDo: if (baseServicesData.FeaturesCOD.ContainsKey ("Todo:DefineAConstantForMeHasQueues") && baseServicesData.FeaturesCOD("Todo:defineaconstantforMeHasQueues")== true)
            // ToDo: && (baseServicesData.FeaturesCOD.ContainsKey ("Todo:DefineAConstantForMeHasPersistentDiskAnalysis") && baseServicesData.FeaturesCOD("Todo:defineaconstantforMeHasPersistentDiskAnalysis")== true)
            // ToDo: && (baseServicesData.FeaturesCOD.ContainsKey ("Todo:DefineAConstantForMeHasQueuesUsingPersistentDiskAnalysis") && baseServicesData.FeaturesCOD("Todo:defineaconstantforMeHasQueuesUsingPersistentDiskAnalysis")== true)
            int conditions = 0;
            Type recordRootTReq;
            Type recordRootTRsp;
            switch (conditions) {
                case 0: { // No persistence, no Message Queue
                    break;
                }
                case 1: // Persistence only, no Message Queue
                {
                    recordRootTReq = typeof(RecordRootReqDTOORM);
                    recordRootTRsp = typeof(RecordRootRspDTOORM);
                    break;
                }
                case 2: // No Persistence, Message Queue only
                {
                    recordRootTReq = typeof(RecordRootReqDTOMessage);
                    recordRootTRsp = typeof(RecordRootRspDTOMessage);
                    break;
                }
                case 3:
                { //  both Persistence and Message Queue
                    recordRootTReq=typeof(RecordRootReqDTONested);
                    recordRootTRsp=typeof(RecordRootRspDTONested);
                    var recordRoot = new RecordRootBuilder(RecordRootReqDTONested, RecordRootRspDTONested).Build();
                    var recordFSEntities = new RecordFSEntitiesBuilder(RecordFSEntitiesReqDTONested, RecordFSEntitiesRspDTONested)
                        .Add(typeof(DirectoryInfoEx))
                        .Build();

                    // Ask the baseServicesData.FeaturesCOD for the classes that can supply a factory that can supply the parts needed to build all of the lambdas used by this FileSystemAnalysis instance
                    // get the MQServer
                    var messageQueueClientFactory = TryResolve<IAMQPMessageQueueClientFactory>();
                    var messageFactory = TryResolve<IAMQPMessageFactory>();
                    var messageHandlerFactory = TryResolve<IAMQPMessageHandlerFactory>();
                    var messageService = TryResolve<IAMQPMessageService>();
                    var messageProducer = TryResolve<IAMQPMessageProducer>();

                    var messageQueueClient = messageQueueClientFactory.CreateMessageQueueClient();
                    IMessageClient<RecordRootReq> recordRootMessageClient = recordRootBuilder.Build();

                    Factory messageServiceFactory = baseServicesData.PlugIns(nameof(messageServiceFactory)).GetFactory();
                    IMessageClient<RecordRootReqDTONested> recordRootMessageClient = recordRootBuilder.Build();
                    IMessageServiceForMeHasQueuesUsingPersistentDiskAnalysis messageService = HostContext.TryResolve<IMessageServiceForMeHasQueuesUsingPersistentDiskAnalysis>();
                    break;
                }
                default: {
                    throw new NotImplementedException("Todo: add exception constant for ");
                        }
            }
                    // Get the persistence-backed messageservice for instances of the types used by the plugin FileSystemAnalysis stored in the DI

            // Setup the instance. Use Configuration Data if the request payload is null
            var blockSize = request.AnalyzeFileSystemRequestPayload.AsyncFileReadBlockSize>=0? request.AnalyzeFileSystemRequestPayload.AsyncFileReadBlockSize : diskAnalysisServicesData.ConfigurationData.BlockSize;
            var fileSystemAnalysis = new FileSystemAnalysis(Log.Logger, blockSize, messageService, recordRoot, recordFSEntities);
            Log.Debug("in Post(AnalyzeFileSystemRequest) 2");


            // Create storage for the results and progress
            var analyzeFileSystemResult = new AnalyzeFileSystemResult();
            diskAnalysisServicesData.AnalyzeFileSystemResultsCOD.Add(longRunningTaskID, analyzeFileSystemResult);
            var analyzeFileSystemProgress = new AnalyzeFileSystemProgress();
            diskAnalysisServicesData.AnalyzeFileSystemProgressCOD.Add(longRunningTaskID, analyzeFileSystemProgress);

            // Define the lambda that describes the task
            var task = new Task(() => {
                fileSystemAnalysis.AnalyzeFileSystem(
                    request.AnalyzeFileSystemRequestPayload.Root,
                    analyzeFileSystemResult, analyzeFileSystemProgress,
                    cancellationToken, RecordRoot
                    (crud, r) => {
                        Log.Debug($"starting recordRoot Lambda, r = {r}");
                    }
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
                // ToDo: need to be sure that the when.any loop and GetLongRunningTasksStatus can handle a taskInfo in these states;

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
