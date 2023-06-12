using AutoMapper;
using AutoMapper.Configuration;
using System.Linq.Expressions;
using System.Reflection;

namespace BIP.InternalCRM.Application.Extensions;

public static class AutoMapperExtensions
{
    public static void MapFromItems<TSource, TMember>(
        this ICtorParamConfigurationExpression<TSource> configExpr,
        string itemKey)
    {
        configExpr
            .MapFrom((_, ctx) =>
            {
                if (!ctx.TryGetItems(out var items))
                {
                    throw new InvalidOperationException("Resolution context doesn't have items.");
                }
                
                if (!items.TryGetValue(itemKey, out var value))
                {
                    throw new ArgumentException($"Item with {nameof(itemKey)}:{itemKey} doesn't have a value.", nameof(itemKey));
                }

                return (TMember)value;
            });
    }


    public static IMappingExpression<TSource, TDestination>
        ForRecordParameter<TSource, TDestination, TMember>(
            this IMappingExpression<TSource, TDestination> expr,
            Expression<Func<TDestination, TMember>> destParameter,
            Action<ICtorParamConfigurationExpression<TSource>> paramOptions)
    {
        var memberInfo = FindMember(destParameter);

        return expr
            .ForCtorParam(memberInfo.Name, paramOptions)
            .ForMember(destParameter, o => o.Ignore());
    }

    // https://github.com/AutoMapper/AutoMapper/blob/master/src/AutoMapper/Internal/ReflectionHelper.cs#L84
    private static MemberInfo FindMember(LambdaExpression lambdaExpression)
    {
        Expression expressionToCheck = lambdaExpression.Body;
        while (true)
        {
            switch (expressionToCheck)
            {
                case MemberExpression
                {
                    Member: var member, Expression.NodeType: ExpressionType.Parameter or ExpressionType.Convert
                }:
                    return member;
                case UnaryExpression { Operand: var operand }:
                    expressionToCheck = operand;
                    break;
                default:
                    throw new ArgumentException(
                        $"Expression '{lambdaExpression}' must resolve to top-level member and not any child object's properties. You can use ForPath, a custom resolver on the child type or the AfterMap option instead.",
                        nameof(lambdaExpression));
            }
        }
    }
}