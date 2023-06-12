using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Leads;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Leads.Commands;

public record ChangeLeadCommand(
    string NameIdentity,
    string Name,
    string Fullname,
    string PhoneNumber,
    string Email,
    decimal Cost,
    DateTime StartDate,
    DateTime EndDate
) : IRequest<OneOf<Lead, NotFound<Lead>, DomainError>>
{
    public class Handler :
        IRequestHandler<ChangeLeadCommand, OneOf<Lead, NotFound<Lead>, DomainError>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Lead, NotFound<Lead>, DomainError>> Handle(ChangeLeadCommand request, CancellationToken cancellationToken)
        {
            var existingLead = await _dbContext.Leads
                .Where(_ => _.Name == request.NameIdentity)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingLead is null) return new NotFound<Lead>();
            var otherLeadsNames = await _dbContext.Leads
                .AsNoTracking()
                .Where(_ => _.Id != existingLead.Id)
                .Select(_ => _.Name)
                .ToListAsync(cancellationToken);

            var changeResult = ((OneOf<Lead, ValueMustBeUnique<Lead>>)existingLead)
                .InvokeIf(
                    _ => _.IsT0 && _.AsT0.Name.NotEquals(request.Name),
                    _ => _.AsT0.ChangeName(request.Name, otherLeadsNames))
                .InvokeIf(
                    _ => _.IsT0 && _.AsT0.Cost.NotEquals(request.Cost),
                    _ => _.AsT0.ChangeCost(request.Cost))
                .InvokeIf(
                    _ => _.IsT0 && _.AsT0.StartDate.NotEquals(request.StartDate)
                         || _.AsT0.EndDate.NotEquals(request.EndDate),
                    _ => _.AsT0.ChangeExpirationDateRange(request.StartDate, request.EndDate))
                .InvokeIf(
                    _ => _.IsT0 && _.AsT0.ContactInfo.Fullname.NotEquals(request.Fullname)
                         || _.AsT0.ContactInfo.PhoneNumber.NotEquals(request.PhoneNumber)
                         || _.AsT0.ContactInfo.Email.NotEquals(request.Email),
                    _ => _.AsT0.ChangeContactInfo(request.Fullname, request.PhoneNumber, request.Email));

            if (changeResult.Value is DomainError domainError) return domainError;

            _dbContext.Leads.Update(changeResult.AsT0);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return changeResult.AsT0;
        }
    }
}