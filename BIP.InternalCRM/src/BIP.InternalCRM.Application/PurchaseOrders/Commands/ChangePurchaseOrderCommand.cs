using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.PurchaseOrders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.PurchaseOrders.Commands;

public record ChangePurchaseOrderCommand(
    string NumberIdentity,
    string Number,
    decimal Amount,
    string CustomerName,
    string ProductName,
    DateTime ReceivedDate,
    DateTime DueDate
) : IRequest<OneOf<PurchaseOrder, NotFound, DomainError>>
{
    public class Handler :
        IRequestHandler<ChangePurchaseOrderCommand, OneOf<PurchaseOrder, NotFound, DomainError>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<PurchaseOrder, NotFound, DomainError>> Handle(
            ChangePurchaseOrderCommand request,
            CancellationToken cancellationToken)
        {
            var existingPo = await _dbContext.PurchaseOrders
                .Where(_ => _.Number == request.NumberIdentity)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingPo is null) return new NotFound<PurchaseOrder>();

            var otherPosNumbers = await _dbContext.PurchaseOrders
                .Where(_ => _.Id != existingPo.Id)
                .Select(_ => _.Number)
                .ToListAsync(cancellationToken);

            var changeResult = ((OneOf<PurchaseOrder, ValueMustBeUnique<PurchaseOrder>>)existingPo)
                .InvokeIf(
                    _ => _.IsT0 && _.AsT0.Number.NotEquals(request.Number),
                    _ => _.AsT0.ChangeNumber(request.Number, otherPosNumbers))
                .InvokeT0If(
                    _ => _.Amount.NotEquals(request.Amount),
                    _ => _.ChangeAmount(request.Amount))
                .InvokeT0If(
                    _ => _.ReceivedDate.NotEquals(request.ReceivedDate)
                         || _.DueDate.NotEquals(request.DueDate),
                    _ => _.ChangeExpirationDate(request.ReceivedDate, request.DueDate));

            if (changeResult.Value is DomainError domainError) return domainError;

            _dbContext.PurchaseOrders.Update(changeResult.AsT0);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return changeResult.AsT0;
        }
    }
}