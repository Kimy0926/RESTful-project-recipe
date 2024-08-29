using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMSApiServer.Models
{
	public class Equipment
	{
		public string EquipmentId { get; set; }

		public string SiteId { get; set; }

		public string EquipmentName { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public virtual ICollection<EquipmentRecipeMap> EquipmentRecipeMap { get; set; } // Navigation property
	}
}
