using System.Linq.Expressions;
using BIP.InternalCRM.Primitives.DomainDriven;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BIP.InternalCRM.Persistence;

public static class StronglyTypedIdConverter
{
    public static ModelBuilder AddStronglyTypedIdConverters(this ModelBuilder builder)
    {
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (!property.ClrType.IsAssignableTo(typeof(IStronglyTypedId))) continue;

                var valueType = property.ClrType
                    .GetProperty(nameof(IStronglyTypedId.Value))!
                    .PropertyType;
                
                property.SetValueConverter(CreateStronglyTypedIdConverter(property.ClrType, valueType));
            }
        }

        return builder;
    }

    private static ValueConverter CreateStronglyTypedIdConverter(
        Type stronglyTypedIdType,
        Type valueType)
    {
        // id => id.Value
        var toProviderFuncType = typeof(Func<,>)
            .MakeGenericType(stronglyTypedIdType, valueType);
        var stronglyTypedIdParam = Expression.Parameter(stronglyTypedIdType, "id");
        var toProviderExpression = Expression.Lambda(
            toProviderFuncType,
            Expression.Property(stronglyTypedIdParam, "Value"),
            stronglyTypedIdParam);

        // value => new ProductId(value)
        var fromProviderFuncType = typeof(Func<,>)
            .MakeGenericType(valueType, stronglyTypedIdType);
        var valueParam = Expression.Parameter(valueType, "value");
        var ctor = stronglyTypedIdType.GetConstructor(new[] { valueType })!;
        var fromProviderExpression = Expression.Lambda(
            fromProviderFuncType,
            Expression.New(ctor, valueParam),
            valueParam);

        var converterType = typeof(ValueConverter<,>)
            .MakeGenericType(stronglyTypedIdType, valueType);

        return (ValueConverter)Activator.CreateInstance(
            converterType,
            toProviderExpression,
            fromProviderExpression,
            null)!;
    }
}