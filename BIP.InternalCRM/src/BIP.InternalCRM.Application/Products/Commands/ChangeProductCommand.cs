using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Products.Commands;

public record ChangeProductCommand(
    string NameIdentity,
    string Name,
    string? MediaType = null,
    byte[]? IconData = null,
    string? OriginalFilename = null,
    byte[]? IlProjectData = null
) : IRequest<OneOf<Product, NotFound<Product>, DomainError>>
{
    public class Handler :
        IRequestHandler<ChangeProductCommand, OneOf<Product, NotFound<Product>, DomainError>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<OneOf<Product, NotFound<Product>, DomainError>> Handle(
            ChangeProductCommand request,
            CancellationToken cancellationToken)
        {
            var existingProduct = await _dbContext.Products
                .Where(_ => _.Name == request.NameIdentity)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingProduct is null) return new NotFound<Product>();

            var otherProductNames = await _dbContext.Products
                .AsNoTracking()
                .Where(_ => _.Id != existingProduct.Id)
                .Select(_ => _.Name)
                .ToListAsync(cancellationToken);

            var changeResult = ((OneOf<Product, ValueMustBeUnique<Product>>)existingProduct)
                .InvokeIf(
                    _ => _.IsT0 && _.AsT0.Name.NotEquals(request.Name),
                    _ => _.AsT0.ChangeName(request.Name, otherProductNames))
                .InvokeT0If(
                    CanChangeIcon(existingProduct.Icon, request.MediaType, request.IconData),
                    _ => _.ChangeIcon(request.MediaType!, request.IconData!))
                .InvokeT0If(
                    CanChangeIlProduct(existingProduct.Project, request.OriginalFilename, request.IlProjectData),
                    _ => _.AddIntelliLockProject(request.OriginalFilename!, request.IlProjectData!));

            if (changeResult.Value is DomainError domainError) return domainError;

            _dbContext.Products.Update(changeResult.AsT0);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return changeResult.AsT0;
        }

        private static bool CanChangeIcon(
            Image? icon,
            string? newMediaType,
            IEnumerable<byte>? newIconData)
        {
            return newMediaType.IsNotEmpty()
                   && newIconData != null
                   && (icon == null
                       || (icon.MediaType.NotEquals(newMediaType)
                           || icon.Data.NotEquals(newIconData)));
        }

        private static bool CanChangeIlProduct(
            IntelliLockProject? project,
            string? newOriginalFilename,
            IEnumerable<byte>? newIlProjectData)
        {
            return newOriginalFilename.IsNotEmpty()
                   && newIlProjectData != null
                   && (project == null
                       || (project.OriginalFilename.NotEquals(newOriginalFilename)
                           || project.Data.NotEquals(newIlProjectData)));
        }
    }
}