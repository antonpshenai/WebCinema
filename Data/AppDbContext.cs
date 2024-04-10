using ITExpertTEST.Models;
using Microsoft.EntityFrameworkCore;


namespace ITExpertTEST.Data
{

    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<FilmCategory> FilmCategories { get; set; }
        public DbSet<Film> Films { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FilmCategory>()
                .HasKey(fc => fc.FilmCategoryId);

            modelBuilder.Entity<FilmCategory>()
                .HasOne(fc => fc.Film)
                .WithMany(f => f.FilmCategories)
                .HasForeignKey(fc => fc.FilmId);

            modelBuilder.Entity<FilmCategory>()
                .HasOne(fc => fc.Category)
                .WithMany(c => c.FilmCategories)
                .HasForeignKey(fc => fc.CategoryId);
        }
    }
}
