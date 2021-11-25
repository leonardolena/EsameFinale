using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SantaClausCrm.DataAccess;
using SantaClausCrm.Dtos;
using SantaClausCrm.Models;
using System;

namespace SantaClausCrm.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OperationsController : ControllerBase
    {
        private readonly ILogger<OperationsController> _logger;
        private readonly IDbContextFactory<ChristmasDbContext> _dbFactory;

        public OperationsController(
            ILogger<OperationsController> logger,
            IDbContextFactory<ChristmasDbContext> dbFactory) {
            _logger = logger;
            _dbFactory = dbFactory;
        }

        [HttpPost]
        public async Task Add(GiftOperationAddDto dto) {
            if(!dto.IsValid()) {
                HttpContext.Response.StatusCode = 422;
                return;
            }
            using var db = _dbFactory.CreateDbContext();
            
            var model = new GiftOperation {
                OperationId = dto.OperationId,
                ElfId = dto.ElfId,
                GiftId = dto.GiftId,
            };
            var n = await db.Operations.SingleAsync(o => o.Id == model.Operation.Id);
            if(n.Name == "MessaInSlitta") {
                if(dto.UncleChristmasId != -1) {
                    model.UncleChristmasId = dto.UncleChristmasId;
                } else {
                    db.Dispose();
                    HttpContext.Response.StatusCode = 422;
                    return;
                }
            }
            try {
                db.Add(model);
                await db.SaveChangesAsync();
            } catch (Exception ex){
                _logger.LogInformation(ex,"Unable to save model due to {0}",ex.Message);
            }
            
        }
    }
}
