using System.ComponentModel.DataAnnotations;

namespace SEM3_API.Models.News
{
    public class EditNews
    {
        [Required]
        public int id { get; set; }

        [Required(ErrorMessage = "vui lòng nhập tên danh mục ")]
        public string name { get; set; }
        [Required(ErrorMessage = "vui lòng nhập thành phố")]
        public string city { get; set; }
        [Required(ErrorMessage = "vui lòng nhập file ảnh")]
        public IFormFile thumbnailFile { get; set; }

        [Required(ErrorMessage = "vui lòng nhập nội dung bài báo")]
        public string description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn quốc gia")]
        public int country_id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int topic_id { get; set; }
    }
}
