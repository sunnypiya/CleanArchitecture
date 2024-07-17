using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTG.NextStep.Service
{
    public class ContributionRateCalculator
    {
        public int CalculateContributionRate(DateOnly? membershipDateOrigBoard)
        {
            int calculatedContributionRate = 0;
            if (membershipDateOrigBoard.HasValue)
            {
                if (membershipDateOrigBoard.Value >= DateOnly.FromDateTime(DateTime.Parse("01/01/1800")) && membershipDateOrigBoard.Value <= DateOnly.FromDateTime(DateTime.Parse("01/01/1975")))
                    calculatedContributionRate = 5;
                if (membershipDateOrigBoard.Value >= DateOnly.FromDateTime(DateTime.Parse("01/01/1975")) && membershipDateOrigBoard.Value <= DateOnly.FromDateTime(DateTime.Parse("12/31/1983")))
                    calculatedContributionRate = 7;
                if (membershipDateOrigBoard.Value >= DateOnly.FromDateTime(DateTime.Parse("01/01/1984")) && membershipDateOrigBoard.Value <= DateOnly.FromDateTime(DateTime.Parse("06/30/1996")))
                    calculatedContributionRate = 8;
                if (membershipDateOrigBoard.Value >= DateOnly.FromDateTime(DateTime.Parse("07/01/1996")) && membershipDateOrigBoard.Value <= DateOnly.FromDateTime(DateTime.Parse("12/31/2399")))
                    calculatedContributionRate = 9;
            }
            return calculatedContributionRate;
        }
    }
}
