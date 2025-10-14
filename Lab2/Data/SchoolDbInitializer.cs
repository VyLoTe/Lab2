using Lab2.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Data
{
    public class SchoolDbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new StudentDbContext(serviceProvider.GetRequiredService<DbContextOptions<StudentDbContext>>()))
            {
               
                context.Database.Migrate();

                var students = new Student[]
                {
                    new Student
                    {
                       
                        Name = "Nguyen Van A",
                        Email = "nguyenvana@gmail.com",
                        Password = "Abc@1234",
                        
                        Gender = Gender.Male,
                        IsRegular = true,
                        Address = "123 Đường Lê Lợi, Hà Nội",
                        DateOfBorth = new DateTime(2001, 5, 10),
                        AvatarPath = "/UploadedFiles/aothat.png",
                        Score = 8.5f
                    },
                    new Student
                    {
                        
                        Name = "Tran Thi B",
                        Email = "tranthib@gmail.com",
                        Password = "Bcd@5678",
                        
                        Gender = Gender.Female,
                        IsRegular = false,
                        Address = "456 Nguyễn Huệ, TP.HCM",
                        DateOfBorth = new DateTime(1999, 11, 23),
                        AvatarPath = "/UploadedFiles/emvy.png",
                        Score = 9.2f
                    },
                    new Student
                    {
                      
                        Name = "Le Hoang C",
                        Email = "lehoangc@gmail.com",
                        Password = "Cde@9012",
                        
                        Gender = Gender.Male,
                        IsRegular = true,
                        Address = "789 Lý Thường Kiệt, Đà Nẵng",
                        DateOfBorth = new DateTime(2000, 8, 15),
                        AvatarPath = "/UploadedFiles/3.png",
                        Score = 7.3f
                    }
                };
                foreach (var s in students)
                {
                    var existing = context.Students.FirstOrDefault(x => x.Email == s.Email);
                    if (existing == null)
                    {
                        // Nếu chưa có, thêm mới
                        context.Students.Add(s);
                    }
                    else
                    {
                        // Nếu có rồi, cập nhật dữ liệu
                        existing.Name = s.Name;
                        existing.Score = s.Score;
                        existing.Address = s.Address;
                        existing.AvatarPath = s.AvatarPath;
                        
                        existing.Gender = s.Gender;
                        existing.IsRegular = s.IsRegular;
                        existing.DateOfBorth = s.DateOfBorth;
                    }
                }
                context.SaveChanges();
                var majors = new Major[]
                {
                    new Major{ MajorName ="CNTT"},
                    new Major{ MajorName ="Kinh Tế"},
                    new Major{ MajorName ="Công trình"},
                    new Major{ MajorName ="Điện - Điện tử"},
                };
                foreach (var major in majors)
                {
                    var existingMajor = context.Majors.FirstOrDefault(x => x.MajorName == major.MajorName);
                    if (existingMajor == null)
                    {
                        context.Majors.Add(major);
                    }
                    else
                    {
                        existingMajor.MajorName = major.MajorName;
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
