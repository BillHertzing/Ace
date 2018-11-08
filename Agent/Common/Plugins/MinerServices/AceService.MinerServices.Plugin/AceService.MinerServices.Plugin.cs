
using ServiceStack;
using Swordfish.NET.Collections;
using System;
using Ace.AceService.MinerServices.Interfaces;


namespace Ace.AceService.MinerServices.Plugin
{


    public class MinerServicesData
    {
    #region string constants
    #region Configuration Key strings
    #endregion Configuration Key strings
    #region Exception Messages (string constants)
    #endregion Exception Messages (string constants)
    #region File Name string constants
    #endregion File Name string constants
    #endregion string constants
    /*
        ConcurrentObservableDictionary<(MinerSWE minerSWE, string version, Coin[] coins), MinerSW> minerSWs;
        ConcurrentObservableDictionary<int, MinerGPU> minerGPUs ;

        public MinerServicesData() : this(new ConcurrentObservableDictionary<(MinerSWE minerSWE, string version, Coin[] coins), MinerSW>(), new ConcurrentObservableDictionary<int, MinerGPU>()) { }

        public MinerServicesData(ConcurrentObservableDictionary<(MinerSWE minerSWE, string version, Coin[] coins), MinerSW> minerSWs, ConcurrentObservableDictionary<int, MinerGPU> minerGPUs)
        {
            this.minerSWs = minerSWs;
            this.minerGPUs = minerGPUs;
        }

        //ToDo: constructors with event handlers

        public ConcurrentObservableDictionary<(MinerSWE minerSWE, string version, Coin[] coins), MinerSW> MinerSWs { get => minerSWs;}
        public ConcurrentObservableDictionary<int, MinerGPU> MinerGPUs { get => minerGPUs; }
        */
  }

  public class MinerServicesPlugin : IPlugin
    {
    /// <summary>
    /// Register this plugin with the appHost
    /// setup any necessary objects and intercepts
    /// </summary>
    /// <param name="appHost">The hosting provider</param>
    public void Register(IAppHost appHost)
    {
      if (null == appHost) {  throw new ArgumentNullException("appHost"); }

            appHost.RegisterService<Ace.AceService.MinerServices.Interfaces.MinerServices>();

            var container = appHost.GetContainer();

            // Create the plugIn's observable data structures based on the configuration settings, and the 
            // current computer inventory
            // Get the latest known current configuration, and use that information to populate the data structures
            // appHost.AppSettings.Get<string>("Ace.AceService.MinerPlugin.InstalledMinerSW");
            // appHost.AppSettings.Get<string>("Ace.AceService.MinerPlugin.InstalledGPUs");

            var mspd = new MinerServicesData();

            // if the current mining rig configuration specifies that the mining rig has mining-specific data structures that
            //  can and should be monitored, attach the event handlers that will respond to changes in the monitored data structures

            // setup the mechanisms that monitors each mining-specific data sensor


            // pass the plugIn's observable data structures and event handlers to the container so they will be available to every other module and services
            container.Register<MinerServicesData>(d => mspd);

      // enable the mechanisms that monitors each mining-specific data sensor, and start them running

      //ComputerInventory computerInventory = container.TryResolve(typeof(ComputerInventory)) as ComputerInventory;
      //var x = computerInventory.ComputerHardware.Computer.Hardware.Length;
      // get this computers current power consumption from the sensors package
      //PowerConsumption pc = new PowerConsumption() { Period = new TimeSpan(0, 1, 0), Watts = 1000.0 };
      // get this computers CPU temp and fan structure from the sensors

      // TempAndFan tf = new TempAndFan { Temp = 50, FanPct = 95.5 };
      // rigConfig = RigConfigBuilder.CreateNew()
      //    .AddMinerSWs(mspd.MinerSWs)
      //    .AddMinerGPUs(mspd.MinerGPUs)
      //    .AddPowerConsumption(pc)
      //    .AddTempAndFan(tf)
      //     .Build();
    }
    //    RigConfig rigConfig;
    //    public RigConfig RigConfig { get => rigConfig; }

  }
}
