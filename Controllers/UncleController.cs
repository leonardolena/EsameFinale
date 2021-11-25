using System;
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
    [Route("[controller]/[action]")]
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
            return await db.UnclesChristmas
                .Select(u => new UncleChristmas 
                    {
                    Id = u.Id,
                    Name = u.Name,
                    CarriedGifts = db.GiftOperations.Count(g => g.UncleChristmasId == u.Id)
                    }
                ).ToListAsync();               
        }
        [HttpPost]
        public async Task<ResultDto> Add(UncleChristmasDto dto) {
            if(!dto.IsValid()) {
                _logger.LogInformation(new ArgumentException("null_argument"),"Dto inserted is invalid",null);
                HttpContext.Response.StatusCode = 422;
                return null;
            }
            using var db = _dbFactory.CreateDbContext();
            var model = new UncleChristmas 
            {
                Name = dto.Name,
            };
            db.Add(model);
            await db.SaveChangesAsync();
            return new ResultDto { NewId = model.Id };
        } 
    }
}