using System;
using ATAP.Utilities.ComputerInventory.Enumerations;
using System.Runtime.InteropServices;

namespace ATAP.Utilities.Runtime {
    public interface IRuntimeKind {
        bool IsConsoleApplication { get; }
        bool IsFreeBSD { get; }
        bool IsLinux { get; }
        bool IsOSX { get; }
        bool IsWindows { get; }
        RuntimePlatformLifetime Kind { get; }
    }

    class RuntimeKind : IRuntimeKind {
        public RuntimeKind(bool isCA) {
            RuntimePlatformLifetime kind;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                kind=isCA ? RuntimePlatformLifetime.WindowsConsoleApp : RuntimePlatformLifetime.WindowsService;
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                kind=isCA ? RuntimePlatformLifetime.LinuxConsoleApp : RuntimePlatformLifetime.LinuxDaemon;
            //} else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD)) {
            //    kind=isCA ? RuntimePlatformLifetime.FreeBSDConsoleApp : RuntimePlatformLifetime.FreeBSDDaemon;
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                kind=isCA ? RuntimePlatformLifetime.OSXConsoleApp : RuntimePlatformLifetime.OSXDaemon;
            } else {
                // ToDo: replace exception message with a string constant
                throw new InvalidOperationException("unknown RuntimeInformation OSPlatform");
            }
            new RuntimeKind(kind, isCA);
        }
        public RuntimeKind(RuntimePlatformLifetime kind, bool isCA) {
            IsConsoleApplication=isCA;
            Kind=kind;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                IsWindows=true;
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                IsLinux=true;
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                IsOSX=true;
            } else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                IsFreeBSD=true;
            } else

                // ToDo: replace exception message with a string constant
                new InvalidOperationException("unknown RuntimeInformation OSPlatform");
        }
        public RuntimePlatformLifetime Kind { get; private set; }
        public bool IsConsoleApplication { get; private set; } = false;
        public bool IsWindows { get; private set; } = false;
        public bool IsLinux { get; private set; } = false;
        public bool IsOSX { get; private set; } = false;
        public bool IsFreeBSD { get; private set; } = false;
    }
}