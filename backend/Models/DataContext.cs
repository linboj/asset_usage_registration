using Microsoft.EntityFrameworkCore;

namespace Backend.Models;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Asset> Assets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Salt> Salts { get; set; }
    public DbSet<Usage> Usages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User to Roles relationship
        modelBuilder.Entity<Role>()
            .HasOne(r => r.User)
            .WithMany(u => u.Roles)
            .HasForeignKey(r => r.UserId);

        // User to Usages relationship
        modelBuilder.Entity<Usage>()
            .HasOne(u => u.User)
            .WithMany(u => u.Usages)
            .HasForeignKey(u => u.UserId);

        // Asset to Usages relationship
        modelBuilder.Entity<Asset>()
            .HasMany(a => a.Usages)
            .WithOne(u => u.Asset)
            .HasForeignKey(t => t.AssetId);

        // User to Salt relationship
        modelBuilder.Entity<User>()
            .HasOne(u => u.Salt)
            .WithOne(s => s.User)
            .HasForeignKey<Salt>(s => s.UserId);
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity entity)
            {
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow; // Set CreatedAt when the entity is added
                    entity.UpdatedAt = entity.CreatedAt; // Set UpdatedAt as same as CreatedAt when the entity is added
                }

                if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow; // Set UpdatedAt when the entity is modified
                }
            }
        }

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity entity)
            {
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;  // Set CreatedAt when the entity is added
                    entity.UpdatedAt = entity.CreatedAt; // Set UpdatedAt as same as CreatedAt when the entity is added
                }

                if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;  // Set UpdatedAt when the entity is modified
                }
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}