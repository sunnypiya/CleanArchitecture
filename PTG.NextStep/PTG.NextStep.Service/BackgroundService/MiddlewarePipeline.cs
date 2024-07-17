using PTG.NextStep.Domain;

public class MiddlewarePipeline
{
    private readonly IList<Func<IBackgroundServiceMiddleware>> _middlewareFactories;

    public MiddlewarePipeline(IList<Func<IBackgroundServiceMiddleware>> middlewareFactories)
    {
        _middlewareFactories = middlewareFactories;
    }

    public async Task InvokeAsync(CancellationToken cancellationToken, Func<CancellationToken, Task> finalTask)
    {
        Func<CancellationToken, Task> next = finalTask;

        for (int i = _middlewareFactories.Count - 1; i >= 0; i--)
        {
            var middlewareFactory = _middlewareFactories[i];
            var middleware = middlewareFactory();
            var currentNext = next;

            next = async ct => await middleware.InvokeAsync(ct, currentNext);
        }

        await next(cancellationToken);
    }
}