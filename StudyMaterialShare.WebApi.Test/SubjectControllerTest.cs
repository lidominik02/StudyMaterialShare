using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyMaterialShare.Data;
using StudyMaterialShare.Database;
using StudyMaterialShare.Database.Models;
using StudyMaterialShare.Database.Repositories;
using StudyMaterialShare.WebApi.Controllers;
using StudyMaterialShare.WebApi.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StudyMaterialShare.WebApi.Test
{
    public class SubjectControllerTest : IDisposable
    {
        private readonly SubjectRepository _repository;
        private readonly SubjectController _controller;
        private readonly StudyMaterialShareDbContext _context;
        private readonly IMapper _mapper;

        public SubjectControllerTest()
        {
            var options = new DbContextOptionsBuilder<StudyMaterialShareDbContext>()
                .UseInMemoryDatabase("SubjectControllerTestDb")
                .UseLazyLoadingProxies()
                .Options;

            _context = new StudyMaterialShareDbContext(options);
            _context.Database.EnsureCreated();
            _context.ChangeTracker.Clear();

            TestDbInitializer.Initialize(_context);

            _repository = new SubjectRepository(_context);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new StudyMaterialProfile());
                cfg.AddProfile(new SubjectProfile());
                cfg.AddProfile(new RatingProfile());
            });
            _mapper = new Mapper(config);
            _controller = new SubjectController(_repository, _mapper);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
        
        [Fact]
        public void GetAllSubjectTest()
        {
            var response = _controller.Get();
            int smCount = _repository.Get().Count();

            var result = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var content = Assert.IsAssignableFrom<IEnumerable<SubjectDTO>>(result.Value);
            Assert.Equal(smCount, content.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetSubjectByIdTest(int id)
        {
            var response = _controller.Get(id);

            var result = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var value = Assert.IsAssignableFrom<SubjectDTO>(result.Value);
            Assert.Equal(id, value.Id);
        }

        [Fact]
        public void GetSubjectWithNotExistentIdTest()
        {
            int invalidId = -10;
            var response = _controller.Get(invalidId);

            Assert.IsAssignableFrom<NotFoundResult>(response.Result);
        }

        [Fact]
        public void PostSubjectTest()
        {
            string newSubjectName = "TestName";
            var subjectToPostDTO = new SubjectDTO()
            {
                Name = newSubjectName
            };
            var response = _controller.Post(subjectToPostDTO);

            var result = Assert.IsAssignableFrom<CreatedAtActionResult>(response.Result);
            var value = Assert.IsAssignableFrom<SubjectDTO>(result.Value);
            Assert.Equal(newSubjectName,value.Name);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void PutSubjectTest(int id)
        {
            string newName = "NewName";
            var subjectToPut = _repository.Get(id);
            var subjectToPutDTO = _mapper.Map<SubjectDTO>(subjectToPut);
            subjectToPutDTO.Name = newName;
            var response = _controller.Put(id,subjectToPutDTO);

            var result = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var content = Assert.IsAssignableFrom<SubjectDTO>(result.Value);
            Assert.Equal(id, content.Id);
            Assert.Equal(newName,content.Name);
        }

        [Fact]
        public void PutNonexistentSubjectTest()
        {
            int id = -10;
            var subjectToPutDTO = new SubjectDTO()
            {
                Id = id
            };
            var response = _controller.Put(id, subjectToPutDTO);

            Assert.IsAssignableFrom<NotFoundResult>(response.Result);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        public void DeleteSubjectWithStudyMaterialsTest(int id)
        {
            var response = _controller.Delete(id);

            var result = Assert.IsAssignableFrom<StatusCodeResult>(response.Result);
            Assert.Equal(405, result.StatusCode);
        }

        [Fact]
        public void DeleteSubjectWithoutStudyMaterialsTest()
        {
            var subjectWithoutStudyMaterial = _repository.Create(new Subject()
            {
                Name = "SubjectWithoutStudyMaterial"
            });
            int id = subjectWithoutStudyMaterial!.Id;

            var response = _controller.Delete(id);

            var result = Assert.IsAssignableFrom<OkObjectResult>(response.Result);
            var value = Assert.IsAssignableFrom<SubjectDTO>(result.Value);
            Assert.Equal(id, value.Id);
        }

        [Fact]
        public void DeleteNonexistentSubjectTest()
        {
            int invalidId = -10;
            var response = _controller.Delete(invalidId);

            Assert.IsAssignableFrom<NotFoundResult>(response.Result);
        }
    }
}
