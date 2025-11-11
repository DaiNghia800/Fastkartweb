namespace Fastkart.Services
{
    public interface IUploadService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
