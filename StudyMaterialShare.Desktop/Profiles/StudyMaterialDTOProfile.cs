using AutoMapper;
using StudyMaterialShare.Data;
using StudyMaterialShare.Desktop.ViewModels;

namespace StudyMaterialShare.Desktop.Profiles
{
    public class StudyMaterialDTOProfile : Profile
    {
        public StudyMaterialDTOProfile()
        {
            CreateMap<StudyMaterialDTO,StudyMaterialViewModel>()
                .ReverseMap();
        }
    }
}
