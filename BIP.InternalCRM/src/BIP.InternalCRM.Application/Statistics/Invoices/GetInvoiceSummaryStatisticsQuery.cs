using BIP.InternalCRM.Application.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Statistics.Invoices;

public record GetInvoiceSummaryStatisticsQuery : IRequest<InvoiceSummaryStatistics>
{
    public class Handler :
        IRequestHandler<GetInvoiceSummaryStatisticsQuery, InvoiceSummaryStatistics>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<InvoiceSummaryStatistics> Handle(GetInvoiceSummaryStatisticsQuery request, CancellationToken cancellationToken)
        {
            var stats = await _dbContext.Invoices
                .GroupBy(_ => 1, (_, invoices) => new InvoiceSummaryStatistics
                {
                    TotalInvoices = invoices.Count(),
                    TotalAmount = invoices.Sum(_ => _.Amount),
                    NumberOfOverdue = invoices.Count(_ => _.IsOverdue),
                    NumberOfPaid = invoices.Count(_ => _.PaidDate.HasValue)
                })
                .FirstAsync(cancellationToken);

            return stats;
        }
    }
}