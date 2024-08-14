using BookApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApp.DbContextManagar
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)  // each Book has one category
                .WithMany(c => c.Books)  // each catogory has many Books
                .HasForeignKey(p => p.CategoryId); // the forigen key on Books is ......

            modelBuilder.Entity<Book>()
                .Property(p => p.Description)
                .IsRequired(false)
                .HasMaxLength(250);

            modelBuilder.Entity<Book>()
                .Property(p => p.Date)
                .IsRequired(true);

            modelBuilder.Entity<Book>()
                .Property(p => p.Name)
                .IsRequired(true)
                .HasMaxLength(70);


            

        }

        public DbSet<Book> books { get; set; }
        public DbSet<Category> categorys {  get; set; }
        public DbSet<ToRead> toRead { get; set;}
        
    }
}
