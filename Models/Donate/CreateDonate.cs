namespace SEM3_API.Models.Donate
{
    public class CreateDonate
    {
        public int project_id { get; set; }
        public int user_id { get; set; }
        public decimal amount { get; set; }
    }
}
