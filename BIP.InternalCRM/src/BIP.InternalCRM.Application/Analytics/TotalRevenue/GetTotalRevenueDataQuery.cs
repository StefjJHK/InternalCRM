using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Analytics.TotalRevenue;

public record GetTotalRevenueDataQuery(
    int Year,
    FinancialQuarter? Quarter,
    string? ProductName = null
) : IRequest<GetTotalRevenueDataQuery.Result>
{
    public record Result(IReadOnlyCollection<Model> Revenue);

    public record Model(int Week, decimal Amount);

    public class Handler
        : IRequestHandler<GetTotalRevenueDataQuery, Result>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> Handle(
            GetTotalRevenueDataQuery request,
            CancellationToken cancellationToken)
        {
            var payments = await _dbContext.Invoices
                .UseIf(
                    request.ProductName.IsNotEmpty(),
                    q => q
                        .Join(
                            _dbContext.Products,
                            i => i.ProductId,
                            p => p.Id,
                            (i, p) => new { i, ProductName = p.Name })
                        .Where(j => j.ProductName == request.ProductName)
                        .Select(j => j.i))
                .SelectMany(_ => _.Payments)
                .Select(_ => new { _.Amount, _.ReceivedDate })
                .ToListAsync(cancellationToken);

            var data = payments
                .Where(_ => _.ReceivedDate.Year == request.Year)
                .Where(_ => request.Quarter == FinancialQuarter.All
                            || _.ReceivedDate.GetFinancialQuarter() == request.Quarter)
                .GroupBy(_ => _.ReceivedDate.GetIso8601WeekOfYear())
                .Select(_ => new Model(_.Key, _.Sum(_ => _.Amount)))
                .OrderBy(_ => _.Week)
                .ToList();

            return new(data);
        }
    }
}