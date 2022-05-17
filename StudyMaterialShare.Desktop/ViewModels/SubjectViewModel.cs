using StudyMaterialShare.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyMaterialShare.Desktop.ViewModels
{
    public class SubjectViewModel : ViewModelBase
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public SubjectViewModel ShallowClone()
        {
            return (SubjectViewModel)this.MemberwiseClone();
        }
    }
}
