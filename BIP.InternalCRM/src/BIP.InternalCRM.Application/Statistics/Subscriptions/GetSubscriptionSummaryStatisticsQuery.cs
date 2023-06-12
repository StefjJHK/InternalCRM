using BIP.InternalCRM.Application.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Statistics.Subscriptions;

public record GetSubscriptionSummaryStatisticsQuery : IRequest<SubscriptionSummaryStatistics>
{
    public class Handler :
        IRequestHandler<GetSubscriptionSummaryStatisticsQuery, SubscriptionSummaryStatistics>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SubscriptionSummaryStatistics> Handle(
            GetSubscriptionSummaryStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            var stats = await _dbContext.Subscriptions
                .GroupBy(_ => 1, (_, subs) => new SubscriptionSummaryStatistics
                {
                    TotalSubscriptions = subs.Count(),
                    TotalCost = subs.Sum(s => s.Cost),
                    // NumberOfActive = subs.Count(s => s.ValidUntil.Millisecond > DateTime.UtcNow.Millisecond)
                })
                .FirstAsync(cancellationToken);

            return stats;
        }
    }
}