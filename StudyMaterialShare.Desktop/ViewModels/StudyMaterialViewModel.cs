using StudyMaterialShare.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyMaterialShare.Desktop.ViewModels
{
    public class StudyMaterialViewModel : ViewModelBase
    {
        private int _id;
        private string _title = string.Empty;
        private string _fileType = string.Empty;
        private DateTime _uploadedAt;
        private int _downloads;
        private int _subjectId;
        private string _userId = string.Empty;
        private double _avarageRating;
        private string _subjectName = string.Empty;
        private string _userName = string.Empty;

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string FileType
        {
            get => _fileType;
            set { _fileType = value; OnPropertyChanged(); }
        }

        public DateTime UploadedAt
        {
            get => _uploadedAt;
            set { _uploadedAt = value; OnPropertyChanged(); }
        }

        public int Downloads
        {
            get => _downloads;
            set { _downloads = value; OnPropertyChanged(); }
        }

        public int SubjectId
        {
            get => _subjectId;
            set { _subjectId = value; OnPropertyChanged(); }
        }

        public string SubjectName { get => _subjectName; set => _subjectName = value; }

        public string UserId
        {
            get => _userId;
            set { _userId = value; OnPropertyChanged(); }
        }

        public string UserName { get => _userName; set => _userName = value; }

        public double AvarageRating
        {
            get => _avarageRating;
            set { _avarageRating = value; OnPropertyChanged(); }
        }

        public StudyMaterialViewModel ShallowClone()
        {
            return (StudyMaterialViewModel)this.MemberwiseClone();
        }
    }
}
