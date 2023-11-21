using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SEM3_API.DTOs
{
    [Route("[Controller]")]
    [ApiController]
    public class TopicDTO
    {
        public int id {  get; set; }
        public string name { get; set; }
    }
}
