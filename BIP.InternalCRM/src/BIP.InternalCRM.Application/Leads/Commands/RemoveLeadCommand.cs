using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Domain.Leads;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace BIP.InternalCRM.Application.Leads.Commands;

public record RemoveLeadCommand(
    string Name
) : IRequest<OneOf<Success, NotFound<Lead>>>
{
    public class Handler :
        IRequestHandler<RemoveLeadCommand, OneOf<Success, NotFound<Lead>>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Success, NotFound<Lead>>> Handle(RemoveLeadCommand request, CancellationToken cancellationToken)
        {
            var lead = await _dbContext.Leads
                .Where(_ => _.Name == request.Name)
                .FirstOrDefaultAsync(cancellationToken);

            if (lead is null) return new NotFound<Lead>();

            var removeResult = lead.Remove();

            _dbContext.Leads.Remove(lead);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return removeResult;
        }
    }
}