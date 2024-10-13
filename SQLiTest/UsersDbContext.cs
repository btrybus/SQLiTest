using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SQLiTest
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext()
        {
        }

        public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Uzytkownik> Uzytkownicy { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Users;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Uzytkownik>(entity =>
            {
                entity.HasKey(e => e.Login)
                    .HasName("PK__Uzytkown__5E55825AFFDF9CBD");

                entity.ToTable("Uzytkownicy");

                entity.Property(e => e.Login).HasMaxLength(50);

                entity.Property(e => e.Haslo).HasMaxLength(50);

                entity.Property(e => e.Uprawnienia)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
            });          
        }
       
    }

}
