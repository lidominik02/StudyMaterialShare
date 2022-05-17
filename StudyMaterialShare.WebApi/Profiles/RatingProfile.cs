using AutoMapper;
using StudyMaterialShare.Data;
using StudyMaterialShare.Database.Models;

namespace StudyMaterialShare.WebApi.Profiles
{
    public class RatingProfile: Profile
    {
        public RatingProfile()
        {
            CreateMap<Rating,RatingDTO>()
                .ReverseMap();
        }
    }
}
