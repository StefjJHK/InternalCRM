using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Leads;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Leads.Commands;

public record AddLeadCommand(
    string Name,
    string Fullname,
    string PhoneNumber,
    string Email,
    string ProductName,
    decimal Cost,
    DateTime StartDate,
    DateTime EndDate
) : IRequest<OneOf<Lead, NotFound<Product>, DomainError>>
{
    public class Handler :
        IRequestHandler<AddLeadCommand, OneOf<Lead, NotFound<Product>, DomainError>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Lead, NotFound<Product>, DomainError>> Handle(
            AddLeadCommand request,
            CancellationToken cancellationToken)
        {
            var existingProduct = await _dbContext.Products
                .AsNoTracking()
                .UseIf(request.ProductName.IsNotEmpty(), q => q.Where(_ => _.Name == request.ProductName))
                .FirstOrDefaultAsync(cancellationToken);

            if (existingProduct is null) return new NotFound<Product>();

            var otherLeadsNames = await _dbContext.Leads
                .AsNoTracking()
                .Select(_ => _.Name)
                .ToListAsync(cancellationToken);
            
            var contactInfo = ContactInfo.Create(request.Fullname, request.PhoneNumber, request.Email);
            
            var leadResult = Lead.Create(
                new LeadId(Guid.NewGuid()),
                request.Name,
                otherLeadsNames,
                contactInfo,
                existingProduct.Id,
                request.Cost,
                request.StartDate,
                request.EndDate);

            if (leadResult.Value is DomainError domainError) return domainError;

            await _dbContext.Leads.AddAsync(leadResult.AsT0, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return leadResult.AsT0;
        }
    }
}
