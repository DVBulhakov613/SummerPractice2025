using Microsoft.EntityFrameworkCore;
using Class_Lib.Backend.Delivery_vehicles;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;

namespace Class_Lib
{
    public class AppDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<DeliveryVehicle> DeliveryVehicles { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<PackageEvent> PackageEvents { get; set; }
        public DbSet<BaseLocation> Locations { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var projectDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\");
            var dbPath = System.IO.Path.Combine(projectDirectory, "app.db");
            System.Diagnostics.Debug.WriteLine($"Using database at: {dbPath}"); // debug line
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // initial data seeding
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country ("Ukraine", "UA"),
                new Country ("Poland", "PL"),
                new Country ("Romania", "RO"));


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
            modelBuilder.Entity<Warehouse>() // restricts from deleting when there are packages
                .HasMany(po => po.StoredPackages)
                .WithOne(p => p.SentFrom)
                .HasForeignKey(p => p.SentFromID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Warehouse>()
                .HasDiscriminator<string>("WarehouseType")
                .HasValue<Warehouse>("Warehouse")
                .HasValue<PostalOffice>("PostalOffice");
            #endregion

            #region package table specifications
            modelBuilder.Entity<Package>() // primary key definition
                .HasKey(p => p.ID);
            modelBuilder.Entity<Package>() // auto increment for package ID
                .Property(p => p.ID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Package>() // restricts deleting a package if its stored in a fixed location
                .HasOne(p => p.SentFrom)
                .WithMany(po => po.StoredPackages)
                .HasForeignKey(p => p.SentFromID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Package>() // restricts deleting a package if it has a destination
                .HasOne(p => p.SentTo)
                .WithMany()
                .HasForeignKey(p => p.SentToID)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Package>() // assigns a default value to the PackageStatus property
                .Property(p => p.PackageStatus)
                .HasDefaultValue(PackageStatus.STORED);
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

            #region delivery vehicle table specifications
            modelBuilder.Entity<DeliveryVehicle>() // defines the primary key for the delivery vehicle table
                .HasKey(dv => dv.ID);
            modelBuilder.Entity<DeliveryVehicle>() // auto-increment for ID
                .Property(dv => dv.ID)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<DeliveryVehicle>() // restricts removal of delivery vehicles if they have packages
                .HasMany(dv => dv.StoredPackages)
                .WithOne()
                .HasForeignKey(p => p.ID)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            #region coordinates table specifications
            modelBuilder.Entity<Coordinates>() // defines the composite key for the Coordinates table (if i have one)
                .HasKey(c => new {c.Longitude, c.Latitude });
            #endregion

            #region country table specifications
            modelBuilder.Entity<Country>() // defines the primary key for the country table
                .HasKey(c => c.ISO2Code);
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
                .OnDelete(DeleteBehavior.Restrict);
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
