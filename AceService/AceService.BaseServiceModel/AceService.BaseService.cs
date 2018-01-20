using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;


namespace Ace.AceService.BaseServiceModel
{
    [Route("/isAlive")]
    public class BaseServiceIsAlive : IReturn<IsAliveResponse>
    {
        //  public string Name { get; set; }
    }

    //[Route("/isAlive/{Name}")]
    //public class BaseServiceIsAlive : IReturn<IsAliveResponse>
    //{
    //    public string Name { get; set; }
    //}

    public class IsAliveResponse
    {
        public string Result { get; set; }
    }

    [Route("/GetConfiguration")]
    public class BaseServiceGetConfiguration : IReturn<GetConfigurationResponse>
    {
        //  public string Name { get; set; }
    }
    public class GetConfigurationResponse
    {
        public string Result { get; set; }
    }


    [Route("/PutConfiguration")]
    public class BaseServicePutConfiguration : IReturn<PutConfigurationResponse>
    {
        //  public string Name { get; set; }
    }
    public class PutConfigurationResponse
    {
        public string Result { get; set; }
    }
    /*
        public class BaseServiceUpdateConfiguration : IReturn<UpdateConfigurationResponse>
        {
            //  public string Name { get; set; }
        }

        public class BaseServiceDeleteConfiguration : IReturn<DeleteConfigurationResponse>
        {
            //  public string Name { get; set; }
        }

        public class BaseServiceWriteConfigurationToStorage : IReturn<WriteConfigurationToStorageResponse>
        {
            //  public string Name { get; set; }
        }
        public class BaseServiceReadConfigurationFromStorage : IReturn<ReadConfigurationFromStorageResponse>
        {
            //  public string Name { get; set; }
        }
        */
}
