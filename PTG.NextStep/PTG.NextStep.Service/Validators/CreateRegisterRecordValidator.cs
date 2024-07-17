using PTG.NextStep.Domain.DTO;
using FluentValidation;


namespace PTG.NextStep.Service.Validators
{
    public class CreateRegisterRecordValidator : CustomAbstractValidator<CreateRegisterRecordDTO>
    {
        public CreateRegisterRecordValidator() {
            RuleFor(x => x.PostingNumber).NotEmpty().WithMessage("PostingNumber is required.");
        }
    }
}
