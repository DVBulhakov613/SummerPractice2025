namespace Class_Lib.Backend.Services
{
    /// <summary>
    /// Represents a permission in the system.
    /// </summary>
    public class  Permission
    {
        public uint ID { get; set; }
        public string Name { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
