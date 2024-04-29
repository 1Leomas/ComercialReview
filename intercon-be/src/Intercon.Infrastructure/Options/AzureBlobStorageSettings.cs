namespace Intercon.Infrastructure.Options;

public class AzureBlobStorageSettings
{
    public string StorageAccount { get; set; } = null!;
    public string AccessKeyPath { get; set; } = null!;
    public string ContainerName { get; set; } = null!;
}