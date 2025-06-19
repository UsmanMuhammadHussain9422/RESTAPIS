using FirstAPI.Models;
using FirstAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FirstAPI.Data
{
    public class FirstAPIContext: DbContext
    {
        public FirstAPIContext(DbContextOptions<FirstAPIContext> dbContextOptions):base(dbContextOptions) { }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().HasData(BookDataGenerator.GenerateBooks());
        }
    }
}
