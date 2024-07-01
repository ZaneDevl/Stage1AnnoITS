using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fatturazione.Models;
using Microsoft.EntityFrameworkCore;
using System.Formats.Tar;



namespace Fatturazione.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FatturaController : ControllerBase
    {
        private readonly FatturazioneDbContext _context;
        private readonly ILogger<FatturaController> _logger;    

        public FatturaController(FatturazioneDbContext context, ILogger<FatturaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<FatturaResponseDTO>> GetFattura([FromQuery]string donumdoc)
        {
            try
            {
                _logger.LogInformation($"Ricevuta richieta per il numero fattura: {donumdoc}", donumdoc);

                var testataFattura = await _context.BaDocumeM005s
                    .FirstOrDefaultAsync(t => t.Donumdoc == donumdoc
                                            && t.Dotipdoc == "FAT" 
                                            && t.Doflcicl == "VEN"
                                            && t.Docodcau == "FATVENINTR");

                if (testataFattura == null)
                {
                    _logger.LogWarning($"Testata non trovata per questo numero di fattura: {donumdoc}", donumdoc);
                    return NotFound();
                }

                var righeFattura = await _context.BaDocume005s
                    .Join(_context.BaDocumeM005s, ri => ri.Doserial, te => te.Doserial, (ri, te) => new { ri, te })
                    .Join(_context.BaArtkey005s, combined => combined.ri.Dokeyart, art => art.Kakeyart, (combined, art) => new { combined, art })
                    .Where(x => x.combined.te.Donumdoc == donumdoc
                             && x.combined.te.Dotipdoc == "FAT"
                             && x.combined.te.Doflcicl == "VEN"
                             && x.combined.te.Docodcau == "FATVENINTR"
                             && x.combined.ri.Doflgart == 1)
                    .Select(x => new RigaFatturaDTO
                    {
                        Desc = x.art.Kadescri,
                        Cfact = x.combined.ri.Doqtamov ?? 0.0m,
                        Preciob = x.combined.ri.Doprezzo ?? 0.0m,
                        Precion = (x.combined.ri.Doimpnet ?? 0.0m) / (x.combined.ri.Doqtamov ?? 1.0m),  // Conversione esplicita a decimal
                        Neto = x.combined.ri.Doimpnet ?? 0.0m
                    })
                    .ToListAsync();


                decimal sumnetos = righeFattura.Sum(r => r.Preciob * r.Cfact);

                decimal descuen = await _context.BaDocume005s
                    .Where(ri => ri.Doserial == testataFattura.Doserial)
                    .SumAsync(ri => ri.Doscorig ?? 0.0m);

                decimal baseimp1 = testataFattura.Dototimp ?? 0.0m;

                var fatturaDto = new FatturaResponseDTO
                {
                    Testata = new TestataFatturaDTO
                    {
                        Numfac = testataFattura.Donumdoc,
                        Fentrega = new DateTime(testataFattura.Dodatdoc!.Value.Year, testataFattura.Dodatdoc!.Value.Month, testataFattura.Dodatdoc!.Value.Day),
                        Baseimp1 = baseimp1, 
                        Sumnetos = sumnetos,
                        Descuen = descuen
                    },
                    Righe = righeFattura
                };


                Console.WriteLine($"Response per il seriale: {donumdoc}");
                return Ok(fatturaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Errore durante il recupero della fattura: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Errore durante il recupero della fattura");
            }
        }
    }



    public class FatturaDTO
    {
        public BaDocumeM005 Testata { get; set; }
        public List<BaDocume005> Righe { get; set; }
    }
}
