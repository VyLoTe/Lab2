using Lab2.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.ViewComponents
{
    public class MajorViewComponent : ViewComponent
    {
        StudentDbContext db;
        List<Major> majors;
        public MajorViewComponent(StudentDbContext _context)
        {
            db = _context;
            majors = db.Majors.ToList();
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("RenderMajor", majors);
        }
    }
}
