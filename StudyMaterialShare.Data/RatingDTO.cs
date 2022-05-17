using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyMaterialShare.Data
{
    public class RatingDTO
    {
        public int Id { get; set; }

        public int RateValue { get; set; }

        public int StudyMaterialId { get; set; }

        public string UserId { get; set; } = string.Empty;
    }
}
