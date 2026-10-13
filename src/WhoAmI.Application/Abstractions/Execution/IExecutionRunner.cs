using WhoAmI.Application.Results;

namespace WhoAmI.Application.Abstractions.Execution;

public interface IExecutionRunner {
    public Task<TResult> ExecuteAsync<TResult>(
        string operation,
        object request,
        Func<Task<TResult>> handler
    ) where TResult : ResultBase;
}
