using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Products;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Invoices;
using BIP.InternalCRM.Domain.Subscriptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Subscriptions.Commands;

public record AddSubscriptionCommand(
    string InvoiceNumber,
    string SubLegalEntity,
    decimal Cost,
    DateTime ValidFrom,
    DateTime ValidUntil
) : IRequest<OneOf<Subscription, NotFound, DomainError>>
{
    public class Handler :
        IRequestHandler<AddSubscriptionCommand, OneOf<Subscription, NotFound, DomainError>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntelliLockProjectRepository _ilProjectRepository;
        private readonly IMediator _mediator;

        public Handler(
            IDomainDbContext dbContext,
            IMediator mediator,
            IIntelliLockProjectRepository ilProjectRepository,
            IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _mediator = mediator;
            _ilProjectRepository = ilProjectRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Subscription, NotFound, DomainError>> Handle(AddSubscriptionCommand request,
            CancellationToken cancellationToken)
        {
            var invoice = await _dbContext.Invoices
                .Include(_ => _.Subscriptions)
                .Where(_ => _.Number == request.InvoiceNumber)
                .FirstOrDefaultAsync(cancellationToken);

            if (invoice is null) return new NotFound<Invoice>();

            var subNumber = await _mediator.Send(
                new CreateSubscriptionIdCommand(invoice.Subscriptions.Select(_ => _.Number)),
                cancellationToken);

            var result = invoice.AddSubscription(
                new SubscriptionId(Guid.NewGuid()),
                subNumber,
                request.SubLegalEntity,
                request.Cost,
                request.ValidFrom,
                request.ValidUntil);

            if (result.Value is DomainError domainError) return domainError;

            var projectResult = await _ilProjectRepository.GetByProductIdAsync(invoice.ProductId, cancellationToken);

            if (projectResult.IsT0)
            {
                var generateLicData = new GenerateIlLicenseDataCommand(
                    invoice.ProductId,
                    invoice.CustomerId,
                    DateTime.Now,
                    projectResult.AsT0);

                var generateLicDataResult = await _mediator.Send(generateLicData, cancellationToken);
                result = result.MapT0(
                    _ => _.AddIlLicense(
                        generateLicDataResult.Key,
                        generateLicDataResult.IlLicenseData));
            }

            await _dbContext.Subscriptions.AddAsync(result.AsT0, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result.AsT0;
        }
    }
}