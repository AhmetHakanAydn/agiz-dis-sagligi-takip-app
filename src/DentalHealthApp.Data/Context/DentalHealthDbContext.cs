using DentalHealthApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DentalHealthApp.Data.Context;

public class DentalHealthDbContext : DbContext
{
    public DentalHealthDbContext(DbContextOptions<DentalHealthDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<Recommendation> Recommendations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
        });

        // Configure Goal entity
        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Period).HasConversion<int>();
            entity.Property(e => e.ImportanceLevel).HasConversion<int>();
            
            entity.HasOne(d => d.User)
                .WithMany(p => p.Goals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Activity entity
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.ImagePath).HasMaxLength(500);
            
            entity.HasOne(d => d.User)
                .WithMany(p => p.Activities)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(d => d.Goal)
                .WithMany(p => p.Activities)
                .HasForeignKey(d => d.GoalId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Note entity
        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
            
            entity.HasOne(d => d.User)
                .WithMany(p => p.Notes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Recommendation entity
        modelBuilder.Entity<Recommendation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Category).HasMaxLength(500);
        });

        // Seed data for recommendations
        modelBuilder.Entity<Recommendation>().HasData(
            new Recommendation { Id = 1, Title = "Günlük Diş Fırçalama", Content = "Günde en az 2 kez, sabah ve akşam diş fırçalayın. Florürlü diş macunu kullanmayı unutmayın.", Category = "Günlük Bakım" },
            new Recommendation { Id = 2, Title = "Diş İpi Kullanımı", Content = "Her gün diş ipi kullanarak diş aralarındaki plakları temizleyin. Bu, diş eti hastalıklarını önlemeye yardımcı olur.", Category = "Günlük Bakım" },
            new Recommendation { Id = 3, Title = "Ağız Gargarası", Content = "Antibakteriyel ağız gargarası kullanarak bakterileri azaltın ve nefes ferahlatın.", Category = "Günlük Bakım" },
            new Recommendation { Id = 4, Title = "Şekerli İçecekleri Sınırlayın", Content = "Şekerli ve asitli içecekleri sınırlayın. Su içmeyi tercih edin.", Category = "Beslenme" },
            new Recommendation { Id = 5, Title = "Düzenli Diş Hekimi Kontrolü", Content = "6 ayda bir diş hekimi kontrolünden geçin. Erken teşhis önemlidir.", Category = "Profesyonel Bakım" },
            new Recommendation { Id = 6, Title = "Doğru Fırçalama Tekniği", Content = "Dişlerinizi dairesel hareketlerle, nazikçe fırçalayın. Sert fırçalama diş etlerinize zarar verebilir.", Category = "Teknik" },
            new Recommendation { Id = 7, Title = "Tütün ve Alkol Kullanımından Kaçının", Content = "Tütün ve aşırı alkol kullanımı ağız kanserine ve diş eti hastalıklarına neden olabilir.", Category = "Yaşam Tarzı" },
            new Recommendation { Id = 8, Title = "Kalsiyum ve D Vitamini", Content = "Diş ve kemik sağlığı için kalsiyum ve D vitamini açısından zengin besinler tüketin.", Category = "Beslenme" }
        );
    }
}