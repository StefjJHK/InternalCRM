using BIP.InternalCRM.Application.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Analytics.TotalSales;

public record GetTotalSalesDataQuery(
    DateTime StartDate,
    DateTime EndDate
) : IRequest<GetTotalSalesDataQuery.Result>
{
    public record Result(decimal Total, IReadOnlyCollection<Model> Sales);
    
    public record Model(int Year, decimal TotalSales);
    
    public class Handler :
        IRequestHandler<GetTotalSalesDataQuery, Result>
    {
        private readonly IDomainDbContext _domainDbContext;

        public Handler(IDomainDbContext domainDbContext)
        {
            _domainDbContext = domainDbContext;
        }

        public async Task<Result> Handle(GetTotalSalesDataQuery request, CancellationToken cancellationToken)
        {
            var payments = await _domainDbContext.Payments
                .AsNoTracking()
                .Select(_ => new { _.Amount, _.ReceivedDate })
                .ToListAsync(cancellationToken);

            var data = payments
                .Where(_ => _.ReceivedDate.Year >= request.StartDate.Year)
                .Where(_ => _.ReceivedDate.Year <= request.EndDate.Year)
                .GroupBy(_ => _.ReceivedDate.Month)
                .Select(_ => new Model(_.Key, _.Sum(x => x.Amount)))
                .ToList();

            return new(data.Sum(_ => _.TotalSales), data);
        }
    }
}