using GymMgmt.Application.Common.Interfaces;
using MediatR;

namespace GymMgmt.Application.Behaviours
{
    internal class UnitOfWorkBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandBase
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next(cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
