using ICFAIClone.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ICFAIClone.Models
{
    public class AdminDashboardViewModel
    {
        public List<Enquiry> Enquiries { get; set; }
        public List<Student> Students { get; set; }
        public List<MediaItem> MediaItems { get; set; }

    }

    public class MediaItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }


}
