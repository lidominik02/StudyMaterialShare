using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyMaterialShare.Data;
using StudyMaterialShare.Database.Models;
using StudyMaterialShare.Database.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudyMaterialShare.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly SubjectRepository _subjectRepository;
        private readonly IMapper _mapper;

        public SubjectController(SubjectRepository subjectRepository, IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _mapper = mapper;
        }

        // GET: api/<SubjectController>
        [HttpGet]
        public ActionResult<IEnumerable<SubjectDTO>> Get()
        {
            return Ok(_mapper
                .Map<IEnumerable<Subject>, IEnumerable<SubjectDTO>>(_subjectRepository.Get()));
        }

        // GET api/<SubjectController>/5
        [HttpGet("{id}")]
        public ActionResult<SubjectDTO> Get(int id)
        {
            var subject = _subjectRepository.Get(id);

            if (subject == null) return NotFound();

            return Ok(_mapper.Map<SubjectDTO>(subject));
        }

        // POST api/<SubjectController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult<SubjectDTO> Post([FromBody] SubjectDTO newSubjectDTO)
        {
            var newSubject = _subjectRepository.Create(_mapper.Map<Subject>(newSubjectDTO));

            if (newSubject == null) return StatusCode(StatusCodes.Status500InternalServerError);

            return CreatedAtAction(
                nameof(Get),
                new { id = newSubject.Id}, 
                _mapper.Map<SubjectDTO>(newSubject));
        }

        // PUT api/<SubjectController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult<SubjectDTO> Put(int id, [FromBody] SubjectDTO updatableSubjectDTO)
        {
            if (id != updatableSubjectDTO.Id) return BadRequest();

            var updatableSubject = _subjectRepository.Get(id);

            if (updatableSubject == null) return NotFound();

            _mapper.Map(updatableSubjectDTO, updatableSubject);

            var updatedSubject = _subjectRepository.Update(updatableSubject);

            if (updatedSubject == null) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(_mapper.Map<SubjectDTO>(updatedSubject));
        }

        // DELETE api/<SubjectController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult<SubjectDTO> Delete(int id)
        {
            var deletableSubject = _subjectRepository.Get(id);

            if (deletableSubject == null) return NotFound();

            var studyMaterialsForSubject = _subjectRepository.GetStudyMaterialsForSubject(deletableSubject); 

            if (studyMaterialsForSubject.Any())
                return StatusCode(StatusCodes.Status405MethodNotAllowed);

            var deletedSubject = _subjectRepository.Delete(deletableSubject);

            if (deletedSubject == null) 
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(_mapper.Map<SubjectDTO>(deletableSubject));
        }
    }
}
