using Microsoft.AspNetCore.Identity;
using StudyMaterialShare.Database;
using StudyMaterialShare.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudyMaterialShare.WebApi.Test
{
    public static class TestDbInitializer
    {
        public static void Initialize(StudyMaterialShareDbContext context)
        {
            Random random = new();
            List<Subject> subjects = new List<Subject>();
            List<StudyMaterial> studyMaterials = new List<StudyMaterial>();
            List<Rating> ratings = new List<Rating>(); 
            ApplicationUser user = new ApplicationUser()
            {
                UserName = "test_user",
                DisplayName = "Test User",
                Email = "test@user.hu",
                PasswordHash = "password",

            };

            context.Add(user);

            subjects.Add(new Subject()
            {
                Name = "Webes alkalamzások fejlesztése"
            });

            subjects.Add(new Subject()
            {
                Name = "Eseményvezérelt alkalmazások"
            });

            subjects.Add(new Subject()
            {
                Name = "Objektumelvű programozás"
            });

            subjects.Add(new Subject()
            {
                Name = "Programozás"
            });

            subjects.Add(new Subject()
            {
                Name = "Kliensoldali webprogramozás"
            });

            subjects.Add(new Subject()
            {
                Name = "Szerveroldali webprogramozás"
            });

            subjects.Add(new Subject()
            {
                Name = "Webprogramozás"
            });

            foreach (var subject in subjects)
            {
                context.Add(subject);
            }

            foreach (var subject in subjects)
            {
                for (int i = 0; i < 3; i++)
                {
                    var sm = new StudyMaterial()
                    {
                        Title = $"{subject.Name} tananyag no. {i + 1}",
                        File = new byte[1],
                        FileType = "txt",
                        UploadedAt = DateTime.Now,
                        Downloads = 0,
                        Subject = subject,
                        User = user,
                    };
                    studyMaterials.Add(sm);
                    context.Add(sm);
                }
            }

            foreach (var studyMaterial in studyMaterials)
            {
                int ratingCount = 3;
                studyMaterial.Ratings = new List<Rating>();

                for (int i = 0; i < ratingCount; i++)
                {
                    context.Add(new Rating()
                    {
                        RateValue = random.Next(1, 5),
                        StudyMaterial = studyMaterial,
                        User = user,
                    });
                }
            }

            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
