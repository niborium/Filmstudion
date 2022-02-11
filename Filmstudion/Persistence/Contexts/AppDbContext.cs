
using Filmstudion.Models;
using Filmstudion.Models.Film;
using Filmstudion.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Filmstudion
{
    public class AppDbContext : DbContext
    {
        public DbSet<FilmStudio> FilmStudios { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<FilmCopy> FilmCopies { get; set; }
        public DbSet<User> Users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Filmstudios
            builder.Entity<FilmStudio>().HasData(new FilmStudio
            {
                FilmStudioId = 1,
                Name = "Columbia Pictures",
                City = "California"

            });

            builder.Entity<FilmStudio>().HasData(new FilmStudio
            {
                FilmStudioId = 2,
                Name = "Warner Bros. Pictures",
                City = "New York"

            });

            //Users
            builder.Entity<User>().HasData(new User
            {

                Email = "olbin@data.com",
                IsAdmin = true,
                Id = 1,
                Password = "P@ssw0rd!1"
            });

            builder.Entity<User>().HasData(new User
            {

                Email = "rafiq@data.com",
                IsAdmin = true,
                Id = 2,
                Password = "P@ssw0rd!2"
            });
            builder.Entity<User>().HasData(new User
            {
                FilmStudioId = 2,
                Email = "safiq@data.com",
                IsAdmin = false,
                Id = 3,
                Password = "P@ssw0rd!2"
            });

            //Films
            builder.Entity<Film>().HasData(new Film
            {
                FilmId = 1,
                Name = "The Tomorrow War",
                Director = "Chris McKay",
                Country = "USA",
                ReleaseYear = 2021,
                NumberOfCopies = 2
            });

            builder.Entity<Film>().HasData(new Film
            {
                FilmId = 2,
                Name = "For Sama (Till min dotter)",
                Director = "Waad Al-Kateab, Edward Watts",
                Country = "Storbritannien",
                ReleaseYear = 2019,
                NumberOfCopies = 4
            });

            builder.Entity<Film>().HasData(new Film
            {
                FilmId = 3,
                Name = "In the Mood for Love",
                Director = "Wong Kar-Wai",
                Country = "Kina",
                ReleaseYear = 2000,
                NumberOfCopies = 3
            });
            builder.Entity<Film>().HasData(new Film
            {
                FilmId = 4,
                Name = "6 Underground",
                Director = "Michael Bay",
                Country = "USA",
                ReleaseYear = 2019,
                NumberOfCopies = 5
            });



            //Filmcopies
            builder.Entity<FilmCopy>().HasData(new FilmCopy
            {
                FilmCopyId = 1.1,
                FilmId = 1,
                RentedOut = true,
                FilmStudioId = 1
            });

            builder.Entity<FilmCopy>().HasData(new FilmCopy
            {
                FilmCopyId = 1.2,
                FilmId = 1,
                RentedOut = false,
                FilmStudioId = 0
            });

            builder.Entity<FilmCopy>().HasData(new FilmCopy
            {
                FilmCopyId = 2.1,
                FilmId = 2,
                RentedOut = false,
                FilmStudioId = 0
            });
            builder.Entity<FilmCopy>().HasData(new FilmCopy
            {
                FilmCopyId = 2.2,
                FilmId = 2,
                RentedOut = false,
                FilmStudioId = 0
            });
            builder.Entity<FilmCopy>().HasData(new FilmCopy
            {
                FilmCopyId = 2.3,
                FilmId = 2,
                RentedOut = true,
                FilmStudioId = 2
            });
            builder.Entity<FilmCopy>().HasData(new FilmCopy
            {
                FilmCopyId = 2.4,
                FilmId = 2,
                RentedOut = false,
                FilmStudioId = 0
            });

            builder.Entity<FilmCopy>().HasData(new FilmCopy
            {
                FilmCopyId = 3.1,
                FilmId = 3,
                RentedOut = false,
                FilmStudioId = 0
            });
            builder.Entity<FilmCopy>().HasData(new FilmCopy
            {
                FilmCopyId = 3.2,
                FilmId = 3,
                RentedOut = false,
                FilmStudioId = 2
            });
            builder.Entity<FilmCopy>().HasData(new FilmCopy
            {
                FilmCopyId = 3.3,
                FilmId = 3,
                RentedOut = false,
                FilmStudioId = 1
            });

            builder.Entity<FilmCopy>().HasData(new FilmCopy
            {
                FilmCopyId = 4.1,
                FilmId = 4,
                RentedOut = false,
                FilmStudioId = 0
            });
            builder.Entity<FilmCopy>().HasData(new FilmCopy
            {
                FilmCopyId = 4.2,
                FilmId = 4,
                RentedOut = false,
                FilmStudioId = 0
            });
            builder.Entity<FilmCopy>().HasData(new FilmCopy
            {
                FilmCopyId = 4.3,
                FilmId = 4,
                RentedOut = false,
                FilmStudioId = 0
            });

            builder.Entity<FilmCopy>().HasData(new FilmCopy
            {
                FilmCopyId = 4.4,
                FilmId = 4,
                RentedOut = false,
                FilmStudioId = 0
            });
            builder.Entity<FilmCopy>().HasData(new FilmCopy
            {
                FilmCopyId = 4.5,
                FilmId = 4,
                RentedOut = false,
                FilmStudioId = 0
            });


        }
    }
}
