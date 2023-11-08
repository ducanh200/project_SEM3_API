using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEM3_API.DTOs;
using SEM3_API.Entities;
using SEM3_API.Models.Topic;

namespace SEM3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly Sem3ApiContext _context;

        public TopicController(Sem3ApiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Topic> topics = _context.Topics.ToList();
            List<TopicDTO> data = new List<TopicDTO>();
            foreach (Topic t in topics)
            {
                data.Add(new TopicDTO { id = t.Id, name = t.Name });
            }
            return Ok(data);
        }
        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(int id)
        {
            try
            {
                Topic t = _context.Topics.Find(id);
                if (t != null)
                {
                    return Ok(new TopicDTO { id = t.Id, name = t.Name });
                }
            }
            catch (Exception ex)
            {
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Create(CreateTopic model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Topic data = new Topic { Name = model.name };
                    _context.Topics.Add(data);
                    _context.SaveChanges();
                    return Created($"get-by-id?id={data.Id}",
                        new TopicDTO { id = data.Id, name = data.Name });
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
        public IActionResult Update(EditTopic model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Topic topic = new Topic { Id = model.id, Name = model.name };
                    if (topic != null)
                    {
                        _context.Topics.Update(topic);
                        return NoContent();
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            return BadRequest();
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                Topic topic = _context.Topics.Find(id);
                if (topic == null)
                    return NotFound();
                _context.Topics.Remove(topic);
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
