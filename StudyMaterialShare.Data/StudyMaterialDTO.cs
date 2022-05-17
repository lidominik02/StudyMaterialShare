using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyMaterialShare.Data
{
    public class StudyMaterialDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        //public byte[] File { get; set; } = null!;

        public string FileType { get; set; } = string.Empty;

        public DateTime UploadedAt { get; set; }

        public int Downloads { get; set; }

        public int SubjectId { get; set; }

        public string SubjectName { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public double AvarageRating { get; set; }
    }
}
