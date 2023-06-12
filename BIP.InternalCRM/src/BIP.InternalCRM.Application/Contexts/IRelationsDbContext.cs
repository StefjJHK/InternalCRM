using BIP.InternalCRM.Application.Customers;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Contexts;

public interface IRelationsDbContext
{
    DbSet<CustomerRelations> CustomersRelations { get; }
}