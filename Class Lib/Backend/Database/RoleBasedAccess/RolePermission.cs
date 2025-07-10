namespace Class_Lib.Backend.Services
{
    /// <summary>
    /// Represents a many-to-many relationship between roles and permissions.
    /// </summary>
    public class RolePermission
    {
        public uint RoleID { get; set; }
        public uint PermissionID { get; set; }

        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}
