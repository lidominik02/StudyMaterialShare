using AutoMapper;
using StudyMaterialShare.Data;
using StudyMaterialShare.Desktop.ViewModels;

namespace StudyMaterialShare.Desktop.Profiles
{
    public class SubjectDTOProfile : Profile
    {
        public SubjectDTOProfile()
        {
            CreateMap<SubjectDTO,SubjectViewModel>()
                .ReverseMap();
        }
    }
}
