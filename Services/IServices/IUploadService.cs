namespace Fastkart.Services.IServices
{
    public interface IUploadService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
