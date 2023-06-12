using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace BIP.InternalCRM.Application.Customers.Commands;

public record RemoveCustomerCommand(
    string Name
) : IRequest<OneOf<Success, NotFound<Customer>>>
{
    public class Handler :
        IRequestHandler<RemoveCustomerCommand, OneOf<Success, NotFound<Customer>>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Success, NotFound<Customer>>> Handle(
            RemoveCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers
                .UseIf(request.Name.IsNotEmpty(), q => q.Where(_ => _.Name == request.Name))
                .FirstOrDefaultAsync(cancellationToken);

            if (customer is null) return new NotFound<Customer>();

            var removeResult = customer.Remove();

            _dbContext.Customers.Remove(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return removeResult;
        }
    }
}