using System.ComponentModel;
using System.Globalization;
using BIP.InternalCRM.Domain;
using BIP.InternalCRM.Primitives.DomainDriven;
using Newtonsoft.Json;

namespace BIP.InternalCRM.WebApi;

public class StronglyTypedIdJsonConverter<TStronglyTypedId> : JsonConverter<TStronglyTypedId>
    where TStronglyTypedId : IStronglyTypedId
{
    public override void WriteJson(JsonWriter writer, TStronglyTypedId? stronglyTypedId, JsonSerializer serializer)
    {
        serializer.Serialize(writer, stronglyTypedId != null ? stronglyTypedId.Value : null);
    }

    public override TStronglyTypedId ReadJson(JsonReader reader, Type objectType, TStronglyTypedId? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var value = serializer.Deserialize<Guid>(reader);
        return (TStronglyTypedId)Activator.CreateInstance(typeof(TStronglyTypedId), value)!;
    }
}

public class StronglyTypedIdTypeConverter<TStronglyTypedId> : TypeConverter
    where TStronglyTypedId : IStronglyTypedId
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(TStronglyTypedId) || base.CanConvertFrom(context, sourceType);
    }

    public override object ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return (TStronglyTypedId)Activator.CreateInstance(typeof(TStronglyTypedId), value)!;
    }
}

public static class StronglyTypedIdConverterExtensions
{
    public static void AddStronglyTypedIdJsonConverters(this IList<JsonConverter> converters)
    {
        var stronglyTypedIds = DomainProject.AssemblyRef
            .GetTypes()
            .Where(t => !t.IsInterface && t.IsAssignableTo(typeof(IStronglyTypedId)))
            .ToList();
        
        foreach (var stronglyTypedId in stronglyTypedIds)
        {
            converters.Add(CreateStronglyTypedIdJsonConverter(stronglyTypedId));
        }
    }

    private static JsonConverter CreateStronglyTypedIdJsonConverter(Type stronglyTypedIdType)
    {
        var converterType = typeof(StronglyTypedIdJsonConverter<>)
            .MakeGenericType(stronglyTypedIdType);
        
        var converter = (JsonConverter)Activator.CreateInstance(converterType)!;
        return converter;
    }

    public static IServiceCollection AddStronglyTypedIdTypeConverters(this IServiceCollection services)
    {
        var stronglyTypedIds = DomainProject.AssemblyRef
            .GetTypes()
            .Where(t => !t.IsInterface && t.IsAssignableTo(typeof(IStronglyTypedId)))
            .ToList();

        foreach (var stronglyTypedId in stronglyTypedIds)
        {
            var converterType = typeof(StronglyTypedIdTypeConverter<>)
                .MakeGenericType(stronglyTypedId);

            TypeDescriptor.AddAttributes(stronglyTypedId, new TypeConverterAttribute(converterType));
        }
        
        return services;
    }
}