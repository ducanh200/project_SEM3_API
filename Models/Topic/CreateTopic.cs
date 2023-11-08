using System.ComponentModel.DataAnnotations;

namespace SEM3_API.Models.Topic
{
    public class CreateTopic
    {
        [Required(ErrorMessage = "Vui lòng nhập tên danh mục")]
        [MinLength(3, ErrorMessage = "Nhập tối thiểu 3 ký tự")]
        [MaxLength(255, ErrorMessage = "Nhập tối đa 255 ký tự")]
        public string name { get; set; }
    }
}
