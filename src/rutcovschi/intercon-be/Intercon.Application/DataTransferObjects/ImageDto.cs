namespace Intercon.Application.DataTransferObjects;

public record ImageDto(int Id, string ContentType, byte[] Raw);
