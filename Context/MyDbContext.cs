using MblexApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MblexApp.Context
{
    public class MyDbContext:DbContext
    {
        public DbSet<Question> Questions { get; set; }

       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use the connection string from your Azure SQL server
            optionsBuilder.UseSqlServer("Server=tcp:jtappserver.database.windows.net,1433;Initial Catalog=MblexDB;Persist Security Info=False;User ID=jthuko;Password=Jnzusyo77!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define your model configuration here
        }

       
    }
}
