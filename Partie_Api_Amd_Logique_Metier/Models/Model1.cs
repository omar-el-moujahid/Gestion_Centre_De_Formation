using System;
using System.Data.Entity;
using System.Linq;

namespace Partie_Api_Amd_Logique_Metier.Models
{
    public class Model1 : DbContext
    {
       
        public Model1()
            : base("name=Model1")
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Formateur> Formateurs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Formation> Formations { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Inscription> Inscriptions { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Payment> Payments { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // User and Role: Many-to-One
            modelBuilder.Entity<User>()
                .HasRequired(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .WillCascadeOnDelete(false);

            // Formation and Category: Many-to-One
            modelBuilder.Entity<Formation>()
                .HasRequired(f => f.Category)
                .WithMany(c => c.Formations)
                .HasForeignKey(f => f.CategoryId)
                .WillCascadeOnDelete(false);

            // Formation and Formateur: Many-to-One
            modelBuilder.Entity<Formation>()
                .HasRequired(f => f.Formateur)
                .WithMany(fm => fm.Formations)
                .HasForeignKey(f => f.FormateurId)
                .WillCascadeOnDelete(false);

            // Formation and Media: One-to-Many
            modelBuilder.Entity<Formation>()
                .HasMany(f => f.Media)
                .WithRequired(m => m.Formation)
                .HasForeignKey(m => m.FormationId);

            // Formation and Certificates: One-to-Many
            modelBuilder.Entity<Formation>()
                .HasMany(f => f.Certificates)
                .WithRequired(c => c.Formation)
                .HasForeignKey(c => c.FormationId);

            // Formation and Evaluation: One-to-Many
            modelBuilder.Entity<Formation>()
                .HasMany(f => f.Evaluations)
                .WithRequired(e => e.Formation)
                .HasForeignKey(e => e.FormationId);

            // Formation and Inscription: One-to-Many
            modelBuilder.Entity<Formation>()
                .HasMany(f => f.Inscriptions)
                .WithRequired(i => i.Formation)
                .HasForeignKey(i => i.FormationId);

            // Formation and Payment: One-to-Many
            modelBuilder.Entity<Formation>()
                .HasMany(f => f.Payment)
                .WithRequired(p => p.Formation)
                .HasForeignKey(p => p.FormationId);

            // Participant and Inscription: One-to-Many
            modelBuilder.Entity<Participant>()
                .HasMany(p => p.Inscriptions)
                .WithRequired(i => i.Participant)
                .HasForeignKey(i => i.ParticipaantId);

            // Participant and Evaluation: One-to-Many
            modelBuilder.Entity<Participant>()
                .HasMany(p => p.Evaluations)
                .WithRequired(e => e.Participant)
                .HasForeignKey(e => e.ParticipantId);

            // Participant and Certificate: One-to-Many
            modelBuilder.Entity<Participant>()
                .HasMany(p => p.Certificates)
                .WithRequired(c => c.Participant)
                .HasForeignKey(c => c.ParticipantId);

            // Participant and Payment: One-to-Many
            modelBuilder.Entity<Participant>()
                .HasMany(p => p.Payments)
                .WithRequired(pmt => pmt.Participant)
                .HasForeignKey(pmt => pmt.ParticipantId);

            // Certificate and Formation: Many-to-One
            modelBuilder.Entity<Certificate>()
                .HasRequired(c => c.Formation)
                .WithMany(f => f.Certificates)
                .HasForeignKey(c => c.FormationId);

            // Certificate and Participant: Many-to-One
            modelBuilder.Entity<Certificate>()
                .HasRequired(c => c.Participant)
                .WithMany(p => p.Certificates)
                .HasForeignKey(c => c.ParticipantId);

            // Evaluation and Formation: Many-to-One
            modelBuilder.Entity<Evaluation>()
                .HasRequired(e => e.Formation)
                .WithMany(f => f.Evaluations)
                .HasForeignKey(e => e.FormationId);

            // Evaluation and Participant: Many-to-One
            modelBuilder.Entity<Evaluation>()
                .HasRequired(e => e.Participant)
                .WithMany(p => p.Evaluations)
                .HasForeignKey(e => e.ParticipantId);

            // Inscription and Formation: Many-to-One
            modelBuilder.Entity<Inscription>()
                .HasRequired(i => i.Formation)
                .WithMany(f => f.Inscriptions)
                .HasForeignKey(i => i.FormationId);

            // Inscription and Participant: Many-to-One
            modelBuilder.Entity<Inscription>()
                .HasRequired(i => i.Participant)
                .WithMany(p => p.Inscriptions)
                .HasForeignKey(i => i.ParticipaantId);

            // Payment and Formation: Many-to-One
            modelBuilder.Entity<Payment>()
                .HasRequired(p => p.Formation)
                .WithMany(f => f.Payment)
                .HasForeignKey(p => p.FormationId);

            // Payment and Participant: Many-to-One
            modelBuilder.Entity<Payment>()
                .HasRequired(p => p.Participant)
                .WithMany(pt => pt.Payments)
                .HasForeignKey(p => p.ParticipantId);

            base.OnModelCreating(modelBuilder);
        }
    }

   
}