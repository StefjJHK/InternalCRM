using BIP.InternalCRM.Application.AppErrors;
using BIP.InternalCRM.Application.Extensions;
using BIP.InternalCRM.Application.Leads.Commands;
using FluentValidation;

namespace BIP.InternalCRM.Application.Leads.Validators;

public class AddLeadCommandValidator : AbstractValidator<AddLeadCommand>
{
    public AddLeadCommandValidator()
    {
        RuleFor(_ => _.Name).NotEmpty().WithMessage(AppValidationErrors.IsRequired);
        RuleFor(_ => _.Fullname).NotEmpty().WithMessage(AppValidationErrors.IsRequired);
        
        RuleFor(_ => _.PhoneNumber)
            .NotEmpty().When(_ => _.Email.IsEmpty())
            .WithMessage(AppValidationErrors.AtLeastOneIsRequired);
        RuleFor(_ => _.Email)
            .NotEmpty().When(_ => _.PhoneNumber.IsEmpty())
            .WithMessage(AppValidationErrors.AtLeastOneIsRequired);
        
        RuleFor(_ => _.Cost).GreaterThan(0).WithMessage(AppValidationErrors.MustBeGreaterThanZero);
        
        RuleFor(_ => _.StartDate).LessThanOrEqualTo(_ => _.EndDate)
            .WithMessage(AppValidationErrors.DateTimeMustBeConsistent);
        RuleFor(_ => _.EndDate).GreaterThanOrEqualTo(_ => _.StartDate)
            .WithMessage(AppValidationErrors.DateTimeMustBeConsistent);
    }
}