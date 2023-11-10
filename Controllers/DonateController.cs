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
                project_id = d.ProjectId,
                user_id = d.UserId,
                create_at = DateTime.Now,
            }).ToList();

            return Ok(donateDTOs);
        }

        [HttpPost("contribute")]
        public IActionResult Contribute(CreateDonate model)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                // Log lỗi và trả về thông báo lỗi
                return BadRequest($"Lỗi xử lý thanh toán: {ex.Message}");
            }
            return Ok();
        }
    }
}
