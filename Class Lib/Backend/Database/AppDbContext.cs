using Microsoft.EntityFrameworkCore;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;

namespace Class_Lib
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<BaseLocation> Locations { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<PostalOffice> PostalOffices { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<PackageEvent> PackageEvents { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Manager>().ToTable("Managers");
            modelBuilder.Entity<Administrator>().ToTable("Administrators");
            modelBuilder.Entity<Client>().ToTable("Clients");


            modelBuilder.Entity<BaseLocation>().ToTable("Locations");
            modelBuilder.Entity<Warehouse>().ToTable("Warehouses");
            modelBuilder.Entity<PostalOffice>().ToTable("PostalOffices");

            #region base location table specifications
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
            #endregion

            // since postal office is also a warehouse
            #region warehouse table specifications
            modelBuilder.Entity<Warehouse>()
                .HasMany(po => po.PackagesSentFromHere)
                .WithOne(p => p.SentFrom)
                .HasForeignKey(p => p.SentFromID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Warehouse>()
                .HasMany(po => po.PackagesSentToHere)
                .WithOne(p => p.SentTo)
                .HasForeignKey(p => p.SentToID)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region package table specifications
            modelBuilder.Entity<Package>() // primary key definition
                .HasKey(p => p.ID);
            modelBuilder.Entity<Package>() // auto increment for package ID
                .Property(p => p.ID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Package>() // assigns a default value to the PackageStatus property
                .Property(p => p.PackageStatus)
                .HasDefaultValue(PackageStatus.STORED);
            // sender relationship
            modelBuilder.Entity<Package>()
                .HasOne(p => p.Sender) // each Package has one Sender
                .WithMany(c => c.PackagesSent) // each Client can send many Packages
                .HasForeignKey(p => p.SenderID) // foreign key in the Package table
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade delete to avoid deleting clients when packages are deleted
            // receiver relationship
            modelBuilder.Entity<Package>()
                .HasOne(p => p.Receiver) // each Package has one Receiver
                .WithMany(c => c.PackagesReceived) // each Client can receive many Packages
                .HasForeignKey(p => p.ReceiverID) // foreign key in the Package table
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade delete to avoid deleting clients when packages are deleted
            modelBuilder.Entity<Package>()
                .HasOne(p => p.CurrentLocation)
                .WithMany()
                .HasForeignKey(p => p.CurrentLocationID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            modelBuilder.Entity<Package>()
                .Property(p => p.RowVersion)
                .IsRowVersion(); // marks it as a row version column for concurrency control

            #endregion

            #region packageEvent table specifications
            modelBuilder.Entity<PackageEvent>() // declares the PackageEvent key as a composite key between the Timestamp and PackageID
                .HasKey(pe => new { pe.Timestamp, pe.PackageID });
            modelBuilder.Entity<PackageEvent>() // cascade deletes associated package events if a package is deleted
                .HasOne(pe => pe.Package) 
                .WithMany(p => p.Log)
                .HasForeignKey(pe => pe.PackageID)
                .OnDelete(DeleteBehavior.Cascade); // if a package is deleted, all its events are deleted too
            #endregion

            #region content table specification
            modelBuilder.Entity<Content>()
                .HasKey(c => new { c.Name, c.PackageID }); // composite key for content
            modelBuilder.Entity<Content>() // deletes the contents of the package on package removal
                .HasOne(c => c.Package)
                .WithMany(p => p.DeclaredContent)
                .HasForeignKey(c => c.PackageID)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region coordinates table specifications
            modelBuilder.Entity<Coordinates>() // defines the composite key for the Coordinates table (if i have one)
                .HasKey(c => new {c.Longitude, c.Latitude });
            #endregion

            #region country table specifications

            #endregion

            #region employee table specifications
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
                .IsRequired(false);
            modelBuilder.Entity<Employee>() // ties user data to employee objects
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<User>(u => u.PersonID);
            #endregion

            // manager table specifications
            modelBuilder.Entity<Manager>() // has many managed locations with one manager, restrict deletion
                .HasMany(m => m.ManagedLocations)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            #region user table
            modelBuilder.Entity<User>()
                .HasKey(u => u.PersonID); // PersonID as the PK
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username) // unique index for username
                .IsUnique();
            #endregion

            #region client table specifications
            modelBuilder.Entity<Client>() // defines the primary key for the client table (and in general for the person table)
                .HasKey(p => p.ID);
            modelBuilder.Entity<Client>() // auto-increment for ID
                .Property(p => p.ID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Client>() // restricts the length of the PhoneNumber property to 15 characters
                .Property(p => p.PhoneNumber)
                .HasMaxLength(15);
            
            #endregion
        }
    }
}
