using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEM3_API.DTOs;
using SEM3_API.Entities;
using SEM3_API.Models.News;
using System.IO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SEM3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly Sem3ApiContext _context;
        private readonly IWebHostEnvironment _environment;
        public NewsController(Sem3ApiContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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

        [HttpGet]
        [Route("relateds")]
        public IActionResult Relateds(int id)
        {
            try
            {
                News p = _context.News.Find(id);
                if (p == null)
                    return NotFound();

                List<NewsDTO> ls = _context.News
            .Where(p => p.TopicId == p.TopicId && p.Id != id)
            .Include(p => p.Topic)
            .Take(4)
            .OrderByDescending(p => p.Id)
            .Select(p => new NewsDTO
            {
                id = p.Id,
                name = p.Name,
                // Các thuộc tính khác tương tự
                topic = new TopicDTO
                {
                    id = p.Topic.Id,
                    name = p.Topic.Name
                },
                thumbnail = p.Thumbnail,
                description = p.Description,
                city = p.City,
                created_at = Convert.ToDateTime(p.CreatedAt),
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
        public IActionResult Create([FromForm] CreateNews model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the model includes a file
                    if (model.thumbnailFile != null && model.thumbnailFile.Length > 0)
                    {
                        // Generate a unique file name
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.thumbnailFile.FileName);
                        var filePath = Path.Combine("uploads", fileName); // Relative path

                        // Combine the file path with the wwwroot folder (or any other folder you prefer)
                        var absolutePath = Path.Combine(_environment.WebRootPath, filePath);

                        // Save the file to the server
                        using (var fileStream = new FileStream(absolutePath, FileMode.Create))
                        {
                            model.thumbnailFile.CopyTo(fileStream);
                        }
                        string url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

                        var topic = _context.Topics.Find(model.topic_id);
                        var country = _context.Countries.Find(model.country_id);

                        News news = new News
                        {
                            Name = model.name,
                            City = model.city,
                            Thumbnail = url, // Use the file name directly
                            Description = model.description,
                            CountryId = model.country_id,
                            TopicId = model.topic_id,
                            CreatedAt = DateTime.UtcNow
                        };

                        _context.News.Add(news);
                        _context.SaveChanges();

                        NewsDTO newsDTO = new NewsDTO
                        {
                            id = news.Id,
                            name = news.Name,
                            city = news.City,
                            thumbnail = news.Thumbnail,
                            description = news.Description,
                            topic_id = news.TopicId,
                            topic = new TopicDTO { id = news.Topic.Id, name = news.Topic.Name },
                            country_id = news.CountryId,
                            country = new CountryDTO { id = news.Country.Id, name = news.Country.Name },
                            created_at = Convert.ToDateTime(news.CreatedAt)
                        };

                        return Created(url, newsDTO);
                    }
                    else
                    {
                        return BadRequest("Vui lòng chọn file ảnh");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var msgs = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage);
            return BadRequest(string.Join(" | ", msgs));
        }


        [HttpPut]
        public IActionResult Update([FromForm] EditNews model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Tìm kiếm bản ghi News để cập nhật
                    News existingNews = _context.News.Find(model.id);

                    if (existingNews == null)
                    {
                        return NotFound("Không tìm thấy bài báo cần sửa");
                    }

                    // Kiểm tra xem model có chứa ảnh mới hay không
                    if (model.thumbnailFile != null && model.thumbnailFile.Length > 0)
                    {
                        // Tạo đường dẫn lưu trữ file trong thư mục "uploads"
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                        // Đảm bảo thư mục tồn tại
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Tạo tên file duy nhất để tránh trùng lặp
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.thumbnailFile.FileName);

                        // Kết hợp đường dẫn thư mục với tên file
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        // Lưu file vào đường dẫn
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            model.thumbnailFile.CopyTo(fileStream);
                        }
                        string url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

                        // Cập nhật đường dẫn ảnh mới
                        existingNews.Thumbnail = url;
                    }

                    // Cập nhật các thuộc tính khác từ model
                    existingNews.Name = model.name;
                    existingNews.City = model.city;
                    existingNews.Description = model.description;
                    existingNews.TopicId = model.topic_id;
                    existingNews.CountryId = model.country_id;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    _context.SaveChanges();

                    return Ok("Đã sửa bài báo thành công!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return BadRequest("Dữ liệu không hợp lệ");
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
