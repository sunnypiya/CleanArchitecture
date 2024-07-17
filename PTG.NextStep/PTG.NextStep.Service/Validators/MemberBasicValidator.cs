using FluentValidation;
using PTG.NextStep.Domain;
using PTG.NextStep.Domain.DTO;
using System.Globalization;

namespace PTG.NextStep.Service.Validators
{
    public class MemberBasicValidator:CustomAbstractValidator<MemberBasicDTO>
    {        
        public MemberBasicValidator() {            

            RuleFor(x => x.Link).NotEmpty().GreaterThan(0).WithMessage("Link is required.");

            RuleFor(x => x.EmployeeNumber).NotEmpty().WithMessage("Employee Number is required")
                .Matches("^(0|[1-9]\\d*)(\\.\\d+)?$").WithMessage("Employee Number must be numeric.");

            ComplexWarning((model, context) =>
            {
                return model.FirstName?.Trim().Length + model.MiddleName?.Trim().Length + model.LastName?.Trim().Length + model.Suffix.Trim().Length > 75;

            }, nameof(MemberBasicDTO.FullName), "First, middle and last name plus the suffix have more than 75 characters. They must be 75 or less. Shorten one or more of the name fields.");

            //RuleFor(x => new { x.FirstName, x.MiddleName, x.LastName, x.Suffix }).Must(x => ValidFullName(x.FirstName, x.MiddleName, x.LastName, x.Suffix))
            //    .WithMessage("First, middle and last name plus the suffix have more than 75 characters. They must be 75 or less. Shorten one or more of the name fields.");

            RuleFor(x => x.HireDate)
                .NotEmpty().WithMessage("Hire Date must be provided.")
                .LessThan(x => DateOnly.FromDateTime(DateTime.Now)).WithMessage("Hire date should not be in the future.");

            RuleFor(x => new { x.HireDate, x.MembershipDateCurrBoard }).Must(x => ValidHireDate(x.HireDate, x.MembershipDateCurrBoard))
                .WithMessage("Hire Date is different from the Current Board Membership Date.");

            RuleFor(x => new { x.HireDate, x.EnrolledAfter2012 }).Must(x => ValidHireDateEnroll(x.HireDate, x.EnrolledAfter2012, true))
                .WithMessage("Hire Date is on or after April 2, 2012.  Unless the member transferred from another Board, enrolled after 4/1/2012 flag should be set to Yes.");

            RuleFor(x => new { x.HireDate, x.EnrolledAfter2012 }).Must(x => ValidHireDateEnroll(x.HireDate, x.EnrolledAfter2012, false))
                .WithMessage("Hire Date is before April 2, 2012, why is Enrolled after 4/1/2012 flag set to Yes?");

            RuleFor(x => new { x.HireDate, x.BirthDate }).Must(x => ValidHireDateBirthDate(x.HireDate, x.BirthDate))
                .WithMessage("Member must be at least 16 years of age at hire. Check Birth Date and Hire Date fields.");

            RuleFor(x => new { x.MembershipDateCurrBoard, x.MembershipDateOrigBoard }).Must(x => ValidMembershipDateCurrBoard(x.MembershipDateCurrBoard, x.MembershipDateOrigBoard))
                .WithMessage("Current Board Membership Date is different from the Original Board Membership Date.");

            RuleFor(x => new { x.ContributionRate, x.MostRecentStatusEvent }).Must(x => ValidateOnRecentStatusEvent(x.ContributionRate, x.MostRecentStatusEvent))
                .WithMessage("If member is active, Contribution Rate must be provided.");

            RuleFor(x => new { x.TwoPercentContribution, x.MostRecentStatusEvent }).Must(x => ValidateOnRecentStatusEvent(x.TwoPercentContribution, x.MostRecentStatusEvent))
                .WithMessage("If member is active, Two Percent Contribution must be provided.");

            RuleFor(x => new { x.ContributionRate, x.EnrolledAfter2012, x.MemberGroup, x.CreditableServiceToDate }).Must(x => ValidContributionRate(x.ContributionRate, x.EnrolledAfter2012, x.MemberGroup, x.CreditableServiceToDate))
                .WithMessage("Contribution rate of 6% is only valid for Group 1 member who became a member after April 1, 2012 and who has earned 30 years of creditable service!");

            RuleFor(x => x)
                .Custom((memberBasic, context) =>
                {
                    ContributionRateCalculator contributionRateCalculator = new ContributionRateCalculator();
                    int calculatedRate = contributionRateCalculator.CalculateContributionRate(memberBasic.MembershipDateOrigBoard);
                    int.TryParse(memberBasic.ContributionRate, out var actualContributionRate);
                    if ((memberBasic.MostRecentStatusEvent == "HIRED" || memberBasic.MostRecentStatusEvent == "REHIR") && calculatedRate != 0 && calculatedRate != actualContributionRate)
                        context.AddFailure(nameof(memberBasic.ContributionRate), $"Member's contribution rate, based on their Current Board Membership Date, should be {calculatedRate}%.");                    

                });
                

        }
        private bool ValidFullName(string firstName, string middleName, string lastName, string suffix)
        {
            return firstName.Trim().Length + middleName.Trim().Length + lastName.Trim().Length + suffix.Trim().Length <= 75;
        }

        private bool ValidHireDate(DateOnly? hireDate, DateOnly? membershipDateCurrBoard)
        {
            if(!membershipDateCurrBoard.HasValue)
                return false;

            if (hireDate.HasValue && membershipDateCurrBoard.HasValue)
                return hireDate.Value == membershipDateCurrBoard.Value;

            return true;
        }
        private bool ValidHireDateEnroll(DateOnly? hireDate,string enrolledAfter,bool isAfter2012)
        {
            var dateToCompare = DateTime.Parse("04/02/2012");

            if(isAfter2012 && hireDate.HasValue && hireDate.Value >= DateOnly.FromDateTime(dateToCompare) && enrolledAfter != CommonConstants.Yes)
                return false;

            if (!isAfter2012 && hireDate.HasValue && hireDate.Value < DateOnly.FromDateTime(dateToCompare) && enrolledAfter == CommonConstants.Yes)
                return false;

            return true;
        }
        private bool ValidHireDateBirthDate(DateOnly? hireDate, DateOnly? birthDate)
        {
            if (hireDate.HasValue && birthDate.HasValue)
            {
                var years = hireDate.Value.Year - birthDate.Value.Year;
                if (hireDate.Value.Month < birthDate.Value.Month || (birthDate.Value.Month == hireDate.Value.Month && birthDate.Value.Day < hireDate.Value.Day))
                {
                    years--;
                }
                if (years < 16)
                    return false;
            }
            return true;
        }
        private bool ValidMembershipDateCurrBoard(DateOnly? membershipDateCurrBoard, DateOnly? membershipDateOrigBoard)
        {
            if (!membershipDateCurrBoard.HasValue)
                return false;

            if (membershipDateCurrBoard.HasValue && membershipDateOrigBoard.HasValue)
                return membershipDateCurrBoard.Value == membershipDateOrigBoard.Value;

            return true;
        }
        private bool ValidateOnRecentStatusEvent(string propertyValue, string recentStatus)
        {
            if ((recentStatus == "HIRED" || recentStatus == "REHIR") && string.IsNullOrEmpty(propertyValue))
                return false;
            return true;
        }

        private bool ValidContributionRate(string contributionRate, string enrolledAfter2012, string memberGroup, decimal creditableServiceToDate)
        {
            if(contributionRate == "6" )
            {
                if (enrolledAfter2012 == CommonConstants.Yes && memberGroup == "1" && creditableServiceToDate >= 30)
                    return true;
                else
                    return false;
            }
            return true;
        }
        
    }
}
