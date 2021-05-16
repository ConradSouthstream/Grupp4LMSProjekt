using LMS.Grupp4.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Grupp4.Data
{
    public class ApplicationDbContext : IdentityDbContext<Anvandare,IdentityRole,string>
    {
        public DbSet<Kurs> Kurser { get; set; }
        public DbSet<AnvandareKurs> AnvandareKurser { get; set; }
        public DbSet <Anvandare>  Anvandare{ get; set; }
        public DbSet<Dokument> Dokument { get; set; }
        public DbSet<Modul> Moduler { get; set; }
        public DbSet<DokumentTyp> DokumentTyper { get; set; }
        public  DbSet<Aktivitet> Aktiviteter{ get; set; }

        public DbSet<AktivitetTyp> AktivitetTyper { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AnvandareKurs>().HasKey(a => new { a.AnvandareId, a.KursId });
            builder.Entity<Anvandare>()
                .HasMany(s => s.Kurser)
                .WithMany(c => c.Anvandare)
                .UsingEntity<AnvandareKurs>(
                    e => e.HasOne(e => e.Kurs).WithMany(c => c.AnvandareKurser),
                    e => e.HasOne(e => e.Anvandare).WithMany(s => s.KurserAnvandare));
            builder.Entity<Dokument>().HasOne(d => d.Modul).WithMany(a => a.Dokument)             
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Dokument>().HasOne(d => d.Kurs).WithMany(a => a.Dokument)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Dokument>()
               .Property(d => d.UppladdningsDatum).HasDefaultValueSql("getdate()");
            builder.Entity<Dokument>().HasOne(d => d.Aktivitet).WithMany(a=>a.Dokument)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
