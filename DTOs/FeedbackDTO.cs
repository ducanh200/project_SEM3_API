namespace SEM3_API.DTOs
{
    public class FeedbackDTO
    {
        public int id { get; set; }
        public string message { get; set; }
        public int user_id { get; set; }
        public virtual UserDTO user { get; set; }

        public int project_id { get; set; }
        public virtual ProjectDTO project { get; set; }

        public DateTime create_at { get; set; }


    }
}
