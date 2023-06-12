using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.DomainErrors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Customers.Commands;

public record ChangeCustomerCommand(
    string NameIdentity,
    string Name,
    string Fullname,
    string PhoneNumber,
    string Email
) : IRequest<OneOf<Customer, NotFound<Customer>, DomainError>>
{
    public class Handler :
        IRequestHandler<ChangeCustomerCommand, OneOf<Customer, NotFound<Customer>, DomainError>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Customer, NotFound<Customer>, DomainError>> Handle(
            ChangeCustomerCommand request,
            CancellationToken cancellationToken)
        {
            var existingCustomer = await _dbContext.Customers
                .Where(_ => _.Name == request.NameIdentity)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingCustomer is null) return new NotFound<Customer>();

            var otherCustomersNames = await _dbContext.Customers
                .AsNoTracking()
                .Where(_ => _.Name != existingCustomer.Name)
                .Select(_ => _.Name)
                .ToListAsync(cancellationToken);

            var changeResult = ((OneOf<Customer, ValueMustBeUnique<Customer>>)existingCustomer)
                .InvokeIf(
                    _ => _.IsT0 && _.AsT0.Name.NotEquals(request.Name),
                    _ => _.AsT0.ChangeName(request.Name, otherCustomersNames))
                .InvokeIf(
                    _ => _.IsT0 && (_.AsT0.ContactInfo.Fullname.NotEquals(request.Fullname)
                         || _.AsT0.ContactInfo.PhoneNumber.NotEquals(request.PhoneNumber)
                         || _.AsT0.ContactInfo.Email.NotEquals(request.Email)),
                    _ => _.AsT0.ChangeContactInfo(request.Fullname, request.PhoneNumber, request.Email));

            if (changeResult.Value is DomainError domainError) return domainError;

            _dbContext.Customers.Update(changeResult.AsT0);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return changeResult.AsT0;
        }
    }
}