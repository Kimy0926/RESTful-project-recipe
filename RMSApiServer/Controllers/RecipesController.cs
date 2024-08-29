using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RMSApiServer.DTO;
using RMSApiServer.Models;
using System.Globalization;

namespace RMSApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RecipesController(AppDbContext context)
        {
            _context = context;
        }

		// GET: api/recipes
		[HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            var Recipes = await _context.Recipe
                .ToListAsync();

            return Ok(Recipes);
        }

		// GET: api/recipes/site
		[HttpGet("site")]
		public async Task<ActionResult<IEnumerable<string>>> GetSite()
		{
			var SiteIds = await _context.Equipment
				.Select(e => e.SiteId)
				.Distinct()
				.ToListAsync();

			return Ok(SiteIds);
		}

        // GET: api/recipes/equipment
        [HttpGet("equipment")]
        public async Task<ActionResult<IEnumerable<string>>> GetEquipment()
        {
            var equipment = await _context.Equipment
                .ToListAsync();

            return Ok(equipment);
        }

        // GET: api/recipes/map
        [HttpGet("map")]
        public async Task<ActionResult<IEnumerable<string>>> GetEquipmentRecipeMap()
        {
            var mapObj = await _context.EquipmentRecipeMap
                .ToListAsync();

            return Ok(mapObj);
        }

        // GET: api/recipes/map
        [HttpGet("mappings/{recipeId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetMappingsByRecipeId(string recipeId)
        {

            var mapObj = await _context.EquipmentRecipeMap
                .Where( r => r.RecipeId == recipeId)
                .ToListAsync();

            if (mapObj == null || mapObj.Count < 0)
            {
                return NotFound();
            }

            return Ok(mapObj);
        }

        // GET: api/recipes/recipeId
        [HttpGet("{recipeId}")]
        public async Task<ActionResult<RecipeWithParameterDTO>> GetRecipe(string recipeId)
        {
            var recipe = await _context.Recipe
                .Where( j => j.RecipeId == recipeId)
                .ToListAsync();

			if (recipe == null || recipe.Count > 1)
            {
                return NotFound();
            }

            var recipeParameters = await _context.RecipeParameter
				.Where(rp => rp.RecipeId == recipeId)
		        .ToListAsync();

			var equipmentRecipeMap = await _context.EquipmentRecipeMap
				.Where(rm => rm.RecipeId == recipeId)
				.ToListAsync();


            var recipeDTO = new Recipe
			{
				RecipeId = recipe[0].RecipeId,
				SiteId = recipe[0].SiteId,
				RecipeName = recipe[0].RecipeName,
				Description = recipe[0].Description,
				CreatedAt = recipe[0].CreatedAt,
				UpdatedAt = recipe[0].UpdatedAt,
				RecipeParameters = recipeParameters.Select(rp => new RecipeParameter
				{
                    RecipeId = rp.RecipeId,
                    SiteId = rp.SiteId,
					RecipeParamId = rp.RecipeParamId,
					RecipeParamName = rp.RecipeParamName,
					USL = rp.USL,
					LSL = rp.LSL,
					Target = rp.Target,
					CreatedAt = rp.CreatedAt,
					UpdatedAt = rp.UpdatedAt,
				}).ToList(),
				EquipmentRecipeMap = equipmentRecipeMap.Select(rm => new EquipmentRecipeMap
				{ 
					RecipeId = rm.RecipeId,
					EquipmentId = rm.EquipmentId,
					SiteId	= rm.SiteId,
					CreatedAt = rm.CreatedAt,
					UpdatedAt = rm.UpdatedAt,
				}).ToList()
			};


			return Ok(recipeDTO);
        }

		// POST: api/Recipes
		[HttpPost]
		public async Task<ActionResult<string>> PostRecipe(RecipeCreateDTO recipeDTO)
		{
			string uniqueId = GenerateUniqueId(recipeDTO.SiteId);

			var recipe = new Recipe
			{
				RecipeId = uniqueId, // Generate a new unique ID
				SiteId = recipeDTO.SiteId,
				RecipeName = recipeDTO.RecipeName,
				Description = recipeDTO.Description,
				CreatedAt = DateTime.UtcNow,
				UpdatedAt = DateTime.UtcNow
			};

			_context.Recipe.Add(recipe);
			await _context.SaveChangesAsync();

			return Ok(recipe.RecipeId);
        }

        [HttpPut("update/{recipeId}")]
        public async Task<IActionResult> UpdateRecipe(string recipeId, [FromBody] RecipeUpdateDTO recipeDto)
        {
            if (recipeId != recipeDto.RecipeID)
            {
                return BadRequest();
            }

            var existingRecipe = await _context.Recipe.FirstOrDefaultAsync(r => r.RecipeId == recipeId);

            if (existingRecipe == null)
            {
                return NotFound();
            }

            existingRecipe.RecipeName = recipeDto.RecipeName;
            existingRecipe.Description = recipeDto.Description;
            existingRecipe.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(recipeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var updatedRecipeDTO = await _context.Recipe.FirstOrDefaultAsync(r => r.RecipeId == recipeId);


            return Ok(updatedRecipeDTO);
        }

        [HttpPut("{recipeId}")]
        public async Task<IActionResult> UpdateRecipeParams(string recipeId, [FromBody] RecipeWithParameterDTO recipeDto)
        {
            if (recipeId != recipeDto.RecipeId)
            {
                return BadRequest();
            }

            var existingRecipe = await _context.Recipe.Include(r => r.RecipeParameters)
                                                      .FirstOrDefaultAsync(r => r.RecipeId == recipeId);

            if (existingRecipe == null)
            {
                return NotFound();
            }

			existingRecipe.RecipeName = recipeDto.RecipeName;
			existingRecipe.Description = recipeDto.Description;
			existingRecipe.CreatedAt = recipeDto.CreatedAt;
			existingRecipe.UpdatedAt = recipeDto.UpdatedAt;

			// Update RecipeParameters
			_context.RecipeParameter.RemoveRange(existingRecipe.RecipeParameters);
			existingRecipe.RecipeParameters = recipeDto.RecipeParameters.Select(p => new RecipeParameter
			{
				RecipeId = p.RecipeId,
				RecipeParamId = p.RecipeParamId,
				SiteId = p.SiteId,
				RecipeParamName = p.RecipeParamName,
				USL = p.USL,
				LSL = p.LSL,
				Target = p.Target,
				CreatedAt = p.CreatedAt,
				UpdatedAt = p.UpdatedAt,
				Recipe = existingRecipe
			}).ToList();

            try
            {
				await _context.SaveChangesAsync();
			}
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(recipeId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

			var updatedRecipe = await _context.Recipe.Include(r => r.RecipeParameters)
											  .FirstOrDefaultAsync(r => r.RecipeId == recipeId);

			var recipeDTO = new Recipe
			{
				RecipeId = updatedRecipe.RecipeId,
				SiteId = updatedRecipe.SiteId,
				RecipeName = updatedRecipe.RecipeName,
				Description = updatedRecipe.Description,
				CreatedAt = updatedRecipe.CreatedAt,
				UpdatedAt = updatedRecipe.UpdatedAt,
				RecipeParameters = updatedRecipe.RecipeParameters.Select(rp => new RecipeParameter
				{
					RecipeId = rp.RecipeId,
					SiteId = rp.SiteId,
					RecipeParamId = rp.RecipeParamId,
					RecipeParamName = rp.RecipeParamName,
					USL = rp.USL,
					LSL = rp.LSL,
					Target = rp.Target,
					CreatedAt = rp.CreatedAt,
					UpdatedAt = rp.UpdatedAt,
				}).ToList()
			};


			return Ok(recipeDTO);
		}

		// DELETE: api/Recipes/5
		[HttpDelete("{recipeId}")]
        public async Task<IActionResult> DeleteRecipe(string recipeId)
        {
			var recipe = await _context.Recipe.FirstOrDefaultAsync(r => r.RecipeId == recipeId);

            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipe.Remove(recipe);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecipeExists(string recipeId)
        {
            return _context.Recipe.Any(e => e.RecipeId == recipeId);
        }

		private string GenerateUniqueId(string siteId)
		{
			// Format the current date-time as yyyymmddss
			string dateTimeString = DateTime.UtcNow.ToString("yyyyMMddss", CultureInfo.InvariantCulture);

			// Concatenate the parts to form the unique ID
			return $"R{siteId}{dateTimeString}";
		}
	}
}
