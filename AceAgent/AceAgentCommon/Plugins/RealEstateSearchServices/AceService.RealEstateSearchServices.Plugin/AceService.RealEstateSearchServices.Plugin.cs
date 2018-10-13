using System;
using ServiceStack;
using Ace.AceService.RealEstateSearchServices.Interfaces;
namespace AceService.RealEstateSearchServices.Plugin
{
  public class RealEstateSearchServicesPluginData
  {
    string rootPath;
    // Create the plugIn's observable data structures based on the configuration settings
    public RealEstateSearchServicesPluginData() : this(string.Empty)
    {
    }

    // Create the plugIn's observable data structures by specifying each in a constructor call
    public RealEstateSearchServicesPluginData(string rootPath)
    {
      this.rootPath = rootPath;
    }

    //ToDo: constructors with event handlers

    public string RootPath => rootPath;
  }

  public class RealEstateSearchServicesPlugin : IPlugin, IPreInitPlugin
  {
    public void Configure(IAppHost appHost)
    {
      // Create the plugIn's observable data structures 
      // and pass it to the container so it will be available to every other module and services
      appHost.GetContainer()
      .Register<RealEstateSearchServicesPluginData>(observableDataStructures => new RealEstateSearchServicesPluginData());

      // ToDo: enable the mechanisms that monitors each GUI-specific data sensor, and start them running

    }

     {
    /// <summary>
    /// Register this plugin with the appHost
    /// Configure its observableDataStructures and event handlers
    /// </summary>
    /// <param name="appHost">The hosting provider</param>
    /// 
    public void Register(IAppHost appHost)
    {
      if (null == appHost) { throw new ArgumentNullException("appHost"); }
      appHost.RegisterService<Ace.AceService.RealEstateSearchServices.Interfaces.RealEstateSearchServices>();
      this.Configure(appHost);
    }

#if DEBUG
    const string apipath = @"https://www.realtor.com";
#else
  const string apipath = @"https://www.realtor.com";
#endif
  }
}
