using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Products.Commands;

public record AddProductCommand(
    string Name,
    string? MediaType = null,
    byte[]? IconData = null,
    string? OriginalFilename = null,
    byte[]? IlProjectData = null
) : IRequest<OneOf<Product, DomainError>>
{
    public class Handler :
        IRequestHandler<AddProductCommand, OneOf<Product, DomainError>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Product, DomainError>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var otherProductsNames = await _dbContext.Products
                .AsNoTracking()
                .Select(_ => _.Name)
                .ToListAsync(cancellationToken);

            var result = Product
                .Create(new ProductId(Guid.NewGuid()), request.Name, otherProductsNames)
                .InvokeT0If(
                    request is { IconData: not null } && request.MediaType.IsNotEmpty(),
                    _ => _.ChangeIcon(request.MediaType!, request.IconData!))
                .InvokeT0If(
                    request is { IlProjectData: not null } && request.OriginalFilename.IsNotEmpty(),
                    _ => _.AddIntelliLockProject(request.OriginalFilename!, request.IlProjectData!));

            if (result.Value is DomainError domainError) return domainError;
            
            await _dbContext.Products.AddAsync(result.AsT0, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result.AsT0;
        }
    }
}