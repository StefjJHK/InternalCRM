using OneOf;

namespace BIP.InternalCRM.Application.Extensions;

public static class OneOfExtensions
{
    public static OneOf<T0, T1> InvokeIf<T0, T1>(
        this OneOf<T0, T1> source,
        bool condition,
        Func<OneOf<T0, T1>, OneOf<T0, T1>> function)
        => source.InvokeIf(_ => condition, function);

    public static OneOf<T0, T1> InvokeIf<T0, T1>(
        this OneOf<T0, T1> source,
        Func<OneOf<T0, T1>, bool> condition,
        Func<OneOf<T0, T1>, OneOf<T0, T1>> function)
        => condition(source)
            ? function(source)
            : source;
    
    public static OneOf<T0, T1> InvokeT0If<T0, T1>(
        this OneOf<T0, T1> source,
        bool condition,
        Func<T0, T0> mappingFunc)
        => source.InvokeT0If(_ => condition, mappingFunc);
    
    public static OneOf<T0, T1> InvokeT0If<T0, T1>(
        this OneOf<T0, T1> source,
        Func<T0, bool> condition,
        Func<T0, T0> mappingFunc)
    {
        if (!source.IsT0) return source;
        
        return condition(source.AsT0)
            ? mappingFunc(source.AsT0)
            : source.AsT0;
    }
}