using System.Reflection;

// ATAP.Utilities.BuildTooling.targets will update the build (days since 1/1/2000), and revision (seconds since midnight, UTC, / 2) fields each time a new build occurs
[assembly:AssemblyFileVersion("0.1.6843.38401")]
// ATAP.Utilities.BuildTooling.targets will update the AssemblyInformationalVersion field each time a new build occurs
[assembly:AssemblyInformationalVersion("0.1.0-alpha-024")]
[assembly:AssemblyVersion("0.1.0")]
// Turn on ETW logging for Method Entry, Method Exit, and Exceptions
[assembly: ATAP.Utilities.ETW.ETWLogAttribute()]
