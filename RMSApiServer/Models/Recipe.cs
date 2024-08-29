using System.ComponentModel.DataAnnotations;

namespace RMSApiServer.Models
{
	public class Recipe
	{

		public string RecipeId { get; set; }

		public string SiteId { get; set; }

		public string RecipeName { get; set; }

		public string Description { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }


		public virtual ICollection<RecipeParameter> RecipeParameters { get; set; } // Navigation property

		public virtual ICollection<EquipmentRecipeMap> EquipmentRecipeMap { get; set; } // Navigation property

	}
}
