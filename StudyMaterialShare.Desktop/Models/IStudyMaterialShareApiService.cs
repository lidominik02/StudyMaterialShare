using StudyMaterialShare.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyMaterialShare.Desktop.Models
{
    public interface IStudyMaterialShareApiService
    {
        Task<SubjectDTO> CreateSubjectAsync(SubjectDTO newSubject);
        Task<IEnumerable<StudyMaterialDTO>> GetAllStudyMaterialsAsync();
        Task<IEnumerable<StudyMaterialDTO>> GetAllStudyMaterialsAsync(int subjectId);
        Task<IEnumerable<StudyMaterialDTO>> GetAllStudyMaterialsAsync(string title);
        Task<IEnumerable<StudyMaterialDTO>> GetAllStudyMaterialsAsync(string title, int subjectId);
        Task<IEnumerable<SubjectDTO>> GetAllSubjectsAsync();
        Task<SubjectDTO> GetSubjectByIdAsync(int subjectId);
        Task<StudyMaterialDTO> GetStudyMaterialByIdAsync(int studyMaterialId);
        Task<StudyMaterialDTO> RemoveRatingsFromStudyMaterialAsync(int studyMaterialId);
        Task<StudyMaterialDTO> RemoveStudyMaterialAsync(int studyMaterialId);
        Task<SubjectDTO> RemoveSubjectAsync(int subjectId);
        Task<StudyMaterialDTO> ResetStudyMaterialDownloadsAsync(StudyMaterialDTO studyMaterial);
        Task<StudyMaterialDTO> UpdateStudyMaterialAsync(StudyMaterialDTO studyMaterial);
        Task<SubjectDTO> UpdateSubjectAsync(SubjectDTO subject);

        Task LoginAsync(LoginDTO user);
        Task LogoutAsync();
    }
}