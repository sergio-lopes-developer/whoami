using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using WhoAmI.Application.Abstractions.Commands;
using WhoAmI.Application.Abstractions.Dispatching.Commands;
using WhoAmI.Application.Abstractions.Dispatching.Queries;
using WhoAmI.Application.Abstractions.Queries;
using WhoAmI.Application.Results;
using WhoAmI.Infrastructure.Data.Persistence.Context;

namespace WhoAmI.IntegrationTests.Infrastructure;

public sealed class IntegrationTestScope : IAsyncDisposable {
    private readonly IServiceScope _scope;

    private readonly IDbContextTransaction? _transaction;

    private IntegrationTestScope(
        IServiceScope scope,
        IDbContextTransaction? transaction
    ) {
        _scope = scope;
        _transaction = transaction;
    }

    public static async Task<IntegrationTestScope> CreateAsync(
        IServiceProvider rootProvider,
        bool useTransaction = true
    ) {
        var scope = rootProvider.CreateScope();

        if (!useTransaction) {
            return new IntegrationTestScope(scope, null);
        }

        var db = scope.ServiceProvider.GetRequiredService<WhoAmIDbContext>();

        var transaction = await db.Database.BeginTransactionAsync();

        return new IntegrationTestScope(scope, transaction);
    }

    private T Get<T>() where T : notnull =>
        _scope.ServiceProvider.GetRequiredService<T>();

    public Task<Result<TResult>> Execute<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default
    ) {
        var dispatcher = Get<ICommandDispatcher>();
        return dispatcher.Send(command, cancellationToken);
    }

    public Task<Result<TResult>> Query<TResult>(
        IQuery<TResult> query,
        CancellationToken cancellationToken = default
    ) {
        var dispatcher = Get<IQueryDispatcher>();
        return dispatcher.Send(query, cancellationToken);
    }

    public async ValueTask DisposeAsync() {
        if (_transaction is not null) {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        _scope.Dispose();
    }
}
