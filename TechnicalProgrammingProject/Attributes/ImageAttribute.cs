using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;

namespace TechnicalProgrammingProject.Attributes
{
    //[AttributeUsage(AttributeTargets.Property)]
    public class ImageAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
          ValidationContext validationContext)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;

            // The file is required.
            if (file == null)
            {
                return ValidationResult.Success;
            }

            //max file size of 1MB
            if (file.ContentLength > 1 * 1024 * 1024)
            {
                return new ValidationResult("Please upload a file that is smaller than 1MB");
            }

            //Check file extensions for (".png", ".jpg", ".jpeg", and ".gif")
            string ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(ext) ||
               !ext.Equals(".png", StringComparison.OrdinalIgnoreCase) ||
               !ext.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
               !ext.Equals(".gif", StringComparison.OrdinalIgnoreCase) ||
               !ext.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return new ValidationResult("Please upload an image with a .png, .jpg, jpeg, or.gif extension.");
            }
            // Everything OK.
            return ValidationResult.Success;
        }
    }
}