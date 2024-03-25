using Intercon.Application.Abstractions;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Intercon.Infrastructure.Repositories;

//public class FileRepository(InterconDbContext context) : IFileRepository
//{
//    public async Task<FileData?> UploadFileAsync(IFormFile imageData, CancellationToken cancellationToken)
//    {
//        var folderName = "intercon_uploads";

//        if (!Directory.Exists($"/{folderName}"))
//        {
//            Directory.CreateDirectory($"/{folderName}");
//        }

//        var filePath = $"/{folderName}/{Guid.NewGuid()}{Path.GetExtension(imageData.FileName)}";
        
//        using (var fileStream = new FileStream(filePath, FileMode.Create))
//        {
//            await imageData.CopyToAsync(fileStream, cancellationToken);
//        }

//        var fileData = new FileData
//        {
//            Path = filePath,
//        };

//        context.DataFiles.Add(fileData);

//        await context.SaveChangesAsync(cancellationToken);

//        return fileData;
//    }

//    public async Task DeleteFileAsync(int id, CancellationToken cancellationToken)
//    {
//        var file = await context.DataFiles
//            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

//        if (file is null) return;

//        try
//        {
//            if (File.Exists(file.Path)) File.Delete(file.Path);
//        }
//        catch (IOException ioExp)
//        {
//            throw new IOException("Can not delete file", ioExp);
//        }

//        context.DataFiles.Remove(file);

//        await context.SaveChangesAsync(cancellationToken);
//    }
//}