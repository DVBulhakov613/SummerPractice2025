namespace Class_Lib.Backend.Services
{
    /// <summary>
    /// Represents a role in the system.
    /// </summary>
    public class Role
    {
        public uint ID { get; set; }
        public string Name { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
