using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Domain.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Customers.Queries;

public record GetCustomerQuery(
    string Name
) : IRequest<OneOf<Customer, NotFound<Customer>>>
{
    public class Handler :
        IRequestHandler<GetCustomerQuery, OneOf<Customer, NotFound<Customer>>>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OneOf<Customer, NotFound<Customer>>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers
                .AsNoTracking()
                .Where(_ => _.Name == request.Name)
                .FirstOrDefaultAsync(cancellationToken);

            return customer is not null
                ? customer
                : new NotFound<Customer>();
        }
    }
}
