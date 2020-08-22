using StartPage.Models;
using Microsoft.EntityFrameworkCore;

namespace StartPage.Framework
{
    public class StartPageContext : DbContext
    {
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<User> Users { get; set; }

        private static bool _created = false;
        public StartPageContext(DbContextOptions<StartPageContext> options) : base(options)
        {
            if (!_created)
            {
                _created = true;
                Database.EnsureCreated();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(x => x.Id);
                user.HasIndex(x => x.Username).IsUnique();

                user.Property(x => x.Id).ValueGeneratedOnAdd();
                user.Property(x => x.Username).IsRequired();
                user.Property(x => x.EmailAddress).IsRequired();
                user.Property(x => x.Password).IsRequired();

            });
            modelBuilder.Entity<Bookmark>(bookmark => 
            {
                bookmark.HasKey(x => x.Id);

                bookmark.Property(x => x.Id).ValueGeneratedOnAdd();
                bookmark.Property(x => x.Url).IsRequired();
            });
        }
    }
}