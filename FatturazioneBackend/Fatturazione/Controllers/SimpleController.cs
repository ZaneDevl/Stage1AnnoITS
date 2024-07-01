using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fatturazione.Models;
using System.Threading.Tasks;

namespace Fatturazione.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimpleController : ControllerBase
    {
        private readonly FatturazioneDbContext _context;

        public SimpleController(FatturazioneDbContext context)
        {
            _context = context;
        }

        [HttpGet("first")]
        public async Task<ActionResult<BaDocumeM005>> GetFirstDocument()
        {
            try
            {
                var document = await _context.BaDocumeM005s.FirstOrDefaultAsync();
                if (document == null)
                {
                    return NotFound("Documento non trovato.");
                }
                return Ok(document);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Errore durante il recupero del documento: {ex.Message}");
            }
        }
    }
}
