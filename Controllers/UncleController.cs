using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SantaClausCrm.DataAccess;
using SantaClausCrm.Dtos;
using SantaClausCrm.Models;

namespace SantaClausCrm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UncleController : ControllerBase
    {
        private readonly ILogger<UncleController> _logger;
        private readonly IDbContextFactory<ChristmasDbContext> _dbFactory;

        public UncleController(
            ILogger<UncleController> logger,
            IDbContextFactory<ChristmasDbContext> dbFactory) {
            _logger = logger;
            _dbFactory = dbFactory;
        }
        [HttpGet]
        public async Task<List<UncleChristmas>> GetUncles() {
            using var db = _dbFactory.CreateDbContext();
            await db.UnclesChristmas.ForEachAsync(u => u.CarriedGifts = db.GiftOperations.Count(g => g.UncleChristmasId == u.Id));
            return await db.UnclesChristmas.ToListAsync();               
        }
        [HttpPost]
        public async Task Add(UncleChristmasDto dto) {
            if(!dto.IsValid()) {
                HttpContext.Response.StatusCode = 422;
                return;
            }
            using var db = _dbFactory.CreateDbContext();
            var model = new UncleChristmas 
            {
                Name = dto.Name,
            };
            db.Add(model);
            await db.SaveChangesAsync();
        } 
    }
}