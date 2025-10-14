namespace Lab2.Interfaces
{
    public interface IBufferedFileUploadService
    {
        Task<string?> UploadFile(IFormFile file);
    }
}
