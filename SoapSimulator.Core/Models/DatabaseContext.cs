using Microsoft.EntityFrameworkCore;

namespace SoapSimulator.Core.Models;
public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }   
    public virtual DbSet<SystemConfiguration> SystemConfigurations { get; set; }
    public virtual DbSet<SoapAction> SoapActions { get; set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SoapAction>(e =>
        {
            e.HasIndex(e => e.MethodName).IsUnique();
            e.OwnsOne(s => s.Request, f =>
            {
                f.ToTable("RequestFormats");
                f.Property(ff => ff.Id);
                f.WithOwner(ff => ff.Action);
            }); 
            e.OwnsOne(s => s.Response, f2 =>
            {
                f2.ToTable("ResponseFormats");
                f2.Property(ff => ff.Id);
                f2.WithOwner(ff => ff.Action);
            });
        });
        base.OnModelCreating(modelBuilder);
    }
}
