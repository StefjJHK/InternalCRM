using BIP.InternalCRM.Application.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Statistics.Leads;

public record GetLeadSummaryStatisticsQuery :
    IRequest<LeadSummaryStatistics>
{
    public class Handler :
        IRequestHandler<GetLeadSummaryStatisticsQuery, LeadSummaryStatistics>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LeadSummaryStatistics> Handle(GetLeadSummaryStatisticsQuery request, CancellationToken cancellationToken)
        {
            var result = await (
                    from lead in _dbContext.Leads
                    group new { lead.ProductId, lead.Cost } by lead
                    into grp
                    select new LeadSummaryStatistics
                    {
                        TotalLeads = grp.Count(),
                        TotalProducts = grp.Select(_ => _.ProductId).Count(),
                        TotalCost = grp.Select(_ => _.Cost).Sum()
                    })
                .AsNoTracking()
                .FirstAsync(cancellationToken);

            return result;
        }
    }
}