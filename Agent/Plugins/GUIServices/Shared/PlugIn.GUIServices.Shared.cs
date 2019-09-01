using System;
using System.Collections.Generic;

namespace Agent.GUIServices.Shared
{
    #region GUIMap
    public interface IGUIMap {
        public string RelativeToContentRootPath { get; set; }
        public string VirtualRootPath { get; set; }
    }
    public class GUIMap : IGUIMap {
        public string RelativeToContentRootPath { get; set; }
        public string VirtualRootPath { get; set; }
    }
    #endregion
    #region GUIMaps
    // ToDo: Figure out why SS will not load from appsettings text file if the property is declared as IEnumerable<IGUIMap>
    public interface IGUIMaps {
        public IEnumerable<GUIMap> _GUIMaps { get; set; }
    }
    public class GUIMaps : IGUIMaps {
        public IEnumerable<GUIMap> _GUIMaps { get; set; }
    }
    #endregion
    #region GUI
    public interface IGUI {
        public string Handle { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public IGUIMap GUIMap { get; set; }
    }
    public class GUI : IGUI {
        public string Handle { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public IGUIMap GUIMap { get; set; }
    }
    #endregion

    #region GUIS
    public interface IGUIS {
        public List<GUI> GUIs { get; set; }
    }
    public class GUIS : IGUIS {
        public List<GUI> GUIs { get; set; }
    }
    #endregion

    #region ConfigurationData
    public interface IConfigurationData {
        public GUIS GUIS { get; set; }
    }
    public class ConfigurationData {
        public GUIS GUIS { get; set; }
    }
    #endregion

    #region UserData
    public class UserData
    {
        public UserData() : this(string.Empty) { }
        public UserData(string placeholder)
        {
            Placeholder = placeholder;
        }

        public string Placeholder { get; set; }
    }
    #endregion UserData
}