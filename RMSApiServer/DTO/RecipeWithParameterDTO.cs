using RMSApiServer.Models;
using System.ComponentModel.DataAnnotations;

namespace RMSApiServer.DTO
{
    public class RecipeWithParameterDTO
	{
		[Required]
		public string RecipeId { get; set; }

		[Required]
		public string SiteId { get; set; }

		public string RecipeName { get; set; }

		public string Description { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public List<ParameterDTO> RecipeParameters { get; set; }

	}
}
