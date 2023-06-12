using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Domain.Subscriptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Success = OneOf.Types.Success;

namespace BIP.InternalCRM.Application.Subscriptions.Commands;

public record RemoveSubscriptionCommand(
    string Number
) : IRequest<OneOf<Success, NotFound>>
{
    public class Handler :
        IRequestHandler<RemoveSubscriptionCommand, OneOf<Success, NotFound>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Success, NotFound>> Handle(RemoveSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var sub = await _dbContext.Subscriptions
                .Where(_ => _.Number == request.Number)
                .FirstOrDefaultAsync(cancellationToken);

            if (sub is null) return new NotFound<Subscription>();
            
            var removeResult = sub.Remove();

            _dbContext.Subscriptions.Remove(sub);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return removeResult;
        }
    }
}