using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEM3_API.DTOs;
using SEM3_API.Entities;
using SEM3_API.Models.News;

namespace SEM3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly Sem3ApiContext _context;
        public NewsController(Sem3ApiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<News> news = _context.News.Include(p => p.Topic).Include(p => p.Country).ToList();
            List<NewsDTO> data = new List<NewsDTO>();
            foreach (News n in news)
            {
                data.Add(new NewsDTO
                {
                    id = n.Id,
                    name = n.Name,
                    city = n.City,
                    thumbnail = n.Thumbnail,
                    description = n.Description,
                    topic_id = n.TopicId,
                    topic = new TopicDTO { id = n.Topic.Id, name = n.Topic.Name },
                    country_id = n.CountryId,
                    country =  new CountryDTO { id = n.Country.Id, name = n.Country.Name },
                    created_at = Convert.ToDateTime(n.CreatedAt)
                });
            }
            return Ok(data);
        }
        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(int id)
        {
            try
            {
                News n = _context.News
                     .Where(n => n.Id == id).Include(n => n.Topic).Include(n => n.Country).First();
                if (n == null)
                    return NotFound();
                return Ok(new NewsDTO
                {
                    id = n.Id,
                    name = n.Name,
                    city = n.City,
                    thumbnail = n.Thumbnail,
                    description = n.Description,
                    topic_id = (int)(n.TopicId),
                    topic = new TopicDTO { id = n.Topic.Id, name = n.Topic.Name },
                    country_id = (int)(n.CountryId),
                    country = new CountryDTO { id = n.Country.Id, name = n.Country.Name },
                    created_at = Convert.ToDateTime(n.CreatedAt)
                });

            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Create(CreateNews model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    News news = new News { Name = model.name, City = model.city, Thumbnail = model.thumbnail, Description = model.description, CountryId = model.country_id, TopicId = model.topic_id ,CreatedAt = DateTime.UtcNow};
                    _context.News.Add(news);
                    _context.SaveChanges();
                    return Created($"get-by-id?id={news.Id}",
                        new NewsDTO { id = news.Id, name = news.Name });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            var msgs = ModelState.Values.SelectMany(v => v.Errors)
                .Select(v => v.ErrorMessage);
            return BadRequest(string.Join(" | ", msgs));
        }
        [HttpPut]
        [Route("updated_news")]
        public IActionResult Update(EditNews model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    News news = new News { Id = model.id, Name = model.name, City = model.city, Thumbnail = model.thumbnail, Description = model.description, TopicId = model.topicId, CountryId = model.countryId };
                    if (news != null)
                    {
                        _context.News.Update(news);
                        _context.SaveChanges();
                        return Ok("Đã sửa bài báo thành công!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest();
        }
        [HttpDelete]
        [Route("deleted_news")]
        public IActionResult Delete(int id)
        {
            try
            {
                News news = _context.News.Find(id);
                if (news == null)
                    return NotFound();
                _context.News.Remove(news);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
