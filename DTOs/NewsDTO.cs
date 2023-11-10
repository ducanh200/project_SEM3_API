namespace SEM3_API.DTOs
{
    public class NewsDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string thumbnail { get; set; }
        public string description { get; set; }
        public int topic_id { get; set; }
        public virtual TopicDTO topic { get; set; }
        public int country_id { get; set; }
        public virtual CountryDTO country { get; set; }
        public DateTime created_at { get; set; }

    }
}
