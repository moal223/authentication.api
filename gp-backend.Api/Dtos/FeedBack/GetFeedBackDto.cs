namespace gp_backend.Api.Dtos.FeedBack
{
    public class GetFeedBackDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string FeedBackContent { get; set; }
        public bool IsRead { get; set; }
    }
}
