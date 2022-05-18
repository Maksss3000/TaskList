using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TaskList.Models
{
    public class TaskListDbContext : DbContext
    {
        public TaskListDbContext(DbContextOptions<TaskListDbContext> options) : base(options)
        {


        }
        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<Priority> Priorities { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserAssignment> UserAssignments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            
            modelBuilder.Entity<Assignment>().HasMany(u => u.Users).
                    WithMany(a => a.Assignments).UsingEntity<UserAssignment>(j => j.HasOne(u => u.User).WithMany(u => u.UserAssignments),
                     j => j.HasOne(a => a.Assignment).WithMany(u => u.UserAssignments));

        }
    }

}
