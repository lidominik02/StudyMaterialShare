using AutoMapper;
using StudyMaterialShare.Data;
using StudyMaterialShare.Database.Models;

namespace StudyMaterialShare.WebApi.Profiles
{
    public class StudyMaterialProfile: Profile
    {
        public StudyMaterialProfile()
        {
            CreateMap<StudyMaterial,StudyMaterialDTO>()
                .ForMember(des => des.SubjectId,opt => opt.MapFrom(src => src.Subject.Id))
                .ForMember(des => des.SubjectName,opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(des => des.UserId, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.User.DisplayName))
                .ReverseMap();
        }
    }
}
