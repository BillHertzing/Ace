using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;

namespace Ace.GUI
{
    public class MethodBoundaryLogProvider
    {

        public static ILogger Logger { get; set; }
        public static ILoggerFactory LoggerFactory { get; set; }
        MethodBoundaryLogProvider(ILogger logger, ILoggerFactory loggerFactory) {
            LoggerFactory=loggerFactory;
            //    Logger=loggerFactory.CreateLogger<T>();
            //    Log=(BlazorLogProviderComponent<T>)LoggerFactory.CreateLogger<T>();
            Logger=logger;
        }

        public static void LogDebug(string message)
        {
            //throw new System.Exception("Fody Methodboundry LogDebug happened");
        }


        public static void LogMethodEntry(string category, string methodName)
        {
            //if (IsEnabled()) {
            //    //WriteEvent(3, message);
            //}
            if (LoggerFactory != null)
            {
                Logger = LoggerFactory.CreateLogger(category);
                Logger.LogDebug($"<{category}.{methodName}");
            }
        }

        public static void LogMethodExit(string category, string methodName)
        {
            //if (IsEnabled()) {
            //    //WriteEvent(3, message);
            //}
            if (LoggerFactory != null)
            {
                Logger = LoggerFactory.CreateLogger(category);
                Logger.LogDebug($"{category}.{methodName}>");
            }
        }

        // ToDo: add exception (or aggregate exception) message
        public static void LogMethodException(string category, string exceptionType, string exceptionMessage)
        {
            //if (IsEnabled()) {
            //    //WriteEvent(3, message);
            //}
            if (LoggerFactory != null)
            {
                Logger = LoggerFactory.CreateLogger(category);
                Logger.LogDebug($"Exception {exceptionType} with message: {exceptionMessage}");
            }
        }

    }
}
