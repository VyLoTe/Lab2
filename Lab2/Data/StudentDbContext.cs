using Lab2.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Major> Majors { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable(nameof(Student));
            modelBuilder.Entity<Major>().ToTable(nameof(Major));
        }
    }
}
