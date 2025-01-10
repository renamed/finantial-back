using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using R3M.Finantial.Backend.Model;

namespace R3M.Finantial.Backend.Context;

public class FinantialContext : DbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Institution> Institutions { get; set; }
    public DbSet<Period> Periods { get; set; }
    public DbSet<Movimentation> Movimentations { get; set; }

    public FinantialContext(DbContextOptions<FinantialContext> options)
        : base(options)
    {
        
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(e =>
        {
            ConfigId(e);

            e.Property(p => p.Name)
                .HasMaxLength(Constants.CategoryNameMaxLength)
                .IsRequired();

            e.HasIndex(i => new { i.ParentId, i.Name })
                .IsUnique(true);
        });

        modelBuilder.Entity<Institution>(e =>
        {
            ConfigId(e);

            e.Property(p => p.Name)
                .HasMaxLength(Constants.InstitutionNameMaxLength)
                .IsRequired();

            e.HasIndex(i => i.Name)
                .IsUnique(true);
        });

        modelBuilder.Entity<Period>(e =>
        {
            ConfigId(e);

            e.Property(p => p.Description)
                .HasMaxLength(Constants.PeriodDescriptionLength)
                .IsFixedLength(true);

            e.HasIndex(i => new { i.InitialDate, i.FinalDate })
                .IsDescending(false, false);

            e.HasIndex(i => i.Description)
                .IsUnique(true);
        });

        modelBuilder.Entity<Movimentation>(e =>
        {
            ConfigId(e);

            e.Property(p => p.Description)
                .HasMaxLength(Constants.MovimentationDescriptionMaxLength);

            e.HasOne(i => i.Period)
                .WithMany()
                .HasForeignKey(f => f.PeriodId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(i => i.Institution)
                .WithMany()
                .HasForeignKey(f => f.InstitutionId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(i => i.Category)
                .WithMany()
                .HasForeignKey(f => f.CategoryId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);
        });

        base.OnModelCreating(modelBuilder);
    }

    private static void ConfigId<T>(EntityTypeBuilder<T> e)
        where T : Register
    {
        e.HasKey(k => k.Id);
        e.Property(p => p.Id).ValueGeneratedOnAdd();
    }


    public override int SaveChanges()
    {
        SetDates();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        SetDates();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SetDates();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetDates();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetDates()
    {
        foreach(var record in base.ChangeTracker
            .Entries()
            .Where(x => x.State == EntityState.Added 
                    || x.State == EntityState.Modified))
        {
            if (record.Entity is Register register)
            {
                if (record.State == EntityState.Added)
                {
                    register.InsertedAtUtc = DateTime.UtcNow;
                }
                else if (record.State == EntityState.Modified)
                {
                    register.UpdatedAtUtc = DateTime.UtcNow;
                }
            }            
        }
    }
}
