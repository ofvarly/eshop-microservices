using MediatR;

namespace BuildingBlocks.CQRS
{
    // / An interface that represents a query handler in the CQRS pattern.
    // A query handler is a class that handles a query and returns the data that the query requests.
    // It is a class that implements the IQueryHandler interface and provides the logic to handle the query.
    // TQuery is the type of the query that the handler handles.
    // TResponse is the type of the response that the query returns.
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : notnull
    {
    }
}
