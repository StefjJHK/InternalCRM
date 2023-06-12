using BIP.InternalCRM.Application.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Statistics.Payments;

public record GetPaymentSummaryStatisticsQuery : IRequest<PaymentSummaryStatistics>
{
    public class Handler :
        IRequestHandler<GetPaymentSummaryStatisticsQuery, PaymentSummaryStatistics>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaymentSummaryStatistics> Handle(
            GetPaymentSummaryStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            var stats = await _dbContext.Payments
                .GroupBy(_ => 1, (_, payments) => new PaymentSummaryStatistics
                {
                    TotalPayments = payments.Count(),
                    TotalAmount = payments.Sum(p => p.Amount),
                    TotalOverdue = payments.Count(p => p.IsOverdue)
                })
                .FirstAsync(cancellationToken);

            return stats;
        }
    }
}