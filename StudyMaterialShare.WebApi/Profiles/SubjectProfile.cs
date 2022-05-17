using AutoMapper;
using StudyMaterialShare.Data;
using StudyMaterialShare.Database.Models;

namespace StudyMaterialShare.WebApi.Profiles
{
    public class SubjectProfile: Profile
    {
        public SubjectProfile()
        {
            CreateMap<Subject, SubjectDTO>()
                .ReverseMap();
        }
    }
}
