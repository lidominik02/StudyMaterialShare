namespace StudyMaterialShare.Web.Models
{
    public class BrowseQueryViewModel
    {
        private int page = 0;

        public int Page { 
            get => page; 
            set => page = (value < 0) ? 0 : value;
        }

        public string Sort { get; set; } = "title";

        public string Order { get; set; } = "asc";

        public string Subject { get; set; } = "";

        public string TitleFilter { get; set; } = "";
    }
}
