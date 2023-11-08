using SEM3_API.Entities;

namespace SEM3_API.DTOs
{
    public class ProjectDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string thumbnail_1 { get; set; }
        public string thumbnail_2 { get; set; }
        public decimal fund { get; set; }
        public string description { get; set; }
        public string city { get; set; }

        public string address { get; set; }

        public int country_id { get; set; }
        public virtual CountryDTO country { get; set; }

        public int topic_id { get; set; }
        public virtual TopicDTO topic { get; set; }

        public DateTime begin { get; set; }

        public DateTime finish { get; set; }

        public DateTime create_at { get; set; }

    }
}
