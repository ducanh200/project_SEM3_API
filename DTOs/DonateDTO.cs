namespace SEM3_API.DTOs
{
    public class DonateDTO
    {
        public int id { get; set; }
        public decimal amount { get; set; }
        public int project_id { get; set; }
        public int user_id { get;set; }
        public DateTime created_at { get; set; }
    }
}
