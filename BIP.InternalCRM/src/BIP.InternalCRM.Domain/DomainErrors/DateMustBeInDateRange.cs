using System.Linq.Expressions;

namespace BIP.InternalCRM.Domain.DomainErrors;

public record DateMustBeInDateRange<TEntity> : DomainError
{
    public DateMustBeInDateRange(
        Expression<Func<TEntity, DateTime>> fromPropertyAccessor,
        Expression<Func<TEntity, DateTime?>> valuePropertyAccessor,
        Expression<Func<TEntity, DateTime>> toPropertyAccessor)
        : base(
            $"{typeof(TEntity).Name}.{0}",
            $"The datetime of {typeof(TEntity).Name}.{0} mut be in date range from {1} to {2}"
        )
    {
        if (fromPropertyAccessor.Body is not MemberExpression fromMemberExpression)
            throw new InvalidOperationException(
                $"The {nameof(fromPropertyAccessor)} must be of type {nameof(MemberExpression)}");

        if (valuePropertyAccessor.Body is not MemberExpression valueMemberExpression)
            throw new InvalidOperationException(
                $"The {nameof(valuePropertyAccessor)} must be of type {nameof(MemberExpression)}");

        if (toPropertyAccessor.Body is not MemberExpression toMemberExpression)
            throw new InvalidOperationException(
                $"The {nameof(toPropertyAccessor)} must be of type {nameof(MemberExpression)}");

        Code = string.Format(Code, valueMemberExpression.Member.Name);
        Message = string.Format(
            Message,
            valueMemberExpression.Member.Name,
            fromMemberExpression.Member.Name,
            toMemberExpression.Member.Name);
    }
}