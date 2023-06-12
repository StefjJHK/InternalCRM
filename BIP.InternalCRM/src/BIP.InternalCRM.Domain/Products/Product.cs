using System.Text.RegularExpressions;
using BIP.InternalCRM.Domain.DomainErrors;
using BIP.InternalCRM.Domain.Products.DomainEvents;
using BIP.InternalCRM.Domain.ValueObjects;
using BIP.InternalCRM.Primitives.DomainDriven;
using OneOf;
using OneOf.Types;

namespace BIP.InternalCRM.Domain.Products;

public sealed class Product : Entity<ProductId>
{
    private Product(
        ProductId id,
        string name)
        : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public Image? Icon { get; private set; }

    public IntelliLockProject? Project { get; private set; }

    public static OneOf<Product, ValueMustBeUnique<Product>> Create(ProductId id, string name, ICollection<string> otherProductsNames)
    {
        if (otherProductsNames.Contains(name, StringComparer.OrdinalIgnoreCase))
        {
            return new ValueMustBeUnique<Product>(_ => _.Name);
        }
        
        var @new = new Product(id, name);

        @new.Raise(new ProductCreatedDomainEvent(Guid.NewGuid(), @new.Id, @new));

        return @new;
    }

    public Success Remove()
    {
        var removedProduct = (Clone() as Product)!;
        
        RemoveIcon();
        RemoveIlProject();
        
        Raise(new ProductRemovedDomainEvent(
            Guid.NewGuid(),
            Id,
            removedProduct));

        return new Success();
    }

    public OneOf<Product, ValueMustBeUnique<Product>> ChangeName(string newName, IEnumerable<string> otherProductsNames)
    {
        if (otherProductsNames.Contains(newName, StringComparer.OrdinalIgnoreCase))
        {
            return new ValueMustBeUnique<Product>(_ => _.Name);
        }

        var oldName = (Name.Clone() as string)!;
        Name = newName;
        
        Raise(new ProductChangedNameDomainEvent(
            Guid.NewGuid(),
            Id,
            newName,
            oldName));

        return this;
    }

    public Product ChangeIcon(string mediaType, byte[] iconData)
    {
        var iconExtRegex = Regex.Match(mediaType, "^image/(\\w*)$", RegexOptions.IgnoreCase);
        var iconExtValues = iconExtRegex.Groups.Values.ToList();
        var iconFilename = $"{Name}_{Guid.NewGuid()}.{iconExtValues[1]}";

        var iconResult = Image.Create(iconFilename, iconData);
        Icon = iconResult;

        return this;
    }

    public Success RemoveIcon()
    {
        if (Icon is null)
        {
            return new Success();
        }

        Icon = null;

        return new Success();
    }

    public Product AddIntelliLockProject(string originalFilename, byte[] data)
    {
        var projFilename = $"{originalFilename}_{Guid.NewGuid()}.ilproj";

        var newProject = IntelliLockProject.Create(projFilename, originalFilename, data);
        if (Project == null)
        {
            Project = newProject;
            Raise(new ProductAddedIlProjectDomainEvent(
                Guid.NewGuid(),
                Id,
                this,
                newProject));
        }
        else
        {
            var oldIlProject = Project with { };
            Project = newProject;
            
            Raise(new ProductChangedIlProjectDomainEvent(
                Guid.NewGuid(),
                Id,
                newProject,
                oldIlProject));
        }

        return this;
    }

    public Success RemoveIlProject()
    {
        if (Project is null)
        {
            return new Success();
        }

        var removedProject = Project with { };
        Project = null;
        
        Raise(new ProductRemovedIlProjectDomainEvent(
            Guid.NewGuid(),
            Id,
            removedProject));

        return new Success();
    }

    public override object Clone()
    {
        var cloned = new Product(Id, Name);

        if (Icon != null)
        {
            cloned.Icon = Icon with { };
        }

        if (Project != null)
        {
            cloned.Project = Project with { };
        }

        return cloned;
    }
}