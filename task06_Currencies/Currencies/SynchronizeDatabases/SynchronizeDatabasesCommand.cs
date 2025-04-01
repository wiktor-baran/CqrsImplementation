using MediatR;

namespace task06_Currencies.Currencies.SynchronizeDatabases
{
    public record SynchronizeDatabasesCommand : IRequest<bool>;
}
