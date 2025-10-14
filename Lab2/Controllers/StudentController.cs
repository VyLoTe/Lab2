using Lab2.Data;
using Lab2.Interfaces;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Controllers
{
    [Route("Admin/Student")]
    public class StudentController : Controller
    {
        private readonly StudentDbContext db;

        private readonly IBufferedFileUploadService _bufferedFileUploadService;

        // Dùng static để dữ liệu tồn tại qua các request
        public StudentController(IBufferedFileUploadService bufferedFileUploadService, StudentDbContext _context)
        {
            _bufferedFileUploadService = bufferedFileUploadService;
            db = _context;
        }
        //khai báo biến toàn cục pageSize
        private int pageSize = 2;
        // Index với sort
        [Route("List", Name = "StudentList")]
        public async Task<IActionResult> Index(string sort = "asc", int? mid = null)
        {
            // Lấy dữ liệu ban đầu
            var students = (IQueryable<Student>)db.Students
                .Include(s => s.Major);

            // Lọc theo ngành nếu có
            if (mid != null)
            {
                students = (IQueryable<Student>)db.Students
                    .Where(s => s.MajorID == mid)
                    .Include(s => s.Major);
            }

            // Sắp xếp tăng / giảm
            students = sort == "desc"
                ? students.OrderByDescending(s => s.Id)
                : students.OrderBy(s => s.Id);

            // Tính số trang
            int pageNum = (int)Math.Ceiling(students.Count() / (float)pageSize);

            // Trả thông tin về view
            ViewBag.pageNum = pageNum;
            ViewBag.Sort = sort;
            ViewBag.Mid = mid;
            ViewBag.TotalStudents = db.Students.Count();

            // Lấy dữ liệu trang đầu tiên
            var result = students.Take(pageSize).ToList();

            return View(result);
        }
        [Route("StudentFilter")]
        public IActionResult StudentFilter(int? mid, string? keyword, int? pageIndex)
        {
            // lấy toàn bộ students trong dbset chuyển về IQueryable<Student> để query
            var students = (IQueryable<Student>)db.Students;

            // lấy chỉ số trang, nếu chỉ số trang null thì gán ngầm định bằng 1
            int page = (int)(pageIndex == null || pageIndex <= 0 ? 1 : pageIndex);

            // nếu có mid thì lọc student theo mid (chuyên ngành)
            if (mid != null)
            {
                // lọc
                students = students.Where(s => s.MajorID == mid);

                // gửi mid về view để ghi lại trên nav-phân trang
                ViewBag.mid = mid;
            }

            // nếu có keyword thì tìm kiếm theo tên
            if (keyword != null)
            {
                // tìm kiếm
                students = students.Where(s => s.Name.ToLower()
                    .Contains(keyword.ToLower()));

                // gửi keyword về view để ghi lại trên nav-phân trang
                ViewBag.keyword = keyword;
            }

            // tính số trang
            int pageNum = (int)Math.Ceiling(students.Count() / (float)pageSize);

            // gửi số trang về view để hiển thị nav-trang
            ViewBag.pageNum = pageNum;

            // chọn dữ liệu trong trang hiện tại
            var result = students
                .Skip(pageSize * (page - 1))
                .Take(pageSize)
                .Include(s => s.Major)
                .ToList();

            return PartialView("StudentTable", result);
        }


        // GET: hiển thị form thêm student
        [HttpGet("Add", Name = "AddStudent")]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            ViewBag.AllMajors = new SelectList(await db.Majors.ToListAsync(), "MajorID", "MajorName");

            return View();
        }

        // POST: thêm student
        [HttpPost("Add")]
        public async Task<IActionResult> Create(Student s, IFormFile file)
        {
            if (file != null)
            {
                var avatarPath = await _bufferedFileUploadService.UploadFile(file);
                if (avatarPath != null) s.AvatarPath = avatarPath;
            }

            if (ModelState.IsValid)
            {

                db.Students.Add(s);
                await db.SaveChangesAsync();
                // Redirect để sort toggle hoạt động bình thường
                return RedirectToAction("Index");
            }

            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            ViewBag.AllMajors = new SelectList(await db.Majors.ToListAsync(), "MajorID", "MajorName");

            return View(s);
        }

        // Tìm kiếm student
        [HttpGet("Search", Name = "SearchStudent")]
        public IActionResult Search(string? keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return RedirectToAction("Index");

            var students = db.Students
                .AsNoTracking()
                .Where(s => s.Name != null && s.Name.ToLower().Contains(keyword.ToLower()))
                .ToList();


            if (!students.Any())
            {
                TempData["ErrorMessage"] = $"No student found with '{keyword}'";
                return RedirectToAction("Index");
            }

            return View("Index", students);
        }
        
        
        [HttpGet("Delete/{id}", Name = "DeleteStudent")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await db.Students
                .Include(s => s.Major)
                .AsNoTracking() // chỉ đọc, không theo dõi trạng thái
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return NotFound();

            return View(student); // hiển thị trang xác nhận
        }

        [HttpPost("Delete/{id}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await db.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            db.Students.Remove(student);
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpGet("Edit/{id}", Name = "EditStudent")]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await db.Students.FindAsync(id);
            if (student == null) return NotFound();
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            // Lấy danh sách Major từ DB
            var majors = await db.Majors.ToListAsync() ?? new List<Major>();
            ViewBag.AllMajors = new SelectList(majors, "MajorID", "MajorName", student.MajorID);

            return View(student);
        }
        [HttpPost("Edit/{id}")]
        [ActionName("Edit")]
        public async Task<IActionResult> EditConfirmed(int id, IFormFile? file, Student s)
        {
            var student = await db.Students.FindAsync(id);
            if (student == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    var avatarPath = await _bufferedFileUploadService.UploadFile(file);
                    if (!string.IsNullOrEmpty(avatarPath))
                        student.AvatarPath = avatarPath;
                }

                // Nếu người dùng nhập password mới → cập nhật
                if (!string.IsNullOrWhiteSpace(s.Password))
                    student.Password = s.Password;

                student.Name = s.Name;
                student.Email = s.Email;
                student.Address = s.Address;
                student.DateOfBorth = s.DateOfBorth;
                student.Gender = s.Gender;
                student.IsRegular = s.IsRegular;
                student.MajorID = s.MajorID;
                student.Score = s.Score;

                db.Students.Update(student);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AllGenders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            var majorsReload = await db.Majors.ToListAsync() ?? new List<Major>();
            ViewBag.AllMajors = new SelectList(majorsReload, "MajorID", "MajorName", s.MajorID);

            return View(s);
        }

        public async Task<IActionResult> StudentByMajorID(int mid)
        {
            Console.WriteLine($"StudentByMajorID called with mid = {mid}");
            var students = await db.Students
                .Include(s => s.Major)
                .Where(s => s.MajorID == mid)
                .ToListAsync();
            return PartialView("StudentTable", students);
        }
    }
}
