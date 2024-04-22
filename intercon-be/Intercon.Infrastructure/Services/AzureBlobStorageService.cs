using Azure.Storage;
using Azure.Storage.Blobs;
using Intercon.Application.Abstractions;
using Intercon.Domain.Entities;
using Intercon.Infrastructure.Options;
using Intercon.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Intercon.Infrastructure.Services;

public class AzureBlobStorageService : IFileRepository
{
    public AzureBlobStorageService(
        InterconDbContext context, 
        IOptions<AzureBlobStorageSettings> azureBlobStorageSettings)
    {
        _context = context;
        _azureBlobStorageSettings = azureBlobStorageSettings.Value;

        var storageAccount = _azureBlobStorageSettings.StorageAccount;
        var containerName = _azureBlobStorageSettings.ContainerName;

        var credential = new StorageSharedKeyCredential(
            storageAccount, 
            _azureBlobStorageSettings.AccessKey);

        var blobUri = new Uri($"https://{storageAccount}.blob.core.windows.net");
        _blobServiceClient = new BlobServiceClient(blobUri, credential);

        _blobLinkPrefix = $"https://{storageAccount}.blob.core.windows.net/{containerName}";
    }

    private readonly BlobServiceClient _blobServiceClient;
    private readonly InterconDbContext _context;
    private readonly AzureBlobStorageSettings _azureBlobStorageSettings;

    private readonly string _blobLinkPrefix;

    private async Task<string> UploadFileToAzureAsync(IFormFile file)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(_azureBlobStorageSettings.ContainerName);

        var filePath = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var blob = blobContainer.GetBlobClient(filePath);

        await using var fileStream = file.OpenReadStream();
        await blob.UploadAsync(fileStream, true);

        return blob.Uri.AbsoluteUri;
    }

    private async Task<bool> DeleteFileFromAzureAsync(string filePath)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(_azureBlobStorageSettings.ContainerName);
        var blob = blobContainer.GetBlobClient(filePath.Replace($"{_blobLinkPrefix}", ""));

        return await blob.DeleteIfExistsAsync();
    }

    public async Task<FileData?> UploadFileAsync(IFormFile imageData, CancellationToken cancellationToken)
    {
        var filePath = await UploadFileToAzureAsync(imageData);

        var fileData = new FileData
        {
            Path = filePath,
        };

        _context.DataFiles.Add(fileData);

        await _context.SaveChangesAsync(cancellationToken);

        return fileData;
    }

    public async Task DeleteFileAsync(int id, CancellationToken cancellationToken)
    {
        var filePath = await _context.DataFiles
            .Where(x => x.Id == id).Select(x => x.Path)
            .FirstOrDefaultAsync(cancellationToken);

        if (filePath == null)
        {
            throw new InvalidOperationException("File not found in db");
        }

        var result = await DeleteFileFromAzureAsync(filePath);

        if (!result)
        {
            throw new InvalidOperationException("File not found in Azure Blob Storage");
        }
    }
}