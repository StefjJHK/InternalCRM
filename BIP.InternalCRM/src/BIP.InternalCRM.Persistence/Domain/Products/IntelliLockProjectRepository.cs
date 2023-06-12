using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.FileStorage;
using BIP.InternalCRM.Application.Products;
using BIP.InternalCRM.Domain.Products;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Persistence.Domain.Products;

public class IntelliLockProjectRepository : IIntelliLockProjectRepository
{
    private readonly string _filePathPattern;
    private readonly IFileStorageService _fileStorageService;
    private readonly IDomainDbContext _domainDbContext;

    public IntelliLockProjectRepository(
        string filePathPattern,
        IFileStorageService fileStorageService,
        IDomainDbContext domainDbContext)
    {
        _filePathPattern = filePathPattern;
        _fileStorageService = fileStorageService;
        _domainDbContext = domainDbContext;
    }

    public async Task<OneOf<IntelliLockProject, NotFound>> GetByProductIdAsync(ProductId productId,
        CancellationToken cancellationToken = default)
    {
        var product = await _domainDbContext.Products
            .AsNoTracking()
            .Where(_ => _.Id == productId)
            .FirstOrDefaultAsync(cancellationToken);

        if (product?.Project is null) return new NotFound<IntelliLockProject>();

        var data = await _fileStorageService.GetAsync(
            string.Format(_filePathPattern, product.Project.Filename),
            cancellationToken);
        product.Project.LoadData(data);

        return product.Project;
    }
}