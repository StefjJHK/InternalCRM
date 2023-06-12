namespace BIP.InternalCRM.WebIdentity.Permissions;

public class PermissionSet
{
    private PermissionSet(
        AnalyticsPermissionSet analytics,
        ProductPermissionSet product,
        CustomerPermissionSet customer,
        LeadPermissionSet lead,
        PurchaseOrderPermissionSet purchaseOrder,
        InvoicePermissionSet invoice,
        PaymentPermissionSet payment,
        SubscriptionPermissionSet subscription,
        AdminPermissionSet admin,
        UserPermissionSet user,
        RolePermissionSet role
    )
    {
        Analytics = analytics;
        Product = product;
        Customer = customer;
        Payment = payment;
        Subscription = subscription;
        Lead = lead;
        PurchaseOrder = purchaseOrder;
        Invoice = invoice;
    }

#pragma warning disable
    // constructor for ef core
    private PermissionSet()
    {
    }
#pragma warning restore

    public AnalyticsPermissionSet Analytics { get; init; }

    public ProductPermissionSet Product { get; init; }

    public CustomerPermissionSet Customer { get; init; }

    public LeadPermissionSet Lead { get; init; }

    public PurchaseOrderPermissionSet PurchaseOrder { get; init; }

    public InvoicePermissionSet Invoice { get; init; }

    public PaymentPermissionSet Payment { get; init; }

    public SubscriptionPermissionSet Subscription { get; init; }

    public static readonly PermissionSet Empty = new(
        AnalyticsPermissionSet.Empty,
        ProductPermissionSet.Empty,
        CustomerPermissionSet.Empty,
        LeadPermissionSet.Empty,
        PurchaseOrderPermissionSet.Empty,
        InvoicePermissionSet.Empty,
        PaymentPermissionSet.Empty,
        SubscriptionPermissionSet.Empty,
        AdminPermissionSet.Empty,
        UserPermissionSet.Empty,
        RolePermissionSet.Empty
    );

    public static PermissionSet Create(
        AnalyticsPermissionSet analytics,
        ProductPermissionSet product,
        CustomerPermissionSet customer,
        LeadPermissionSet lead,
        PurchaseOrderPermissionSet purchaseOrder,
        InvoicePermissionSet invoice,
        PaymentPermissionSet payment,
        SubscriptionPermissionSet subscription,
        AdminPermissionSet admin,
        UserPermissionSet user,
        RolePermissionSet role
    )
    {
        var @new = new PermissionSet(
            analytics,
            product,
            customer,
            lead,
            purchaseOrder,
            invoice,
            payment,
            subscription,
            admin,
            user,
            role);

        return @new;
    }
}

public interface IPermissions
{
}