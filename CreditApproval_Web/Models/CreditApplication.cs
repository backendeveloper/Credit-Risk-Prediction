using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreditApproval_Web.Models
{
    public class CreditApplication
    {
        [Required]
        public string CheckingAccount { get; set; }
        
        [Required]
        public string DurationInMonths { get; set; }
        
        [Required]
        public string CreditHistory { get; set; }
        
        [Required]
        public string Purpose { get; set; }
        
        [Required]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "{0} sayı olmalıdır.")]
        public string CreditAmount { get; set; }
        
        [Required]
        public string SavingsAccountBonds { get; set; }
        
        [Required]
        public string PresentEmploymentSince { get; set; }
        
        [Required]
        [Range(1, 90)]
        public string InstallmentRate { get; set; }
        
        [Required]
        public string PersonalStatusAndSex { get; set; }
        
        [Required]
        public string OtherDebtorsGuarantors { get; set; }
        
        [Required]
        [Range(1, 90)]
        public string PresentResidenceSince { get; set; }
        
        [Required]
        public string Property { get; set; }
        
        [Required]
        [Range(18, 85)]
        public string AgeInYears { get; set; }
        
        [Required]
        public string OtherInstallmentPlans { get; set; }
        
        [Required]
        public string Housing { get; set; }
        
        [Required]
        [Range(1, 5)]
        public string NumberOfExistingCredits { get; set; }
        
        [Required]
        public string Job { get; set; }
        
        [Required]
        [Range(1, 10)]
        public string NumberOfPeopleBeingLiableFor { get; set; }
        
        [Required]
        public string Telephone { get; set; }

        [Required]
        public string ForeignWorker { get; set; }

        //Result will be populated after ML responds
        public string Result { get; set; }
    }
}