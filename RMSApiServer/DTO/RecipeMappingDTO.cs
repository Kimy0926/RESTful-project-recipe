using RMSApiServer.Models;
using System.ComponentModel.DataAnnotations;

namespace RMSApiServer.DTO
{
    public class RecipeMappingDTO
	{


		[Required]
		public string RecipeId { get; set; }

		[Required]
		public string SiteId { get; set; }

        [Required]
        public string EquipmentId { get; set; }


	}
}
