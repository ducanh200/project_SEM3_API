using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEM3_API.DTOs;
using SEM3_API.Entities;
using SEM3_API.Models.Donate;
using SEM3_API.Models;

namespace SEM3_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonateController : ControllerBase
    {
        private readonly Sem3ApiContext _context;

        public DonateController(Sem3ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ListDonate()
        {
            var allDonates = _context.Donates.ToList();

            if (allDonates == null || allDonates.Count == 0)
            {
                return BadRequest("Không có đóng góp nào trong cơ sở dữ liệu!");
            }

            // Chuyển đổi danh sách các đóng góp sang đối tượng DTO nếu cần
            List<DonateDTO> donateDTOs = allDonates.Select(d => new DonateDTO
            {
                id = d.Id,
                amount = d.Amount,
                project_id = d.ProjectId,
                user_id = d.UserId,
                created_at = Convert.ToDateTime(d.CreateAt)
            }).ToList();

            return Ok(donateDTOs);
        }

        [HttpPost("createdonate")]
        public IActionResult CreateDonate([FromBody] CreateDonate createDonate)
        {
            if (createDonate == null)
            {
                return BadRequest("Invalid request body");
            }

            // Tạo một đối tượng Donate từ model CreateDonate
            Donate newDonation = new Donate
            {
                Amount = createDonate.amount,
                UserId = createDonate.user_id,
                ProjectId = createDonate.project_id,
                CreateAt = DateTime.UtcNow
            };

            // Thêm đối tượng mới vào database
            _context.Donates.Add(newDonation);
            _context.SaveChanges();

            return Ok(newDonation);
        }
    }
}
