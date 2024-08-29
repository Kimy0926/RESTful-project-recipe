using RMSApiServer.Models;
using System.ComponentModel.DataAnnotations;

namespace RMSApiServer.DTO
{
    public class ParameterDTO
	{
		[Required]
		public string RecipeId { get; set; }

		[Required]
		public string RecipeParamId { get; set; }

		[Required]
		public string SiteId { get; set; }

		public string RecipeParamName { get; set; }

		public float USL { get; set; }

		public float LSL { get; set; }

		public float Target { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

	}
}
