using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TechnicalProgrammingProject.Attributes
{
    public class DateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = (DateTime)value;
            DateTime startDate = Convert.ToDateTime("01/01/1900");
            DateTime endDate = Convert.ToDateTime("01/01/2100");

            if (date > endDate || date < startDate)
            {
                return new ValidationResult(string.Format("Date must be between {0} and {1}.", startDate, endDate));
            }

            return ValidationResult.Success;
        }
    }
}