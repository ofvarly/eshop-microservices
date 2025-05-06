using MediatR;

namespace BuildingBlocks.CQRS
{
    // An interface that represents a query in the CQRS pattern.
    // A query is a request to retrieve data from the system.
    // It is a request to retrieve data, not a command to perform an action.
    // TResponse is the type of the response that the query returns.
    // TResponse is a generic type parameter that represents the type of the response.
    // where TResponse : notnull is a constraint that specifies that TResponse cannot be null. Because since we are executing a query, we expect a response.
    public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull
    {
    }

    
}
