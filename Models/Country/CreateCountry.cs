using System.ComponentModel.DataAnnotations;

namespace SEM3_API.Models.Country
{
    public class CreateCountry
    {
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        [MinLength(3, ErrorMessage = "Nhập tối thiểu 3 ký tự")]
        [MaxLength(255, ErrorMessage = "Nhập tối đa 255 ký tự")]
        public string name { get; set; }
    }
}
