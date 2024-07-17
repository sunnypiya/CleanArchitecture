using FluentValidation;
using FluentValidation.Results;
using PTG.NextStep.Service.Extensions;
using System.ComponentModel.DataAnnotations;

namespace PTG.NextStep.Service.Validators
{
    public abstract class CustomAbstractValidator<T> : AbstractValidator<T>
    {
        protected void WarningWhen(Func<T, bool> predicate, string propertyName, string errorMessage)
        {
            RuleFor(x => x)
                .Custom((x, context) =>
                {
                    if (predicate(x))
                    {
                        var warning = new ValidationFailure(propertyName, errorMessage) { Severity = Severity.Warning };
                        context.AddFailure(warning);
                    }
                });
        }

        protected void ComplexWarning(Func<T, ValidationContext<T>, bool> validationLogic, string propertyName, string errorMessage)
        {
            RuleFor(x => x)
                .Custom((x, context) =>
                {
                    
                    if (validationLogic(x, context))
                    {
                        var warning = new ValidationFailure(propertyName, errorMessage) { Severity = Severity.Warning };
                        context.AddFailure(warning);
                    }
                });
        }
    }
}
