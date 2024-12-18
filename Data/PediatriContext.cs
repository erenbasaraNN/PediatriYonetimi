using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PediatriYonetimi.Models
{
    public class PediatriContext : IdentityDbContext<Kullanici>
    {
        public DbSet<Rol> Roller { get; set; }
        public DbSet<Bolum> Bolumler { get; set; }
        public DbSet<Nobet> Nobetler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<RandevuMusaitlikDurumu> RandevuMusaitlikleri { get; set; }
        public DbSet<Acildurum> AcilDurumlar { get; set; }
        public DbSet<Duyuru> Duyurular { get; set; }

        public PediatriContext(DbContextOptions<PediatriContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Kullanici - Nobet ilişkisi
            modelBuilder.Entity<Nobet>()
                .HasOne(n => n.Asistan)
                .WithMany(u => u.Nobetler)
                .HasForeignKey(n => n.KullaniciId)
                .OnDelete(DeleteBehavior.Restrict);

            // Bölüm - Nobet ilişkisi
            modelBuilder.Entity<Nobet>()
                .HasOne(n => n.Bolum)
                .WithMany(b => b.Nobetler)
                .HasForeignKey(n => n.BolumId)
                .OnDelete(DeleteBehavior.Restrict);

            // Kullanıcı - Randevu Musaitlik İlişkisi (Öğretim Üyesi)
            modelBuilder.Entity<RandevuMusaitlikDurumu>()
                .HasOne(m => m.OgretimUyesi)
                .WithMany()
                .HasForeignKey(m => m.OgretimUyesiId)
                .OnDelete(DeleteBehavior.Restrict);

            // Randevu - RandevuMusaitlikDurumu İlişkisi
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.RandevuMusaitlikDurumu)
                .WithMany(m => m.Randevular)
                .HasForeignKey(r => r.RandevuMusaitlikDurumuId)
                .OnDelete(DeleteBehavior.Restrict);

            // Randevu - Kullanıcı İlişkisi (Asistan)
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Asistan)
                .WithMany()
                .HasForeignKey(r => r.AsistanId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
