using DriveOps.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DriveOps.Api.Data;

public class DriveOpsContext(DbContextOptions<DriveOpsContext> options)
    : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<IndividualCustomer> IndividualCustomers { get; set; }
    public DbSet<CompanyCustomer> CompanyCustomers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleOwnership> VehicleOwnerships { get; set; }
    public DbSet<Technician> Technicians { get; set; }
    public DbSet<JobOrder> JobOrders { get; set; }
    public DbSet<ReportedIssue> ReportedIssues { get; set; }
    public DbSet<InspectionFinding> InspectionFindings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureCustomer(modelBuilder);
        ConfigureIndividualCustomer(modelBuilder);
        ConfigureCompanyCustomer(modelBuilder);
        ConfigureVehicle(modelBuilder);
        ConfigureVehicleOwnership(modelBuilder);
        ConfigureTechnician(modelBuilder);
        ConfigureJobOrder(modelBuilder);
        ConfigureReportedIssue(modelBuilder);
        ConfigureInspectionFinding(modelBuilder);
    }

    private static void ConfigureCustomer(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Type)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(c => c.Address)
                .HasMaxLength(512);

            entity.Property(c => c.Notes)
                .HasMaxLength(1024);

            entity.Property(c => c.Status)
                .IsRequired()
                .HasConversion<string>();

            entity.HasOne(c => c.IndividualCustomer)
                .WithOne(ic => ic.Customer)
                .HasForeignKey<IndividualCustomer>(ic => ic.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.CompanyCustomer)
                .WithOne(cc => cc.Customer)
                .HasForeignKey<CompanyCustomer>(cc => cc.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureIndividualCustomer(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IndividualCustomer>(entity =>
        {
            entity.HasKey(ic => ic.CustomerId);

            entity.Property(ic => ic.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(ic => ic.LastName)
                .IsRequired()
                .HasMaxLength(100);
        });
    }

    private static void ConfigureCompanyCustomer(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompanyCustomer>(entity =>
        {
            entity.HasKey(cc => cc.CustomerId);

            entity.Property(cc => cc.CompanyName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(cc => cc.ContactPerson)
                .HasMaxLength(100);
        });
    }

    public static void ConfigureVehicle(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(v => v.Id);

            entity.Property(v => v.PlateNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(v => v.Make)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(v => v.Model)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(v => v.Year)
                .IsRequired();

            entity.Property(v => v.Vin)
                .IsRequired()
                .HasMaxLength(50);
        });
    }

    private static void ConfigureVehicleOwnership(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleOwnership>(entity =>
        {
            entity.HasKey(vo => vo.Id);

            entity.Property(vo => vo.IsCurrentOwner)
                .IsRequired();

            entity.Property(vo => vo.Notes)
                .HasMaxLength(1024);

            entity.HasOne(vo => vo.Vehicle)
                .WithMany(v => v.VehicleOwnerships)
                .HasForeignKey(vo => vo.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(vo => vo.Customer)
                .WithMany(c => c.VehicleOwnerships)
                .HasForeignKey(vo => vo.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public static void ConfigureTechnician(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Technician>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.FullName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(m => m.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(m => m.Specialization)
                .HasMaxLength(100);

            entity.Property(m => m.Status)
                .IsRequired()
                .HasConversion<string>();
        });
    }

    private static void ConfigureJobOrder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobOrder>(entity =>
        {
            entity.HasKey(jo => jo.Id);

            entity.Property(jo => jo.JobOrderNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(jo => jo.Status)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(jo => jo.CustomerId)
                .IsRequired();

            entity.Property(jo => jo.VehicleId)
                .IsRequired();

            entity.Property(jo => jo.TechnicianId)
                .IsRequired();

            entity.Property(jo => jo.CreatedAt)
                .IsRequired();
        });
    }

    private static void ConfigureReportedIssue(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReportedIssue>(entity =>
        {
            entity.HasKey(ri => ri.Id);

            entity.Property(ri => ri.Description)
                .IsRequired()
                .HasMaxLength(1024);

            entity.Property(ri => ri.CreatedAt)
                .IsRequired();

            entity.HasOne(ri => ri.JobOrder)
                .WithMany(jo => jo.Issues)
                .HasForeignKey(ri => ri.JobOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureInspectionFinding(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InspectionFinding>(entity =>
        {
            entity.HasKey(f => f.Id);

            entity.Property(f => f.Description)
                .IsRequired()
                .HasMaxLength(1024);

            entity.Property(f => f.Recommendation)
                .HasMaxLength(1024);

            entity.Property(f => f.Severity)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(f => f.IsResolved)
                .IsRequired();

            entity.Property(f => f.CreatedAt)
                .IsRequired();

            entity.HasOne(f => f.JobOrder)
                .WithMany(jo => jo.Findings)
                .HasForeignKey(f => f.JobOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
