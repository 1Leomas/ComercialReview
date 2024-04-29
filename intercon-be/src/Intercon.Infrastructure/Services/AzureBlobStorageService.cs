using Azure.Storage;
using Azure.Storage.Blobs;
using Intercon.Application.Abstractions;
using Intercon.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Intercon.Infrastructure.Services;

public class AzureBlobStorageService : IBlobStorage
{
    public AzureBlobStorageService(
        IOptions<AzureBlobStorageSettings> azureBlobStorageSettings)
    {
        _azureBlobStorageSettings = azureBlobStorageSettings.Value;

        var storageAccount = _azureBlobStorageSettings.StorageAccount;
        var containerName = _azureBlobStorageSettings.ContainerName;

        string accessKey;

        using (var sr = new StreamReader(_azureBlobStorageSettings.AccessKeyPath))
        {
            try
            {
                accessKey = sr.ReadToEnd();
            }
            catch (Exception)
            {
                throw new ArgumentException("Missing '.secret' file in root folder");
            }
        }

        var credential = new StorageSharedKeyCredential(
            storageAccount,
            accessKey);

        var blobUri = new Uri($"https://{storageAccount}.blob.core.windows.net");
        _blobServiceClient = new BlobServiceClient(blobUri, credential);

        _blobLinkPrefix = $"https://{storageAccount}.blob.core.windows.net/{containerName}";
    }

    private readonly BlobServiceClient _blobServiceClient;
    private readonly AzureBlobStorageSettings _azureBlobStorageSettings;

    private readonly string _blobLinkPrefix;

    public async Task<string> UploadAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(_azureBlobStorageSettings.ContainerName);

        var filePath = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var blob = blobContainer.GetBlobClient(filePath);

        await using var fileStream = file.OpenReadStream();
        await blob.UploadAsync(fileStream, true, cancellationToken);

        return blob.Uri.AbsoluteUri;
    }

    public async Task<bool> DeleteAsync(string filePath, CancellationToken cancellationToken)
    {
        var blobContainer = _blobServiceClient.GetBlobContainerClient(_azureBlobStorageSettings.ContainerName);
        var blob = blobContainer.GetBlobClient(filePath.Replace($"{_blobLinkPrefix}", ""));

        var result = await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);

        return result;
    }
}