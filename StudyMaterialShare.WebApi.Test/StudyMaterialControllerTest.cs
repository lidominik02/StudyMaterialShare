using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using StudyMaterialShare.Data;
using StudyMaterialShare.Database;
using StudyMaterialShare.Database.Repositories;
using StudyMaterialShare.WebApi.Controllers;
using StudyMaterialShare.WebApi.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StudyMaterialShare.WebApi.Test
{
    public class StudyMaterialControllerTest : IDisposable
    {
        private readonly StudyMaterialRepository _repository;
        private readonly StudyMaterialController _controller;
        private readonly StudyMaterialShareDbContext _context;
        private readonly IMapper _mapper;

        public StudyMaterialControllerTest()
        {

            var options = new DbContextOptionsBuilder<StudyMaterialShareDbContext>()
                .UseInMemoryDatabase("StudyMaterialControllerTestDb")
                .UseLazyLoadingProxies()
                .Options;

            _context = new StudyMaterialShareDbContext(options);
            _context.Database.EnsureCreated();
            _context.ChangeTracker.Clear();

            TestDbInitializer.Initialize(_context);

            _repository = new StudyMaterialRepository(_context);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new StudyMaterialProfile());
                cfg.AddProfile(new SubjectProfile());
                cfg.AddProfile(new RatingProfile());
            });
            _mapper = new Mapper(config);
            _controller = new StudyMaterialController(_repository, _mapper);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        
        [Fact]
        public void GetAllStudyMaterialsTest()
        {
            var response = _controller.Get(null, null);
            int smCount = _repository.Get().ToList().Count;
            var content = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<StudyMaterialDTO>>(content.Value);
            Assert.Equal(smCount, value.Count());
        }
        
        [Theory]
        [InlineData("Programozás")]
        [InlineData("rog")]
        [InlineData("e")]
        public void GetStudyMaterialsWithTitleTest(string? title)
        {
            var response = _controller.Get(null, title);
            var result = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<StudyMaterialDTO>>(result.Value);
            foreach (var item in value)
            {
                Assert.Contains(title, item.Title);
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetStudyMaterialsWithSubjectIdTest(int? subjectId)
        {
            var response = _controller.Get(subjectId, null);
            var result = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<StudyMaterialDTO>>(result.Value);
            foreach (var item in value)
            {
                Assert.Equal(subjectId, item.SubjectId);
            }
        }
        
        [Theory]
        [InlineData(4,"Programozás")]
        [InlineData(5,"e")]
        public void GetStudyMaterialsWithTitleAndSubjectIdTest(int? subjectId,string? title)
        {
            var response = _controller.Get(subjectId, null);
            var result = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<StudyMaterialDTO>>(result.Value);
            foreach (var item in value)
            {
                Assert.Contains(title, item.Title);
                Assert.Equal(subjectId,item.SubjectId);
            }
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void PutStudyMaterialTest(int id)
        {
            string newTitle = $"Updated{id}";
            int newDownload = 0;

            var studyMaterialToPut = _repository.Get(id);

            var studyMaterialDTOToPut = _mapper.Map<StudyMaterialDTO>(studyMaterialToPut);
            studyMaterialDTOToPut.Title = newTitle;
            studyMaterialDTOToPut.Downloads = newDownload;

            var response = _controller.Put(id,studyMaterialDTOToPut);
            var result = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var value = Assert.IsAssignableFrom<StudyMaterialDTO>(result.Value);

            Assert.Equal(id, value.Id);
            Assert.Equal(newTitle, value.Title);
            Assert.Equal(newDownload, value.Downloads);
        }
        
        [Theory]
        [InlineData(7)]
        [InlineData(8)]
        public void DeleteStudyMaterialTest(int id)
        {
            var response = _controller.Delete(id);
            var result = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var value = Assert.IsAssignableFrom<StudyMaterialDTO>(result.Value);
            Assert.Equal(id,value.Id);
        }

        [Fact]
        public void DeleteNonexistentStudyMaterialTest()
        {
            var response = _controller.Delete(-10);
            Assert.IsAssignableFrom<NotFoundResult>(response.Result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void DeleteStudyMaterialRatingsTest(int id)
        {
            var response = _controller.DeleteRatings(id);
            var result = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var value = Assert.IsAssignableFrom<StudyMaterialDTO>(result.Value);
            var studyMaterial = _repository.Get(value.Id);
            Assert.Empty(studyMaterial!.Ratings);
        }

        [Fact]
        public void DeleteNonexistentStudyMaterialRatingsTest()
        {
            int nonexistentId = -10;
            var response = _controller.DeleteRatings(nonexistentId);
            Assert.IsAssignableFrom<NotFoundResult>(response.Result);
        }

    }
}
