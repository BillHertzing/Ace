using ATAP.Utilities.ComputerHardware.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using Itenso.TimePeriod;
using System.Threading.Tasks;
using System.Threading;
using ATAP.Utilities.Database.Enumerations;
// for .Dump utility
using ServiceStack.Text;
// ToDo: figure out logging for the ATAP libraries, this is only temporary
using ServiceStack.Logging;
using ATAP.Utilities.DiskDrive;
using ATAP.Utilities.FileSystem;

namespace ATAP.Utilities.LongRunningTasks {

    public static class LongRunningTasksExtensions {
        public static async Task<bool> CancelTask (this LongRunningTaskInfo me) {
            var cTS = me.CTS;
            cTS.Cancel();
            // ToDo: implement a better understanding of cancellation and how to handle it 
            await Task.Yield();

            //Log.Debug($"leaving CancelTask: LRTId = {me.LRTId}");
            return true;
        }

        #region Properties
        #region Properties:class logger
        //public ILog Log;
        #endregion
        #endregion
    }

 }
