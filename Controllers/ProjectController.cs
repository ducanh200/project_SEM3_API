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
        private readonly IWebHostEnvironment _environment;
        public ProjectController(Sem3ApiContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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
                    begin = Convert.ToDateTime(p.Begin),
                    finish = Convert.ToDateTime(p.Finish),
                    create_at = Convert.ToDateTime(p.CreatedAt)                
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
                    begin = Convert.ToDateTime(p.Begin),
                    finish = Convert.ToDateTime(p.Finish),
                    create_at = Convert.ToDateTime(p.CreatedAt)
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

                List<ProjectDTO> ls = _context.Projects
            .Where(p => p.TopicId == p.TopicId && p.Id != id)
            .Include(p => p.Topic)
            .Take(4)
            .OrderByDescending(p => p.Id)
            .Select(p => new ProjectDTO
            {
                id = p.Id,
                name = p.Name,
                // Các thuộc tính khác tương tự
                topic = new TopicDTO
                {
                    id = p.Topic.Id,
                    name = p.Topic.Name
                },
                thumbnail_1 = p.Thumbnail1,
                thumbnail_2 = p.Thumbnail2,
                fund = (decimal)(p.Fund),
                description = p.Description,
                city = p.City,
                address = p.Address,
                begin = Convert.ToDateTime(p.Begin),
                finish= Convert.ToDateTime(p.Finish),
                create_at = Convert.ToDateTime (p.CreatedAt),
            })
            .ToList();
                return Ok(ls);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Create([FromForm] CreateProject model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra xem model có chứa ít nhất một ảnh thumbnail hay không
                    if ((model.thumbnailFile1 == null || model.thumbnailFile1.Length == 0) &&
                        (model.thumbnailFile2 == null || model.thumbnailFile2.Length == 0))
                    {
                        return BadRequest("Vui lòng chọn ít nhất một file ảnh");
                    }

                    // Tạo danh sách để lưu trữ các tên file ảnh
                    List<string> fileNames = new List<string>();

                    // Xử lý thumbnail 1
                    if (model.thumbnailFile1 != null && model.thumbnailFile1.Length > 0)
                    {
                        var fileName1 = Guid.NewGuid().ToString() + Path.GetExtension(model.thumbnailFile1.FileName);
                        var filePath1 = Path.Combine(_environment.WebRootPath, "uploads", fileName1);

                        using (var fileStream1 = new FileStream(filePath1, FileMode.Create))
                        {
                            model.thumbnailFile1.CopyTo(fileStream1);
                        }
                        string url1 = $"{Request.Scheme}://{Request.Host}/uploads/{fileName1}";

                        fileNames.Add(url1);
                    }

                    // Xử lý thumbnail 2
                    if (model.thumbnailFile2 != null && model.thumbnailFile2.Length > 0)
                    {
                        var fileName2 = Guid.NewGuid().ToString() + Path.GetExtension(model.thumbnailFile2.FileName);
                        var filePath2 = Path.Combine(_environment.WebRootPath, "uploads", fileName2);

                        using (var fileStream2 = new FileStream(filePath2, FileMode.Create))
                        {
                            model.thumbnailFile2.CopyTo(fileStream2);
                        }
                        string url2 = $"{Request.Scheme}://{Request.Host}/uploads/{fileName2}";

                        fileNames.Add(url2);
                    }

                    // Tạo dự án với danh sách tên file ảnh
                    Project project = new Project
                    {
                        Name = model.name,
                        Thumbnail1 = fileNames.Count > 0 ? fileNames[0] : null,
                        Thumbnail2 = fileNames.Count > 1 ? fileNames[1] : null,
                        Fund = model.fund,
                        Description = model.description,
                        City = model.city,
                        Address = model.address,
                        CountryId = model.countryId,
                        TopicId = model.topicId,
                        Begin = model.begin,
                        Finish = model.finish,
                        CreatedAt = DateTime.UtcNow
                    };

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

        [HttpPut]
        public IActionResult Update([FromForm] EditProject model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Project existingProject = _context.Projects.Find(model.id);

                    if (existingProject == null)
                    {
                        return NotFound("Không tìm thấy dự án cần sửa");
                    }

                    // Kiểm tra xem model có chứa ảnh mới hay không
                    if (model.thumbnailFile1 != null && model.thumbnailFile1.Length > 0)
                    {
                        var fileName1 = Guid.NewGuid().ToString() + Path.GetExtension(model.thumbnailFile1.FileName);
                        var filePath1 = Path.Combine(_environment.WebRootPath, "uploads", fileName1);

                        using (var fileStream1 = new FileStream(filePath1, FileMode.Create))
                        {
                            model.thumbnailFile1.CopyTo(fileStream1);
                        }
                        string url1 = $"{Request.Scheme}://{Request.Host}/uploads/{fileName1}";
                        existingProject.Thumbnail1 = url1;
                    }

                    if (model.thumbnailFile2 != null && model.thumbnailFile2.Length > 0)
                    {
                        var fileName2 = Guid.NewGuid().ToString() + Path.GetExtension(model.thumbnailFile2.FileName);
                        var filePath2 = Path.Combine(_environment.WebRootPath, "uploads", fileName2);

                        using (var fileStream2 = new FileStream(filePath2, FileMode.Create))
                        {
                            model.thumbnailFile2.CopyTo(fileStream2);
                        }
                        string url2 = $"{Request.Scheme}://{Request.Host}/uploads/{fileName2}";
                        existingProject.Thumbnail2 = url2;
                    }

                    // Cập nhật các thuộc tính khác từ model
                    existingProject.Name = model.name;
                    existingProject.Fund = model.fund;
                    existingProject.Description = model.description;
                    existingProject.City = model.city;
                    existingProject.Address = model.address;
                    existingProject.CountryId = model.countryId;
                    existingProject.TopicId = model.topicId;
                    existingProject.Begin = model.begin;
                    existingProject.Finish = model.finish;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _context.SaveChanges();

                    return Ok("Đã sửa dự án thành công!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest("Dữ liệu không hợp lệ");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                Project project = _context.Projects.Find(id);
                if (project == null)
                    return NotFound();
                    _context.Projects.Remove(project);
                    _context.SaveChanges();
                    return Ok("Deleted");
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
