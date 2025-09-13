using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class GestorTurnosContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Professional> Professionals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Study> Studies { get; set; }

        public GestorTurnosContext(DbContextOptions<GestorTurnosContext> options) : base(options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //}
    }
}
