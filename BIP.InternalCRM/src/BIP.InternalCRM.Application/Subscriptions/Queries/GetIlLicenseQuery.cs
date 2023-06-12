using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Domain.Subscriptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Subscriptions.Queries;

public record GetIlLicenseQuery(Guid Key) :
    IRequest<OneOf<IntelliLockLicense, NotFound>>
{
    public class Handler :
        IRequestHandler<GetIlLicenseQuery, OneOf<IntelliLockLicense, NotFound>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IIntelliLockLicenseRepository _licenseRepository;

        public Handler(IDomainDbContext dbContext, IIntelliLockLicenseRepository licenseRepository)
        {
            _dbContext = dbContext;
            _licenseRepository = licenseRepository;
        }

        public async Task<OneOf<IntelliLockLicense, NotFound>> Handle(GetIlLicenseQuery request, CancellationToken cancellationToken)
        {
            var sub = await _dbContext.Invoices
                .Include(_ => _.Subscriptions)
                .SelectMany(_ => _.Subscriptions
                    .Where(sub => sub.License!.Key == request.Key))
                .FirstOrDefaultAsync(cancellationToken);

            if (sub is null) return new NotFound<IntelliLockLicense>();

            var lic = await _licenseRepository.GetBySubscriptionIdAsync(sub.Id, cancellationToken);

            return lic;
        }
    }
}
