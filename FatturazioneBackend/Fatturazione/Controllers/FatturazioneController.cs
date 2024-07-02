using Fatturazione.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Fatturazione.Controllers;

namespace Fatturazione.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class FatturazioneController : ControllerBase
    {
        private readonly ILogger<FatturazioneController> _logger;
        private readonly FatturazioneDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public FatturazioneController(FatturazioneDbContext context, ILogger<FatturazioneController> logger, IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
        }

        [HttpGet("GeneraFileFatturazione")]
        public async Task<IActionResult> GeneraFileFatturazione([FromQuery] string donumdoc)
        {
            try
            {
                if (string.IsNullOrEmpty(donumdoc))
                {
                    return BadRequest("Il numero di fattura non può essere vuoto");  
                }

                var donumdocTrimmed = donumdoc.Trim().ToUpper();
                var sanitizedDonumdoc = donumdocTrimmed.Replace("/", "_").Replace("\\", "_");

                var fatturaLogger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<FatturaController>();
                var fatturaController = new FatturaController(_context, fatturaLogger);


                var testataFileResult = await fatturaController.GeneraTestata(donumdocTrimmed) as FileContentResult;
                if (testataFileResult == null || testataFileResult.FileContents == null) 
                {
                    return StatusCode(500, "Errore nella generazione del file di testata");
                }
                var testataFileBytes = testataFileResult.FileContents;
                var testataFileName = $"TestataFattura_{sanitizedDonumdoc}.csv";

                var righeFileResult = await fatturaController.GeneraRighe(donumdocTrimmed) as FileContentResult;
                if (righeFileResult == null || righeFileResult.FileContents == null)
                {
                    return StatusCode(500, "Errore nella generazione del file di riga");
                }
                var righeFileBytes = righeFileResult?.FileContents;
                var righeFileName = $"RigheFattura_{sanitizedDonumdoc}.csv";

                var targetDirectory = Path.Combine(_environment.ContentRootPath, "FatturazioneFiles");
                _logger.LogInformation($"Percorso directory di destinazione: {targetDirectory}");

                if (!Directory.Exists(targetDirectory))
                {
                    try 
                    {
                        _logger.LogInformation($"Directory non trovata, creazione della directory: {targetDirectory}");
                        Directory.CreateDirectory(targetDirectory);
                    }
                    catch (Exception ex) 
                    {
                        _logger.LogError(ex, $"Errore durante la creazione della directory {targetDirectory}");
                        return StatusCode(500, "Errore interno del server durante la creazione della directory di destinazione");
                    }
                }    


                var testataFilePath = Path.Combine(targetDirectory, testataFileName);
                var righeFilePath = Path.Combine(targetDirectory, righeFileName);

                _logger.LogInformation($"Percorso file di testata: {testataFilePath}");
                _logger.LogInformation($"Percorso file di righe: {righeFilePath}");

                try
                {
                    await System.IO.File.WriteAllBytesAsync(testataFilePath, testataFileBytes);
                    await System.IO.File.WriteAllBytesAsync(righeFilePath, righeFileBytes);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore durante la scrittura dei file");
                    return StatusCode(500, "Errore interno del server durante la scrittura dei file");
                }

                return Ok(new { TestataFilePath = testataFilePath, RigheFilePath = righeFilePath });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la generazione dei file di fatturazione");
                return StatusCode(500, "Errore interno del server");
            }
        }
    }
}
