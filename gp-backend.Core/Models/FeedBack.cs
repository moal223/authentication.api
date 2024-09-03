namespace gp_backend.Core.Models
{
    public class FeedBack
    {
        public int Id { get; set; }
        public bool IsRead { get; set; }
        public string Subject { get; set; }
        public string FeedBackContent { get; set; }
        public ApplicationUser User { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
