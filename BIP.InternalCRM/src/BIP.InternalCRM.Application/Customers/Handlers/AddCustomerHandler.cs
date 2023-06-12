using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Customers.Commands;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Leads;
using BIP.InternalCRM.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Customers.Handlers;

public class AddCustomerHandler :
    IRequestHandler<AddCustomerCommand, OneOf<Customer, DomainError>>,
    IRequestHandler<AddCustomerFromLeadCommand, OneOf<Customer, NotFound<Lead>, DomainError>>
{
    private readonly IDomainDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;

    public AddCustomerHandler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<OneOf<Customer, DomainError>> Handle(
        AddCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var otherCustomersNames = await _dbContext.Customers
            .AsNoTracking()
            .Select(_ => _.Name)
            .ToListAsync(cancellationToken);

        var contactInfo = ContactInfo.Create(request.Fullname, request.PhoneNumber, request.Email);

        var result = Customer.Create(
            new CustomerId(Guid.NewGuid()),
            request.Name,
            otherCustomersNames,
            contactInfo);

        if (result.Value is DomainError domainError) return domainError;

        await _dbContext.Customers.AddAsync(result.AsT0, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result.AsT0;
    }

    public async Task<OneOf<Customer, NotFound<Lead>, DomainError>> Handle(
        AddCustomerFromLeadCommand request,
        CancellationToken cancellationToken)
    {
        var lead = await _dbContext.Leads
            .AsNoTracking()
            .UseIf(request.LeadName.IsNotEmpty(), q => q.Where(_ => _.Name == request.LeadName))
            .FirstOrDefaultAsync(cancellationToken);

        if (lead is null) return new NotFound<Lead>();

        var otherCustomersNames = await _dbContext.Customers
            .Select(_ => _.Name)
            .ToListAsync(cancellationToken);

        var result = Customer.Create(
            new CustomerId(Guid.NewGuid()),
            lead.Name,
            otherCustomersNames,
            lead.ContactInfo);

        if (result.Value is DomainError domainError) return domainError;

        await _dbContext.Customers.AddAsync(result.AsT0, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result.AsT0;
    }
}