using FluentValidation;
using PTG.NextStep.Domain;
using PTG.NextStep.Domain.DTO;
using PTG.NextStep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG.NextStep.Service.Validators
{
    public class MemberContactValidator : CustomAbstractValidator<MemberContactDTO>
    {
        public MemberContactValidator()
        {
            //RuleFor(x => x)
            //    .Custom((memberContact, context) =>
            //    {
            //        if ((memberContact.BeginMonth != 0 && (memberContact.BeginDay == 0 || memberContact.EndMonth == 0 || memberContact.EndDay == 0))
            //            || (memberContact.BeginDay != 0 && (memberContact.BeginMonth == 0 || memberContact.EndMonth == 0 || memberContact.EndDay == 0))
            //            || (memberContact.EndMonth != 0 && (memberContact.BeginDay == 0 || memberContact.BeginMonth == 0 || memberContact.EndDay == 0))
            //            || (memberContact.EndDay != 0 && (memberContact.BeginDay == 0 || memberContact.EndMonth == 0 || memberContact.BeginMonth == 0))
            //        )
            //            context.AddFailure(nameof(memberContact.BeginDay), "Incomplete information provided for Begin/End Day/Month fields.");

            //        if(memberContact.BeginMonth == 0)
            //        {
            //            if(!string.IsNullOrEmpty(memberContact.SCAddress1) 
            //                || !string.IsNullOrEmpty(memberContact.SCAddress2)
            //                || !string.IsNullOrEmpty(memberContact.SCCity)
            //                || !string.IsNullOrEmpty(memberContact.SCState)
            //                || (!string.IsNullOrEmpty(memberContact.SCZipcode) && memberContact.SCZipcode != "0")
            //                )
            //                context.AddFailure(nameof(memberContact.BeginDay), "Seasonal Address information is present, but no Begin or End information is present.  The Seasonal Address will NOT be used in the system.");
            //        }
            //    });

            //Converted that to warning as of now, we need to revisit this as per the requirement
            ComplexWarning((memberContact, context) =>
            {
                if ((memberContact.BeginMonth != 0 && (memberContact.BeginDay == 0 || memberContact.EndMonth == 0 || memberContact.EndDay == 0))
                || (memberContact.BeginDay != 0 && (memberContact.BeginMonth == 0 || memberContact.EndMonth == 0 || memberContact.EndDay == 0))
                || (memberContact.EndMonth != 0 && (memberContact.BeginDay == 0 || memberContact.BeginMonth == 0 || memberContact.EndDay == 0))
                || (memberContact.EndDay != 0 && (memberContact.BeginDay == 0 || memberContact.EndMonth == 0 || memberContact.BeginMonth == 0))
                )
                    return true;
                return false;
            }, nameof(MemberBasicDTO.FullName), "Incomplete information provided for Begin/End Day/Month fields.");

            ComplexWarning((memberContact, context) =>
            {
                if (memberContact.BeginMonth == 0)
                {
                    if (!string.IsNullOrEmpty(memberContact.SCAddress1)
                        || !string.IsNullOrEmpty(memberContact.SCAddress2)
                        || !string.IsNullOrEmpty(memberContact.SCCity)
                        || !string.IsNullOrEmpty(memberContact.SCState)
                        || (!string.IsNullOrEmpty(memberContact.SCZipcode) && memberContact.SCZipcode != "0")
                        )
                        return true;
                }
                return false;
            }, nameof(MemberBasicDTO.FullName), "Seasonal Address information is present, but no Begin or End information is present.  The Seasonal Address will NOT be used in the system.");

            RuleFor(x => x)
            .Must(x=> IsValidDay(x.BeginDay,x.BeginMonth))
            .When(x => x.BeginDay != 0 && x.BeginMonth !=0)
            .WithMessage("Invalid Begin Day.");
            RuleFor(x => x)
            .Must(x => IsValidDay(x.EndDay, x.EndMonth))
            .When(x => x.EndDay != 0 && x.EndMonth != 0)
            .WithMessage("Invalid End Day.");

            RuleFor(x => x.SCAddress1)
                .NotEmpty()
                .When(x => x.BeginDay != 0)
                .WithMessage("Seasonal Address 1 must be provided.");

            RuleFor(x => x.SCCity)
                .NotEmpty()
                .When(x => x.BeginDay != 0)
                .WithMessage("Seasonal City must be provided.");

            RuleFor(x => x.SCState)
                .NotEmpty()
                .When(x => x.BeginDay != 0)
                .WithMessage("Seasonal State must be provided.");

            RuleFor(x => x.SCZipcode)
                .NotEmpty()
                .NotEqual("0")
                .When(x => x.BeginDay != 0)
                .WithMessage("Seasonal Zip Code must be provided.");

        }
        private bool IsValidDay(decimal day, decimal month)
        {
            return DateTime.TryParse($"{2000}-{month}-{day}", out _);
        }
    }
}
