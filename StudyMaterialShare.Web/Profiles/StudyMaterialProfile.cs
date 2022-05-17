using AutoMapper;
using StudyMaterialShare.Database.Models;
using StudyMaterialShare.Web.Models;

namespace StudyMaterialShare.Web.Profiles
{
    public class StudyMaterialProfile : Profile
    {
        public StudyMaterialProfile()
        {
            CreateMap<StudyMaterial, StudyMaterialViewModel>()
                .ForMember(src => src.Subject,opt => opt.MapFrom(dst => dst.Subject.Name))
                .ForMember(src => src.User,opt => opt.MapFrom(dst => dst.User.DisplayName));
        }
    }
}
