using Microsoft.EntityFrameworkCore;
using TugasAssigment.Model;

namespace TugasAssigment.Context
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> departments { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Employee>()
        //       .HasOne(a => a.department)
        //       .WithOne(b => b.employees)
        //       .HasForeignKey<Employee>(b => b.NIK)
        //       .IsRequired();
        //}
    }
}
