using System.ComponentModel;

namespace ATAP.Utilities.Database.Enumerations {
    public enum CrudType {
        //ToDo: Add [LocalizedDescription("Create", typeof(Resource))]
        [Description("Create")]
        Create,
        [Description("Replace")]
        Replace,
        [Description("Update")]
        Update,
        [Description("Delete")]
        Delete
    }
}
