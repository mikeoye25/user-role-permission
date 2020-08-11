using System.ComponentModel;

namespace Imprinno.Models.Enums
{
    public enum PermEnums
    {
        //[Description("All")]
        //All = 0,
        //[Description("None")]
        //None = 1,

        #region User

        [Description("Create User")]
        CreateUser = 2,
        [Description("Read User")]
        ReadUser = 3,
        [Description("Update User")]
        UpdateUser = 4,
        [Description("Delete User")]
        DeleteUser = 5,

        #endregion

        #region Role

        [Description("Create Role")]
        CreateRole = 6,
        [Description("Read Role")]
        ReadRole = 7,
        [Description("Update Role")]
        UpdateRole = 8,
        [Description("Delete Role")]
        DeleteRole = 9,

        #endregion

        #region Permission

        [Description("Read Permission")]
        ReadPermission = 10,

        #endregion

        #region Message

        [Description("Send Message")]
        SendMessage = 11,
        [Description("Read Message")]
        ReadMessage = 12,

        #endregion
    }
}
