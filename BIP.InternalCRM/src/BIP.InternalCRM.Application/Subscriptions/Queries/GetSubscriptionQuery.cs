using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Subscriptions.QueryResults;
using BIP.InternalCRM.Domain.Subscriptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Subscriptions.Queries;

public record GetSubscriptionQuery(
    string Number
) : IRequest<OneOf<SubscriptionQueryResult, NotFound<Subscription>>>
{
    public class Handler :
        IRequestHandler<GetSubscriptionQuery, OneOf<SubscriptionQueryResult, NotFound<Subscription>>>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OneOf<SubscriptionQueryResult, NotFound<Subscription>>> Handle(
            GetSubscriptionQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _dbContext.Invoices
                .AsNoTracking()
                .Include(_ => _.Subscriptions
                    .Where(sub => sub.Number == request.Number))
                .FirstOrDefaultAsync(cancellationToken);

            return result is not null
                ? new SubscriptionQueryResult(result.Subscriptions.First(), result.Number)
                : new NotFound<Subscription>();
        }
    }
}