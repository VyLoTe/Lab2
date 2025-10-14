using Lab2.Interfaces;

namespace Lab2.Services
{
    public class BufferedFileUploadLocalService : IBufferedFileUploadService
    {
        public async Task<string?> UploadFile(IFormFile file)
        {
            string path = "";
            try
            {
                if (file.Length > 0)
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return "/UploadedFiles/" + file.FileName;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
            return null;
        }
    }
}
