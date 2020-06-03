using Microsoft.EntityFrameworkCore;

namespace ChaVoV1.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Category>().HasData(
                new Category(){
                    Id = 1,
                    CategoryText = "Coding"
                },
                new Category(){
                    Id = 2,
                    CategoryText = "Cooking"
                },
                new Category(){
                    Id = 3,
                    CategoryText = "Driving"
                });
        }
    }
}