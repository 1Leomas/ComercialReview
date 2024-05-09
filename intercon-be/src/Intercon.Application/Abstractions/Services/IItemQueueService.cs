namespace Intercon.Application.Abstractions.Services;

public interface IItemQueueService<T>
{
    void Enqueue(T item);
    Task<T?> DequeueAsync(CancellationToken cancellationToken);
}