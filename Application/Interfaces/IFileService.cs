using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IFileService
{
    Task<string> SaveEventImageAsync(IFormFile file);
}
