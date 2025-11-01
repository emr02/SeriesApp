using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APIseries.Models.EntityFramework
{
    public partial class SeriesDbContext : DbContext
    {
        public SeriesDbContext()
        {
        }

        public SeriesDbContext(DbContextOptions<SeriesDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Serie> Series { get; set; }
        public virtual DbSet<Utilisateur> Utilisateurs { get; set; }
        public virtual DbSet<Notation> Notations { get; set; }

        public static readonly ILoggerFactory MyLoggerFactory =
            LoggerFactory.Create(builder => builder.AddConsole());

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning Déplace la chaîne de connexion dans appsettings.json pour plus de sécurité
//            => optionsBuilder
//                .UseLoggerFactory(MyLoggerFactory)
//                .EnableSensitiveDataLogging()
//                .UseNpgsql("Server=localhost;Port=5432;Database=APIseries;uid=postgres; password=postgres;")

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            // Serie
            modelBuilder.Entity<Serie>(entity =>
            {
                entity.HasKey(e => e.SerieId).HasName("pk_ser");

                entity.HasIndex(e => e.Titre)
                    .HasDatabaseName("ix_t_e_serie_ser_serie");
            });

            // Utilisateur
            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.HasKey(e => e.UtilisateurId).HasName("pk_utl");

                entity.HasIndex(e => e.Mail)
                    .IsUnique()
                    .HasDatabaseName("uq_utl_mail");

                entity.Property(e => e.Pays)
                    .HasDefaultValue("France");

                entity.Property(e => e.DateCreation)
                    .HasDefaultValueSql("now()");
            });

            // Notation
            modelBuilder.Entity<Notation>(entity =>
            {
                entity.HasKey(e => new { e.UtilisateurId, e.SerieId }).HasName("pk_not");

                entity.HasOne(d => d.UtilisateurNotant)
                    .WithMany(p => p.NotesUtilisateur)
                    .HasForeignKey(d => d.UtilisateurId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_not_utl");

                entity.HasOne(d => d.SerieNotee)
                    .WithMany(p => p.NotesSerie)
                    .HasForeignKey(d => d.SerieId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("fk_not_ser");

                // Index explicitement nommé (sinon EF crée IX_t_j_notation_not_ser_id)
                entity.HasIndex(n => n.SerieId)
                      .HasDatabaseName("IX_t_j_notation_not_ser_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}