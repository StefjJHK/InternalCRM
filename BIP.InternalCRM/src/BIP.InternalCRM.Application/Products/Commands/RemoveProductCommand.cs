using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;

namespace BIP.InternalCRM.Application.Products.Commands;

public record RemoveProductCommand(
    string Name
) : IRequest<OneOf<Success, NotFound<Product>>>
{
    public class Handler :
        IRequestHandler<RemoveProductCommand, OneOf<Success, NotFound<Product>>>
    {
        private readonly IDomainDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IDomainDbContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<OneOf<Success, NotFound<Product>>> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .Where(_ => _.Name == request.Name)
                .FirstOrDefaultAsync(cancellationToken);

            if (product is null) return new NotFound<Product>();

            var removeResult = product.Remove();

            _dbContext.Products.Remove(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return removeResult;
        }
    }
}