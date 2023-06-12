using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Payments.Commands;
using FluentValidation;

namespace BIP.InternalCRM.Application.Payments.Validators;

public class AddPaymentCommandValidator : AbstractValidator<AddPaymentCommand>
{
    public AddPaymentCommandValidator()
    {
        RuleFor(_ => _.InvoiceNumber).NotEmpty().WithMessage(AppValidationErrors.IsRequired);
        RuleFor(_ => _.Number).NotEmpty().WithMessage(AppValidationErrors.IsRequired);
        RuleFor(_ => _.Amount).GreaterThan(0).WithMessage(AppValidationErrors.MustBeGreaterThanZero);
    }
}