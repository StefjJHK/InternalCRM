using BIP.InternalCRM.Application.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Analytics.TotalCustomers;

public record GetTotalCustomersDataQuery(
    DateTime StartDate,
    DateTime EndDate
) : IRequest<GetTotalCustomersDataQuery.Result>
{
    public record Result(
        int Total,
        IReadOnlyCollection<Model> Customers
    );

    public record Model(int Month, int TotalCustomers);

    public class Handler :
        IRequestHandler<GetTotalCustomersDataQuery, Result>
    {
        private readonly IDomainDbContext _domainDbContext;

        public Handler(IDomainDbContext domainDbContext)
        {
            _domainDbContext = domainDbContext;
        }

        public async Task<Result> Handle(
            GetTotalCustomersDataQuery request,
            CancellationToken cancellationToken)
        {
            var customers = await _domainDbContext.Invoices
                .AsNoTracking()
                .Where(_ => _.PaidDate.HasValue)
                .Select(_ => new { _.CustomerId, _.PaidDate })
                .ToListAsync(cancellationToken);

            var data = customers
                .Where(_ => _.PaidDate!.Value.Year >= request.StartDate.Year)
                .Where(_ => _.PaidDate!.Value.Year <= request.EndDate.Year)
                .GroupBy(_ => _.PaidDate!.Value.Month)
                .Select(_ =>
                    new Model(_.Key, _.Count()))
                .ToList();

            return new(data.Sum(_ => _.TotalCustomers), data);
        }
    }
}