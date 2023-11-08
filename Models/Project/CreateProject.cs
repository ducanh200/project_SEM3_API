using System.ComponentModel.DataAnnotations;

namespace SEM3_API.Models.Project
{
    public class CreateProject
    {
        [Required(ErrorMessage = "vui lòng nhập tên danh mục ")]
        [MinLength(3, ErrorMessage = "nhập tối thiểu 3 kí tự")]
        [MaxLength(255, ErrorMessage = "nhập tối đa 255 kí tự")]
        public string name { get; set; }

        [Required(ErrorMessage = "vui lòng nhập file ảnh")]
        public string thumbnail_1 { get; set; }

        [Required(ErrorMessage = "vui lòng nhập file ảnh")]
        public string thumbnail_2 { get; set; }

        [Required(ErrorMessage = "vui lòng nhập tiền vốn")]
        public decimal fund { get; set; }

        [Required(ErrorMessage = "vui lòng nhập mô tả")]
        public string description { get; set; }

        [Required(ErrorMessage = "vui lòng nhập tên thành phố")]
        public string city { get; set; }

        [Required(ErrorMessage = "vui lòng nhập tên địa chỉ")]
        public string address { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int countryId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int topicId { get; set; }

        [Required(ErrorMessage ="Vui lòng nhập thời điểm")]
        public DateTime begin { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thời điểm")]
        public DateTime finish { get; set; }
    }
}
