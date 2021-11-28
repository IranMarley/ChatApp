using System.Data.Entity;
using ChatApp.Domain.Entities;
using ChatApp.Infra.Data.EntityConfig;

namespace ChatApp.Infra.Data.Context
{
    public class Context : DbContext
    {
        public Context()
            : base("DefaultConnection")
        {
            
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}