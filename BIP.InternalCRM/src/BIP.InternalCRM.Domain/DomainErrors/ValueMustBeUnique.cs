using System.Linq.Expressions;

namespace BIP.InternalCRM.Domain.DomainErrors;

public record ValueMustBeUnique<TEntity> : DomainError
{
    public ValueMustBeUnique(Expression<Func<TEntity, object>> propertyAccessor)
        : base(
            $"{typeof(TEntity).Name}.{0}",
            $"The value of {typeof(TEntity).Name}.{0} must be unique")
    {
        if (propertyAccessor.Body is not MemberExpression memberExpression)
            throw new InvalidOperationException(
                $"The {nameof(propertyAccessor)} must be of type {nameof(MemberExpression)}");

        var memberName = memberExpression.Member.Name;
        Code = string.Format(Code, memberName);
        Message = string.Format(Message, memberName);
    }
}
