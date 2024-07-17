using Microsoft.Extensions.Caching.Distributed;
namespace PTG.NextStep.Domain
{
    public interface IBackgroundServiceMiddleware
    {
        Task InvokeAsync(CancellationToken cancellationToken, Func<CancellationToken, Task> next);
    }
}
