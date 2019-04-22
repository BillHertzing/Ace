using System.ComponentModel;

namespace ATAP.Utilities.RealEstate.Enumerations {

    public enum Operation {
        //ToDo: Add [LocalizedDescription("PropertySearch", typeof(Resource))]
        [Description("PropertySearch")]
        PropertySearch,
        [Description("PropertyLastSaleInfo")]
        PropertyLastSaleInfo,
        [Description("PropertyCurrentAgent")]
        PropertyCurrentAgent
    }

}

