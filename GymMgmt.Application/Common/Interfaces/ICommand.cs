using MediatR;


namespace GymMgmt.Application.Common.Interfaces
{
    public interface ICommand<out TResponse> : IRequest<TResponse>, ICommandBase
    {
    }

    public interface ICommand : IRequest, ICommandBase
    {
    }

    public interface ICommandBase
    {
    }
}
