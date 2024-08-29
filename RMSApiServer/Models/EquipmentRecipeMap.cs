using System.ComponentModel.DataAnnotations.Schema;

namespace RMSApiServer.Models
{
	public class EquipmentRecipeMap
	{
		public string RecipeId { get; set; }

		public string EquipmentId { get; set; }

		public string SiteId { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }


		[ForeignKey("RecipeId, SiteId")]
		public virtual Recipe Recipe { get; set; }

		[ForeignKey("EquipmentId, SiteId")]
		public virtual Equipment Equipment { get; set; }
	}
}
