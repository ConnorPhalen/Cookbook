using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TechnicalProgrammingProject.Attributes;

namespace TechnicalProgrammingProject.Models
{
    public class CreateRecipeViewModel
    {
        public CreateRecipeViewModel()
        {
            Ingredients = new HashSet<Ingredient>();
        }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        [DisplayName("Cook Time")]
        [Range(0, 2880)]
        public int CookTime { get; set; }
        [Required]
        [Range(0,100)]
        public int Servings { get; set; }

        [HttpPostedFileExtensions]
        [ValidateFileSize(ErrorMessage = "Image must be less than 1MB")]
        public HttpPostedFileBase Image { get; set; }
        [Required]
        public string Directions { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }

    public class UploadsViewModel
    {
        public UploadsViewModel()
        {
            Recipes = new HashSet<Recipe>();
        }
        public string UploaderName { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }

    public class UploadedRecipe
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime? DateUploaded { get; set; }
        public int Rating { get; set; }
        public string Status { get; set; }
        public bool isDelete { get; set; }
    }
    public class DeleteRecipeViewModel
    {
        public DeleteRecipeViewModel()
        {
            UploadedRecipes = new List<UploadedRecipe>();
        }
        public virtual List<UploadedRecipe> UploadedRecipes { get; set; }
    }
}