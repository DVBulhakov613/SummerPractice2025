using Microsoft.EntityFrameworkCore;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Services;
using Class_Lib.Backend.Package_related;

namespace Class_Lib
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region DbSets
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<BaseLocation> Locations { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<PostalOffice> PostalOffices { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
            public DbSet<Package> Packages { get; set; }
                public DbSet<Content> Contents { get; set; }
                public DbSet<PackageEvent> PackageEvents { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            PopulatePermissions(modelBuilder);

            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Client>().ToTable("Clients");

            modelBuilder.Entity<BaseLocation>().ToTable("Locations");
            modelBuilder.Entity<Warehouse>().ToTable("Warehouses");
            modelBuilder.Entity<PostalOffice>().ToTable("PostalOffices");

            ConfigureRolePermissions(modelBuilder);
            ConfigureBaseLocation(modelBuilder);
            ConfigureWarehouse(modelBuilder);
            ConfigureDelivery(modelBuilder);
            ConfigurePackage(modelBuilder);
            ConfigurePackageEvent(modelBuilder);
            ConfigureContent(modelBuilder);
            ConfigureEmployee(modelBuilder);
            ConfigureUser(modelBuilder);
            ConfigureClient(modelBuilder);
        }

        private static void PopulatePermissions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().HasData(
                            Enum.GetValues(typeof(AccessService.PermissionKey))
                                .Cast<AccessService.PermissionKey>()
                                .Select(p => new Permission
                                {
                                    ID = (uint)p,
                                    Name = p.ToString()
                                }).ToArray()
                        );
        }

        private static void ConfigureClient(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>() // defines the primary key for the client table (and in general for the person table)
                            .HasKey(p => p.ID);
            modelBuilder.Entity<Client>() // auto-increment for ID
                .Property(p => p.ID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Client>() // restricts the length of the PhoneNumber property to 15 characters
                .Property(p => p.PhoneNumber)
                .HasMaxLength(15);
        }

        private static void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                            .HasKey(u => u.ID);
            modelBuilder.Entity<User>() // auto-increment for ID
                .Property(p => p.ID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            modelBuilder.Entity<User>()
                .Property(u => u.RoleID)
                .IsRequired(false); // make RoleID nullable in the database

            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<User>(u => u.PersonID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleID)
                .OnDelete(DeleteBehavior.SetNull); // when Role is deleted, set User.RoleID = null
            modelBuilder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<User>(u => u.PersonID)
                .OnDelete(DeleteBehavior.Cascade); // when Employee is deleted, delete User
        }

        private static void ConfigureEmployee(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>() // defines the primary key for the client table (and in general for the person table)
                            .HasKey(p => p.ID);
            modelBuilder.Entity<Employee>() // auto-increment for ID
                .Property(p => p.ID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Employee>() // restricts the length of the PhoneNumber property to 15 characters
                .Property(p => p.PhoneNumber)
                .HasMaxLength(15);
            modelBuilder.Entity<Employee>() // SHOULD restrict deleting a location if it has employees assigned to it
                .HasOne(e => e.Workplace)
                .WithMany(po => po.Staff)
                .HasForeignKey(e => e.WorkplaceID)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false); // not all employees have a workplace
        }

        private static void ConfigureContent(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Content>()
                            .HasKey(c => new { c.Name, c.PackageID }); // composite key for content
            modelBuilder.Entity<Content>() // deletes the contents of the package on package removal
                .HasOne(c => c.Package)
                .WithMany(p => p.DeclaredContent)
                .HasForeignKey(c => c.PackageID)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigurePackageEvent(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PackageEvent>() // declares the PackageEvent key as a composite key between the Timestamp and PackageID
                            .HasKey(pe => new { pe.Timestamp, pe.PackageID });
            modelBuilder.Entity<PackageEvent>()
                .HasOne(e => e.Package)
                .WithMany()
                .HasForeignKey(e => e.PackageID)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigurePackage(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Package>() // primary key definition
                            .HasKey(p => p.ID);
            modelBuilder.Entity<Package>() // auto increment for package ID
                .Property(p => p.ID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Package>() // assigns a default value to the PackageStatus property
                .Property(p => p.PackageStatus)
                .HasDefaultValue(PackageStatus.STORED);

            modelBuilder.Entity<Package>()
                .Property(p => p.RowVersion)
                .IsRowVersion(); // marks it as a row version column for concurrency control
        }

        private static void ConfigureDelivery(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Delivery>() // pk - ID field (uint)
                            .HasKey(d => d.ID);
            modelBuilder.Entity<Delivery>() // auto increment for package ID
                .Property(p => p.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Delivery>() // cascade delete when package is deleted
                .HasOne(d => d.Package)
                .WithOne(p => p.Delivery)
                .HasForeignKey<Delivery>(d => d.PackageID)
                .OnDelete(DeleteBehavior.Cascade);

            // restrict deletion of a delivery when the sender or receiver or starting point or destination is deleted
            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Sender)
                .WithMany(c => c.DeliveriesSent)
                .HasForeignKey(d => d.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.Receiver)
                .WithMany(c => c.DeliveriesReceived)
                .HasForeignKey(d => d.ReceiverID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.SentFrom)
                .WithMany(w => w.DeliveriesSentFromHere)
                .HasForeignKey(d => d.SentFromID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.SentTo)
                .WithMany(w => w.DeliveriesSentHere)
                .HasForeignKey(d => d.SentToID)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureWarehouse(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Warehouse>()
                            .HasMany(po => po.StoredPackages)
                            .WithOne()
                            .HasForeignKey("StoredInWarehouseID")
                            .OnDelete(DeleteBehavior.NoAction);
        }

        private static void ConfigureBaseLocation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseLocation>() // ID as primary key
                            .HasKey(l => l.ID);
            modelBuilder.Entity<BaseLocation>() // auto increment for ID
                .Property(l => l.ID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<BaseLocation>() // restricts deleting a location if it has employees assigned to it
                .HasMany(l => l.Staff)
                .WithOne(e => e.Workplace)
                .HasForeignKey(e => e.WorkplaceID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<BaseLocation>()
                .OwnsOne(l => l.GeoData,
                    nav =>
                    {
                        nav.Property(c => c.Latitude).HasColumnName("Latitude");
                        nav.Property(c => c.Longitude).HasColumnName("Longitude");
                        nav.Property(c => c.Address).HasColumnName("Address").HasMaxLength(255);
                        nav.Property(c => c.Region).HasColumnName("Region").IsRequired().HasMaxLength(100);
                    });
        }

        private static void ConfigureRolePermissions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>() // auto increment for ID
                            .Property(l => l.ID)
                            .ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>() // should cascade RolePermission deletion when role is deleted
                .HasMany(r => r.RolePermissions)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Role>() // should null User references when role is deleted
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleID)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleID, rp.PermissionID });
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleID);
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionID);
        }
    }
}
