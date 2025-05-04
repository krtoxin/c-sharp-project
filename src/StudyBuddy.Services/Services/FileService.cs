using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using StudyBuddy.Services.IServices;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveTaskImageAsync(IBrowserFile file)
    {
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "tasks");
        Directory.CreateDirectory(uploadsFolder); 

        var fileName = $"{Guid.NewGuid()}_{file.Name}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024); 
        await using var fs = File.Create(filePath);
        await stream.CopyToAsync(fs);

        return $"/uploads/tasks/{fileName}"; 
    }
}
