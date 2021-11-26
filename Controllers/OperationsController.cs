using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SantaClausCrm.DataAccess;
using SantaClausCrm.Dtos;
using SantaClausCrm.Models;
using System;
using System.Linq;

namespace SantaClausCrm.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
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
        public async Task<ResultDto> Add(GiftOperationAddDto dto) {
            try {
                if(!dto.IsValid()) {
                    throw new ArgumentException("The dto inserted wasn't valid");
                }
                using var db = _dbFactory.CreateDbContext();               
                var model = new GiftOperation {
                    OperationId = dto.OperationId,
                    ElfId = dto.ElfId,
                    GiftId = dto.GiftId,                    
                };
                var last = await db.GiftOperations.Where(g => g.GiftId == model.GiftId).DefaultIfEmpty().OrderBy(o => o.Id).LastAsync();
                if(model.OperationId == (int)OperationChecker.Costruzione && last is null) 
                {
                    model.UncleChristmasId = -1;
                } else if(last.OperationId == (int)OperationChecker.Costruzione 
                    && model.OperationId == (int)OperationChecker.Impacchettamento)
                {
                    model.UncleChristmasId = -1;
                } else if(last.OperationId == (int)OperationChecker.Impacchettamento 
                    && dto.UncleChristmasId != -1 
                    && model.OperationId==(int)OperationChecker.MessaInSlitta) 
                { 
                    model.UncleChristmasId = dto.UncleChristmasId;
                } else {
                    throw new InvalidOperationException("can't do this");
                }

                db.Add(model);
                await db.SaveChangesAsync();
                return new ResultDto { NewId = model.Id }; 
            } catch (Exception ex) {
                _logger.LogInformation(ex,"Unable to save model because {0}",ex.Message);
                HttpContext.Response.StatusCode = 422;
                return null;
            }
            
        }
    }
    enum OperationChecker
    {
        Costruzione = 1,
        Impacchettamento =2,
        MessaInSlitta = 3,
    }
}
