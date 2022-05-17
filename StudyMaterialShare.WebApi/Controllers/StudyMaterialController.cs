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
    public class StudyMaterialController : ControllerBase
    {
        private readonly StudyMaterialRepository _studyMaterialRepository;
        private readonly IMapper _mapper;

        public StudyMaterialController(StudyMaterialRepository studyMaterialRepository, IMapper mapper)
        {
            _studyMaterialRepository = studyMaterialRepository;
            _mapper = mapper;
        }

        // GET: api/<StudyMaterialController>
        [HttpGet]
        public ActionResult<IEnumerable<StudyMaterialDTO>> Get([FromQuery] int? subjectId, [FromQuery] string? title)
        {
            IEnumerable<StudyMaterial> studyMaterials;

            if (subjectId != null && !string.IsNullOrEmpty(title))
                studyMaterials = _studyMaterialRepository
                    .Get(sm => sm.Subject.Id == subjectId && sm.Title.Contains(title), null);
            else if (subjectId != null && string.IsNullOrEmpty(title))
                studyMaterials = _studyMaterialRepository.Get(sm => sm.Subject.Id == subjectId, null);
            else if (subjectId == null && !string.IsNullOrEmpty(title))
                studyMaterials = _studyMaterialRepository.Get(sm => sm.Title.Contains(title),null);
            else
                studyMaterials = _studyMaterialRepository.Get();

            return Ok(_mapper
                    .Map<List<StudyMaterial>, IEnumerable<StudyMaterialDTO>>(studyMaterials.ToList())
                    .ToList());
        }

        // GET api/<StudyMaterialController>/5
        [HttpGet("{id}")]
        public ActionResult<StudyMaterialDTO> Get(int id)
        {
            var studyMaterial = _studyMaterialRepository.Get(id);

            if(studyMaterial == null) return NotFound();

            return Ok(_mapper.Map<StudyMaterialDTO>(studyMaterial));
        }

        // POST api/<StudyMaterialController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult<StudyMaterialDTO> Post([FromBody] StudyMaterialDTO newStudyMaterial)
        {
            var studyMaterial = _studyMaterialRepository.Create(_mapper.Map<StudyMaterial>(newStudyMaterial));

            if(studyMaterial == null) return StatusCode(StatusCodes.Status500InternalServerError);

            return CreatedAtAction(nameof(Get),new { id = studyMaterial.Id },_mapper.Map<StudyMaterialDTO>(studyMaterial));
        }

        // PUT api/<StudyMaterialController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult<StudyMaterialDTO> Put(int id, [FromBody] StudyMaterialDTO updatableStudyMaterialDTO)
        {
            if (id != updatableStudyMaterialDTO.Id) return BadRequest();

            var updatableStudyMaterial = _studyMaterialRepository.Get(id);

            if (updatableStudyMaterial == null) return NotFound();

            _mapper.Map(updatableStudyMaterialDTO, updatableStudyMaterial);

            var updatedStudyMaterial = _studyMaterialRepository.Update(updatableStudyMaterial);

            if (updatedStudyMaterial == null) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(_mapper.Map<StudyMaterialDTO>(updatedStudyMaterial));
        }

        // DELETE api/<StudyMaterialController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult<StudyMaterialDTO> Delete(int id)
        {
            var deletableStudyMaterial = _studyMaterialRepository.Get(id);

            if(deletableStudyMaterial == null) return NotFound();

            var deletedStudyMaterial = _studyMaterialRepository.Delete(deletableStudyMaterial);

            if(deletedStudyMaterial == null) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(_mapper.Map<StudyMaterialDTO>(deletedStudyMaterial));
        }

        [HttpDelete("{id}/ratings")]
        [Authorize(Roles = "admin")]
        public ActionResult<StudyMaterialDTO> DeleteRatings(int id)
        {
            var studyMaterial = _studyMaterialRepository.Get(id);

            if (studyMaterial == null) return NotFound();

            var isRatingsDeleteSuccess= _studyMaterialRepository.DeleteStudyMaterialRatings(studyMaterial);

            if (!isRatingsDeleteSuccess) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(_mapper.Map<StudyMaterialDTO>(studyMaterial));
        }
    }
}
