using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.FileStorage;
using BIP.InternalCRM.Application.Subscriptions;
using BIP.InternalCRM.Domain.Subscriptions;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Persistence.Domain.Invoices;

public class IntelliLockLicenseRepository : IIntelliLockLicenseRepository
{
    private readonly string _filePathPattern;
    private readonly IFileStorageService _fileStorageService;
    private readonly IDomainDbContext _domainDbContext;

    public IntelliLockLicenseRepository(
        string filePathPattern,
        IFileStorageService fileStorageService,
        IDomainDbContext domainDbContext)
    {
        _filePathPattern = filePathPattern;
        _fileStorageService = fileStorageService;
        _domainDbContext = domainDbContext;
    }

    public async Task<OneOf<IntelliLockLicense, NotFound>> GetBySubscriptionIdAsync(SubscriptionId subscriptionId,
        CancellationToken cancellationToken = default)
    {
        var sub = await _domainDbContext.Subscriptions
            .Where(_ => _.Id == subscriptionId)
            .Include(_ => _.License)
            .SingleOrDefaultAsync(cancellationToken);

        if (sub?.License is null) return new NotFound<IntelliLockLicense>();

        var data = await _fileStorageService.GetAsync(
            string.Format(_filePathPattern, sub.License.Filename),
            cancellationToken);
        sub.License.LoadData(data);

        return sub.License;
    }
}