using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudyMaterialShare.Database.Models;

namespace StudyMaterialShare.Database
{
    public class DbInitializer
    {
        private static readonly Faker<Subject> _subjectFaker;
        private static readonly Faker<StudyMaterial> _studyMaterialFaker;
        private static readonly Faker<Rating> _ratingFaker;
        private static readonly Faker<ApplicationUser> _userFaker;

        static DbInitializer()
        {
            _subjectFaker = new Faker<Subject>();
            _studyMaterialFaker = new Faker<StudyMaterial>();
            _ratingFaker = new Faker<Rating>();
            _userFaker = new Faker<ApplicationUser>();

            _subjectFaker.RuleFor(subject => subject.Name,faker => faker.Lorem.Sentence(1,2));

            _userFaker
                .RuleFor(user => user.UserName, faker => faker.Internet.UserName())
                .RuleFor(user => user.DisplayName, faker => faker.Name.FullName())
                .RuleFor(user => user.Email, faker => faker.Internet.Email())
                .RuleFor(user => user.PhoneNumber, faker => faker.Phone.PhoneNumber());

            _studyMaterialFaker
                .RuleFor(sm => sm.Title, faker => faker.Random.Words(faker.Random.Number(1,3)))
                .RuleFor(sm => sm.FileType, "txt")
                .RuleFor(sm => sm.UploadedAt, faker => faker.Date.Past(3))
                .RuleFor(sm => sm.Downloads,faker => faker.Random.Number(0,40));

            _ratingFaker.RuleFor(rating => rating.RateValue, faker => faker.Random.Number(1,5));
        }

        public static async Task Initialize(StudyMaterialShareDbContext context,
            UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager,string sourcePath)
        {
            context.Database.Migrate();
            if (context.Subjects.Any()) return;

            await userManager.CreateAsync(new ApplicationUser()
            {
                UserName = "test",
                DisplayName = "Teszt Elek",
                Email = "test@test.hu",
            }, "test");
            var admin = new ApplicationUser()
            {
                UserName = "admin",
                DisplayName = "Admin Elek",
                Email = "admin@admin.hu",
            };
            var adminRole = await roleManager.CreateAsync(new IdentityRole("admin"));
            await userManager.CreateAsync(admin, "admin");
            await userManager.AddToRoleAsync(admin,"admin");

            byte[] testFile = File.ReadAllBytes(Path.Combine(sourcePath, "test_file.txt"));
            var faker = new Faker();
            var users = _userFaker.Generate(20);
            var subjects = _subjectFaker.Generate(10);
            var studyMaterials = _studyMaterialFaker.FinishWith((faker,sm) =>
            {
                sm.User = faker.Random.ListItem(users);
                sm.File = testFile.ToArray();
                sm.Subject = faker.Random.ListItem(subjects);
            }).GenerateBetween(30,40);
            var ratings = new List<Rating>();

            studyMaterials.ForEach(studyMaterial =>
            {
                var usersForRating = faker.Random.ListItems(users,faker.Random.Number(0,users.Count)).ToList();

                if(usersForRating.Any())
                    usersForRating.ForEach(user =>
                    {
                        var rating = _ratingFaker.FinishWith((faker, rating) =>
                        {
                            rating.User = user;
                            rating.StudyMaterial = studyMaterial;
                        }).Generate();

                        ratings.Add(rating);
                    });
            });

            subjects.AddRange(_subjectFaker.Generate(5));

            context.AddRange(users);
            context.AddRange(subjects);
            context.AddRange(studyMaterials);
            context.AddRange(ratings);

            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
