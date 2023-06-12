using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Domain.PurchaseOrders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using Success = OneOf.Types.Success;

namespace BIP.InternalCRM.Application.PurchaseOrders.Commands;

public record RemovePurchaseOrderCommand(
    string Number
) : IRequest<OneOf<Success, NotFound>>
{
    public class Handler :
        IRequestHandler<RemovePurchaseOrderCommand, OneOf<Success, NotFound>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Success, NotFound>> Handle(RemovePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            var po = await _dbContext.PurchaseOrders
                .Where(_ => _.Number == request.Number)
                .FirstOrDefaultAsync(cancellationToken);

            if (po is null) return new NotFound<PurchaseOrder>();

            var removeResult = po.Remove();

            _dbContext.PurchaseOrders.Remove(po);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return removeResult;
        }
    }
}