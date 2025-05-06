using MediatR;

namespace BuildingBlocks.CQRS
{
    // An interface that represents a command handler in the CQRS pattern.
    // A command handler is a class that handles a command and performs the action that the command requests.
    // It is a class that implements the ICommandHandler interface and provides the logic to handle the command.
    // TCommand is the type of the command that the handler handles.
    // TResponse is the type of the response that the command returns.
    public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
        where TCommand : ICommand<Unit>
    {
    }


    // An interface that represents a command handler in the CQRS pattern that returns a response of type TResponse.
    // TCommand is the type of the command that the handler handles.
    // TResponse is the type of the response that the command returns.
    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
        where TResponse : notnull
    {
    }
    
 
}
