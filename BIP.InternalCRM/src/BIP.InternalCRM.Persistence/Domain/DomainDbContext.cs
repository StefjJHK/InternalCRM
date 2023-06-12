using BIP.InternalCRM.Application;
using BIP.InternalCRM.Application.Contexts;
using BIP.InternalCRM.Application.Customers;
using BIP.InternalCRM.Application.FileStorage;
using BIP.InternalCRM.Domain.Customers;
using BIP.InternalCRM.Domain.Invoices;
using BIP.InternalCRM.Domain.Leads;
using BIP.InternalCRM.Domain.Payments;
using BIP.InternalCRM.Domain.Products;
using BIP.InternalCRM.Domain.PurchaseOrders;
using BIP.InternalCRM.Domain.Subscriptions;
using BIP.InternalCRM.Domain.ValueObjects;
using BIP.InternalCRM.Persistence.Attributes;
using BIP.InternalCRM.Primitives.DomainDriven;
using BIP.InternalCRM.Shopify;
using BIP.InternalCRM.Shopify.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BIP.InternalCRM.Persistence.Domain;

public sealed class DomainDbContext :
    DbContextBase, IDomainDbContext, IRelationsDbContext, IShopifyDbContext, IUnitOfWork
{
    #region Domain

    public DbSet<Customer> Customers { get; init; } = null!;

    public DbSet<Invoice> Invoices { get; init; } = null!;

    public DbSet<Lead> Leads { get; init; } = null!;

    public DbSet<Payment> Payments { get; init; } = null!;

    public DbSet<Product> Products { get; init; } = null!;

    public DbSet<PurchaseOrder> PurchaseOrders { get; init; } = null!;

    public DbSet<Subscription> Subscriptions { get; init; } = null!;

    #endregion

    #region Relations

    public DbSet<CustomerRelations> CustomersRelations { get; init; } = null!;

    #endregion

    #region DomainEvents

    private DbSet<DomainEventDbEntity> DomainEvents { get; init; } = null!;

    #endregion

    #region Shopify

    public DbSet<ShopifyProductsRelations> ProductsRelations { get; init; } = null!;

    #endregion

    private readonly IFileStorageService _fileStorageService;
    private readonly IMediator _mediator;

    public DomainDbContext() : base(DbConstants.DomainDbScheme)
    {
        _mediator = null!;
        _fileStorageService = null!;
    }

    public DomainDbContext(DbContextOptions<DomainDbContext> options, IFileStorageService fileStorageService,
        IMediator mediator)
        : base(DbConstants.DomainDbScheme, options)
    {
        _fileStorageService = fileStorageService;
        _mediator = mediator;
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);

        builder
            .Properties<decimal>()
            .HaveColumnType("money")
            .HavePrecision(38, 8);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<DomainEventDbEntity>()
            .ToTable("DomainEvents", "events")
            .HasKey(_ => _.EventId);

        builder.ApplyConfigurationsFromAssembly(
            PersistenceProject.AssemblyRef,
            typeof(IShopifyDbContext),
            typeof(IRelationsDbContext));
        
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await SaveIlProjectsAsync(cancellationToken);
        await SaveIlLicensesAsync(cancellationToken);
        await SaveImagesAsync(cancellationToken);

        await PublishDomainEventsAsync(cancellationToken);

        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(_ => _.Entity)
            .SelectMany(_ => _.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }

    private async Task SaveImagesAsync(CancellationToken cancellationToken = default)
    {
        var images = ChangeTracker
            .Entries<Image>()
            .Where(_ => _.State == EntityState.Added)
            .Select(_ => _.Entity)
            .ToList();

        foreach (var image in images)
        {
            await _fileStorageService.SaveAsync(
                string.Format(FileSystemPaths.ImagesPath, image.Filename),
                image.Data,
                cancellationToken);
        }
    }

    private async Task SaveIlProjectsAsync(CancellationToken cancellationToken = default)
    {
        var ilProjects = ChangeTracker
            .Entries<IntelliLockProject>()
            .Where(_ => _.State == EntityState.Added)
            .Select(_ => _.Entity)
            .ToList();

        foreach (var ilProject in ilProjects)
        {
            await _fileStorageService.SaveAsync(
                string.Format(FileSystemPaths.IntelliLockProjectsPath, ilProject.Filename),
                ilProject.Data,
                cancellationToken);
        }
    }

    private async Task SaveIlLicensesAsync(CancellationToken cancellationToken = default)
    {
        var ilLicenses = ChangeTracker
            .Entries<IntelliLockLicense>()
            .Where(_ => _.State == EntityState.Added)
            .Select(_ => _.Entity)
            .ToList();

        foreach (var ilLicense in ilLicenses)
        {
            await _fileStorageService.SaveAsync(
                string.Format(FileSystemPaths.IntelliLockLicensesPath, ilLicense.Filename),
                ilLicense.Data,
                cancellationToken);
        }
    }
}