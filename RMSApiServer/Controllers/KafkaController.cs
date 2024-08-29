using Microsoft.AspNetCore.Mvc;
using RMSApiServer.Models;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

namespace RMSApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly KafkaService _kafkaService;

        public KafkaController(KafkaService kafkaService, AppDbContext context)
        {
            _kafkaService = kafkaService;
            _context = context;
        }

        [HttpPut("download/{recipeId}")]
        public async Task<IActionResult> SendMessage(string recipeId)
        {
            var recipeParameters = await _context.RecipeParameter
                .Where(rp => rp.RecipeId == recipeId)
                .ToListAsync();

            var recipeDTO = new Recipe
            {
                RecipeId = recipeParameters[0].RecipeId,
                SiteId = recipeParameters[0].SiteId,
                RecipeParameters = recipeParameters.Select(rp => new RecipeParameter
                {
                    RecipeParamId = rp.RecipeParamId,
                    RecipeParamName = rp.RecipeParamName,
                    USL = rp.USL,
                    LSL = rp.LSL,
                    Target = rp.Target,
                }).ToList(),
            };

            var message = System.Text.Json.JsonSerializer.Serialize(recipeDTO);
            await _kafkaService.ProduceMessageAsync(message);
            return Ok("Message sent");
        }
    }
}
