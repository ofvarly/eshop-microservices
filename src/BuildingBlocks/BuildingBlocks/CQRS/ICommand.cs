using MediatR;

namespace BuildingBlocks.CQRS
{
    // An interface that represents a command in the CQRS pattern.
    // A command is a request to perform an action that changes the state of the system.
    // It is a request to perform an action, not a query to retrieve data.
    // Unit is a value type that represents a void return type. (Provided by the MediatR library)
    // Abstracts the commands that do not return a response. (e.g. void methods, like deleting a product)
    public interface ICommand : ICommand<Unit>
    {

    }

    // An interface that represents a command in the CQRS pattern that returns a response of type TResponse.
    // TResponse is the type of the response that the command returns.
    // TResponse is a generic type parameter that represents the type of the response.
    // This interface inherits from IRequest<TResponse> which is provided by the MediatR library.
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {

    }
    
}
