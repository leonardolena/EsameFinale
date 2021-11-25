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
                var last = db.GiftOperations.Where(g => g.GiftId == model.GiftId).OrderBy(g => g.Id).Last();
                var operations = await db.Operations.ToDictionaryAsync(k => k.Id);  
                var n = operations.ContainsKey(model.OperationId) 
                    ? operations.Values.Where(k => k.Id == model.OperationId).Single() 
                    : throw null;
                if(n.Name == "MessaInSlitta" && last.OperationId == operations.Values.Where(k => k.Name == "Impacchettamento").Single().Id) {
                    if(dto.UncleChristmasId != -1) {
                        model.UncleChristmasId = dto.UncleChristmasId;
                    } else {
                        db.Dispose();
                        throw new ArgumentException("Uncle was not provided");
                    }
                }  
                if(n.Name == "Impacchettamento" && last.OperationId == operations.Values.Where(k => k.Name != "Costruzione").Single().Id)   
                    throw new InvalidOperationException("no packing before construction");   
                
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
}
