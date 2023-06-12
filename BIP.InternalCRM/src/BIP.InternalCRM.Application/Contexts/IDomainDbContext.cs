using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.Invoices;
using BIP.InternalCRM.Domain.Leads;
using BIP.InternalCRM.Domain.Payments;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Domain.PurchaseOrders;
using BIP.InternalCRM.Domain.Subscriptions;
using BIP.InternalCRM.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Application.Contexts;

public interface IDomainDbContext
{
    DbSet<Customer> Customers { get; }

    DbSet<Invoice> Invoices { get; }

    DbSet<Lead> Leads { get; }

    DbSet<Payment> Payments { get; }

    DbSet<Product> Products { get; }

    DbSet<PurchaseOrder> PurchaseOrders { get; }

    DbSet<Subscription> Subscriptions { get; }
}