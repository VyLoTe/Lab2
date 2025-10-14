using System.ComponentModel.DataAnnotations;

namespace Lab2.Models
{
    public class Student
    {
        public int Id { get; set; } // Mã sinh viên

        [Required(ErrorMessage = "Tên bắt buộc phải được nhập")]
        [RegularExpression(@"^[\p{Lu}][\p{Ll}]+( [\p{Lu}][\p{Ll}]+)*$", ErrorMessage = "Tên không hợp lệ.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Tên phải từ 4 đến 100 ký tự.")]
        public string? Name { get; set; } // Họ tên

        [Required(ErrorMessage = "Email bắt buộc phải được nhập")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@gmail\.com")]
        public string? Email { get; set; } // Email

        [StringLength(100, MinimumLength = 8)]
       
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[@#$%^&*!?])[A-Za-z0-9@#$%^&*!?]+$", ErrorMessage ="Mật khẩu không hợp lệ")]
        public string? Password { get; set; } // Mật khẩu

        [Required(ErrorMessage ="Giới tính bắt buộc phải được chọn")]
        public Gender? Gender { get; set; } // Giới tính
     
        public bool IsRegular { get; set; } // Hệ: true = chính quy, false = phi chính quy

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage ="Địa chỉ bắt buộc phải được nhập")]
        public string? Address { get; set; } // Địa chỉ

        [Range(typeof(DateTime), "1/1/1963", "12/31/2005")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage ="Ngày sinh bắt buộc phải được nhập")]
        public DateTime DateOfBorth { get; set; } // Ngày sinh

        //avatar path string
        public string? AvatarPath { get; set; }
        [Required(ErrorMessage ="Điểm số bắt buộc phải được nhập")]
        [Range(0.0, 10.0, ErrorMessage ="Điểm số từ 0.0 tới 10.0")]
        public float Score { get; set; }

        public virtual Major? Major { get; set; } // Khóa ngoại
        public int MajorID { get; set; }
    }
}
