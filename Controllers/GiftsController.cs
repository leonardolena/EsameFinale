using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SantaClausCrm.DataAccess;
using SantaClausCrm.Dtos;
using SantaClausCrm.Models;

namespace SantaClausCrm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GiftsController : ControllerBase
    {
        private readonly ILogger<GiftsController> _logger;
        private readonly IDbContextFactory<ChristmasDbContext> _dbFactory;

        public GiftsController(
            ILogger<GiftsController> logger,
            IDbContextFactory<ChristmasDbContext> dbFactory) {
            _logger = logger;
            _dbFactory = dbFactory;
        }

        [HttpPost]
        public async Task<GiftAddResultDto> Add(GiftAddDto dto) {
            if (!dto.IsValid()) {
                HttpContext.Response.StatusCode = 422;
                return null;
            }
            using var db = _dbFactory.CreateDbContext();
            var model = new Gift {
                Product = dto.Product,
            };
            db.Add(model);
            await db.SaveChangesAsync();
            return new GiftAddResultDto { NewId =  model.Id };
        }
    }
}
