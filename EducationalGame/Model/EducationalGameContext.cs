using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalGame.Model
{
   public class EducationalGameContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Principal> Principals { get; set; }
        public DbSet<GameQuiz> GameQuizes { get; set; }
        public DbSet<QuizCategory> QuizCategories { get; set; }
        public DbSet<StudentGameQuiz> StudentGameQuizes { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=EducationalGameDB;Trusted_Connection=True");

            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            int counter = 1;
            modelBuilder.Entity<StudentGameQuiz>().HasKey(sgq => new {sgq.StudentId, sgq.GameQuizId});

            modelBuilder.Entity<QuizCategory>().HasData(
            new QuizCategory { Id = 1, QuizCategoryName = "Човекът и заобикалящия го свят" },
            new QuizCategory { Id = 2, QuizCategoryName = "Нашата красива България" },
            new QuizCategory { Id = 3, QuizCategoryName = "България в Средновековна Европа" }
         );


            string[] line1 = File.ReadAllLines(@"Resources\Човекът и заобикалящия го свят.txt");
            string[] line2 = File.ReadAllLines(@"Resources\Нашата красива България.txt");
            string[] line3 = File.ReadAllLines(@"Resources\България в Средновековна Европа.txt");


            for (int i = 0; i < line1.Length; i = i + 3)
            {
                modelBuilder.Entity<GameQuiz>().HasData(
                new GameQuiz { Id = counter, Question = line1[i], Answer = line1[i + 1], QuizCategoryId = 1 });
                counter++;
            }


            for (int i = 0; i < line2.Length; i = i + 3)
            {
                modelBuilder.Entity<GameQuiz>().HasData(
                new GameQuiz { Id = counter, Question = line2[i], Answer = line2[i + 1], QuizCategoryId = 2 });
                counter++;
            }

            for (int i = 0; i < line3.Length; i = i + 3)
            {
                modelBuilder.Entity<GameQuiz>().HasData(
                new GameQuiz { Id = counter, Question = line3[i], Answer = line3[i + 1], QuizCategoryId = 3 });
                counter++;
            }
        }


   }


}
