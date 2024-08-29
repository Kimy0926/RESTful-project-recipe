using RMSApiServer.Models;
using System.ComponentModel.DataAnnotations;

namespace RMSApiServer.DTO
{
    public class RecipeUpdateDTO
	{
        [Required]
        public string RecipeID { get; set; }

		[Required]
		public string SiteId { get; set; }

		[Required]
		public string RecipeName { get; set; }

		public string Description { get; set; }



    }
}
