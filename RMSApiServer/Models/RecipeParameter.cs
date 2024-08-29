using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMSApiServer.Models
{
	public class RecipeParameter
	{
		public string RecipeId { get; set; }

		public string RecipeParamId { get; set; }

		public string SiteId { get; set; }

		public string RecipeParamName { get; set; }

		public float USL { get; set; }

		public float LSL { get; set; }

		public float Target { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }


		[ForeignKey("RecipeId, SiteId")]
		public virtual Recipe Recipe { get; set; }

	}
}
