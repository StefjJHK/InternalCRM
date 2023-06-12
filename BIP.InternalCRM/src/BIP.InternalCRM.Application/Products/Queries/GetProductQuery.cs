using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace BIP.InternalCRM.Application.Products.Queries;

public record GetProductQuery(
    string Name
) : IRequest<OneOf<Product, NotFound>>
{
    public class Handler :
        IRequestHandler<GetProductQuery, OneOf<Product, NotFound>>
    {
        private readonly IDomainDbContext _dbContext;

        public Handler(IDomainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OneOf<Product, NotFound>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .Where(_ => _.Name == request.Name)
                .FirstOrDefaultAsync(cancellationToken);
            
            return product is not null 
                ? product
                : new NotFound<Product>();
        }
    }
}