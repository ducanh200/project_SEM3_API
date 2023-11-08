using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEM3_API.DTOs;
using SEM3_API.Entities;
using SEM3_API.Models;
using SEM3_API.Models.Project;

namespace SEM3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private Sem3ApiContext _context;

        public ProjectController(Sem3ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var projects = _context.Projects.Include(p => p.Topic).Include(p => p.Country).ToList();
            
            if (projects.Count == 0)
            {
                return BadRequest("Không có cơ sở dữ liệu!");
            }
            List<ProjectDTO> ls = new List<ProjectDTO>();
            foreach (Project p in projects)
            {
                ls.Add(new ProjectDTO
                {
                    id = p.Id,
                    name = p.Name,
                    thumbnail_1 = p.Thumbnail1,
                    thumbnail_2 = p.Thumbnail2,
                    fund = (decimal)(p.Fund),
                    description = p.Description,
                    city = p.City,
                    address = p.Address,
                    country_id = (int)(p.CountryId),
                    country = new CountryDTO { id = p.Country.Id, name = p.Country.Name },
                    topic_id = (int)(p.TopicId),
                    topic = new TopicDTO { id = p.Topic.Id, name = p.Topic.Name },
                    begin = (DateTime)(p.Begin),
                    finish = (DateTime)(p.Finish),
                    create_at = DateTime.Now,
                });
            }

            return Ok(ls);
        }

        [HttpGet]
        [Route("get-by-id")]
        public IActionResult Get(int id)
        {
            try
            {
                Project p = _context.Projects
                    .Where(p => p.Id == id).Include(p => p.Topic).Include(p => p.Country).First();
                if (p == null)

                    return NotFound();
                return Ok(new ProjectDTO
                {
                    id = p.Id,
                    name = p.Name,
                    thumbnail_1 = p.Thumbnail1,
                    thumbnail_2 = p.Thumbnail2,
                    fund = (decimal)(p.Fund),
                    description = p.Description,
                    city = p.City,
                    address = p.Address,
                    country_id = (int)(p.CountryId),
                    country = new CountryDTO { id = p.Country.Id, name = p.Country.Name },
                    topic_id = (int)(p.TopicId),
                    topic = new TopicDTO { id = p.Topic.Id, name = p.Topic.Name },
                    begin = (DateTime)(p.Begin),
                    finish = (DateTime)(p.Finish),
                    create_at = DateTime.Now,
                }) ;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("relateds")]
        public IActionResult Relateds(int id)
        {
            try
            {
                Project p = _context.Projects.Find(id);
                if (p == null)
                    return NotFound();
                List<Project> ls = _context.Projects
                    .Where(p => p.TopicId == p.TopicId)
                    .Where(p => p.Id != id)
                    .Include(p => p.Topic)
                    .Take(4)
                    .OrderByDescending(p => p.Id)
                    .ToList();
                return Ok(ls);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create(CreateProject model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Project project = new Project { Name = model.name, Thumbnail1 = model.thumbnail_1, Thumbnail2 = model.thumbnail_2, Fund = model.fund, Description = model.description, City = model.city, Address = model.address, CountryId = model.countryId, TopicId = model.topicId, Begin = model.begin, Finish = model.finish };
                    _context.Projects.Add(project);
                    _context.SaveChanges();
                    return Created($"get-by-id?id={project.Id}",
                        new ProjectDTO { id = project.Id, name = project.Name });
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
    }
}
