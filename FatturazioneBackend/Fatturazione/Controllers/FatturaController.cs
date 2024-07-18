using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fatturazione.Models;
using Microsoft.EntityFrameworkCore;
using System.Formats.Tar;
using System.Text;
using System;
using System.Text.Encodings;
using NodaTime;
using NodaTime.Text;




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
                _logger.LogInformation($"Ricevuta richiesta per il numero fattura: {donumdoc}", donumdoc);

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

        [HttpGet("GeneraTestata")]
        public async Task<IActionResult> GeneraTestata([FromQuery] string donumdoc, [FromQuery] int? year = null)
        {
            try
            {
                if (string.IsNullOrEmpty(donumdoc)) 
                {
                    return BadRequest("Il numero del documento non può essere vuoto");
                }

                var donumdocTrimmed = donumdoc.Trim().ToUpper();
                _logger.LogInformation($"Valori usati per la query: Dotipdoc = 'FAT', Doflcicl = 'VEN', Docodcau = 'FATVENINTR', Donumdoc = '{donumdocTrimmed}'");

                var fatture = await _context.BaDocumeM005s
                    .Where(doc => doc.Dotipdoc == "FAT" && doc.Doflcicl =="VEN" && doc.Docodcau =="FATVENINTR" && doc.Donumdoc.ToUpper() == donumdocTrimmed)
                    .Select(doc => new 
                    {
                        doc.Doserial,
                        doc.Donumdoc,
                        doc.Dodatreg
                    })
                    .ToListAsync();
                if (!fatture.Any())
                {
                    return NotFound("Fattura non trovata");
                }

                if (fatture.Count > 1)
                {
                    if (year == null)
                    {
                        return BadRequest("Specifica l'anno per identificare la fattura corretta");
                    }

                    fatture = fatture.Where(f => f.Dodatreg.HasValue && f.Dodatreg.Value.Year == year.Value).ToList();

                    if (!fatture.Any())
                    {
                        return NotFound("Fattura non trovata per l'anno specificato");
                    }
                }

                var fattura = fatture.First();

                var fatturaDetails = await _context.BaDocumeM005s
                //.Where(doc => doc.Dotipdoc == "FAT" && doc.Doflcicl == "VEN" && doc.Docodcau == "FATVENINTR" && doc.Donumdoc.ToUpper() == donumdocTrimmed)
                  .Where(doc => doc.Doserial == fattura.Doserial)
                    .Select(doc => new
                    {
                        doc.Doserial,
                        doc.Donumdoc,
                        doc.Dodatdoc,
                        Sumnetos = _context.BaDocume005s.Where(ri => ri.Doserial == doc.Doserial).Sum(ri => (double?)ri.Doimplor) ?? 0.0,
                        Descuen = _context.BaDocume005s.Where(ri => ri.Doserial == doc.Doserial).Sum(ri => (double?)ri.Doscorig) ?? 0.0,
                        doc.Dototimp
                    })
                    .FirstOrDefaultAsync();

                if (fatturaDetails == null)
                {
                    return NotFound("Fattura non trovata");
                }

                var doserPreDDT = await _context.BaDocume005s
                    .Where(riga => riga.Doserial == fattura.Doserial && riga.Doflgart == 1)
                    .MaxAsync(riga => (string?)riga.Doserpre);

                if (doserPreDDT == null)
                {
                    return NotFound("Chiave della testata DDT non trovata");
                }

                var ddt = await _context.BaDocumeM005s
                    .Where(doc => doc.Doserial == doserPreDDT)
                    .Select(doc => new {doc.Doserial, doc.Dotipdoc, doc.Donumreg, doc.Donumdoc, doc.Dodatreg})
                    .FirstOrDefaultAsync();

                if (ddt == null)
                {
                    return NotFound("TestataDDT non trovata");
                }

                var doserPreOrd = await _context.BaDocume005s
                    .Where(riga => riga.Doserial == ddt.Doserial)
                    .MaxAsync(riga => (string?)riga.Doserpre);

                if (doserPreOrd == null)
                {
                    return NotFound("Chiave della testata ORD non trovata");
                }

                var ordineCliente = await _context.BaDocumeM005s
                    .Where(doc => doc.Doserial == doserPreOrd)
                    .Select(doc => new {doc.Doserial, doc.Dotipdoc, doc.Donumreg, doc.Donumdoc})
                    .FirstOrDefaultAsync();

                if (ordineCliente == null)
                {
                    return NotFound("Testata ORDINE CLIENTE non trovata");
                }

                var queryResult = new
                {
                    Doserial = fatturaDetails.Doserial,
                    Numfac = fatturaDetails.Donumdoc,
                    Vendedor = "8057506459995",
                    Emisor = "8057506459995",
                    Cobrador = "4311501086605",
                    Comprado = "4311501086605",
                    Depto = "",
                    Receptor = "4311501086605",
                    Cliente = "4311501086605",
                    Pagador = "",
                    Pedido = ordineCliente.Donumdoc,
                    Fecha = fatturaDetails.Dodatdoc,
                    Nodo = "380",
                    Funcion = "9",
                    Rsocial = "",
                    Calle = "",
                    Ciudad = "",
                    Cp = "",
                    Nif = "DE811135425",
                    Razon = "",
                    Albaran = ddt.Donumreg,
                    Contrato = "",
                    Nfacsus = "",
                    Fpag = "",
                    Divisa = "EUR",
                    Sumbruto = 0.0,
                    Sumnetos = fatturaDetails.Sumnetos,
                    Cargos = 0.0,
                    Descuen = fatturaDetails.Descuen,
                    Baseimp1 = (double?)fatturaDetails.Dototimp ?? 0.0,
                    Tipoimp1 = "VAT",
                    Tasaimp1 = 0.0,
                    Impimp1 = 0.0,
                    Baseimp2 = "",
                    Tipoimp2 = "",
                    Tasaimp2 = "",
                    Impimp2 = "",
                    Baseimp3 = "",
                    Tipoimp3 = "",
                    Tasaimp3 = "",
                    Impimp3 = "",
                    Baseimp4 = "",
                    Tipoimp4 = "",
                    Tasaimp4 = "",
                    Impimp4 = "",
                    Baseimp5 = "",
                    Tipoimp5 = "",
                    Tasaimp5 = "",
                    Impimp5 = "",
                    Baseimp6 = "",
                    Tipoimp6 = "",
                    Tasaimp6 = "",
                    Impimp6 = "",
                    Basimpfa = 0.0,
                    Totimp = 0.0,
                    Total = (double?)fatturaDetails.Dototimp ?? 0.0,
                    Vto1 = "",
                    Impvto1 = "",
                    Vto2 = "",
                    Impvto2 = "",
                    Vto3 = "",
                    Impvto3 = "",
                    Tpverde = "",
                    Calif1 = "",
                    Secuen1 = "",
                    Tipo1 = "",
                    Porcen1 = "",
                    Impdes1 = "",
                    Calif2 = "",
                    Secuen2 = "",
                    Tipo2 = "",
                    Porcen2 = "",
                    Impdes2 = "",
                    Calif3 = "",
                    Secuen3 = "",
                    Tipo3 = "",
                    Porcen3 = "",
                    Impdes3 = "",
                    Calif4 = "",
                    Secuen4 = "",
                    Tipo4 = "",
                    Porcen4 = "",
                    Impdes4 = "",
                    Calif5 = "",
                    Secuen5 = "",
                    Tipo5 = "",
                    Porcen5 = "",
                    Impdes5 = "",
                    Ersocial = "",
                    Ecalle = "",
                    Epoblac = "",
                    Ecp = "",
                    Enif = "IT04418490241",
                    Ermerca = "",
                    Notac = "",
                    Numrel = "",
                    Recogida = "",
                    Destino = "",
                    Fechaefe = ddt.Dodatreg,
                    Nconfrec = "",
                    Nidenticket = "",
                    Contacto = "",
                    Telefono = "",
                    Fax = "",
                    Codprov = "",
                    Fecalb = "",
                    Normexen1 = "",
                    Normexen2 = "",
                    Normexen3 = "",
                    Normexen4 = "",
                    Normexen5 = "",
                    Normexen6 = "",
                    Fechadoc = "",
                    Refpago = "",
                    Origen = "",
                    Nifii = "",
                    Nifpe = "",
                    Nifiv = "",
                    Nifpr = "",
                    Nifsu = "",
                    Nummovi = "",
                    Numincor = "",
                    Impincor = "",
                    Fentmer = "",
                    Femimen = "",
                    Nummen = "",
                    Percfac = "",
                    Fpedido = "",
                    Codaprob = "",
                    Numdevol = "",
                    Fnumdevol = "",
                    Notifdevol = "",
                    Fnotifdevol = "",
                    Sedesoc = "",
                    Buque = "",
                    Fembarque = "",
                    Forwarder = "",
                    Rrsocial = "",
                    Rcalle = "",
                    Rciudad = "",
                    Rcp = "",
                    Agenenif = "",
                    Ecapsocial = "",
                    Lugcarga = "",
                    Fcarga = "",
                    Lugdescarga = "",
                    Matricula = "",
                    Fentrega = "",
                    Numven = "",
                    Cpext = "",
                    Ecpext = "",
                    Fecfacsus = "",
                    Epais = "",
                    Relvto = "",
                    Diasvto = "",
                    Porcvto = "",
                    Tipoimpdes1 = "",
                    Tasaimpdes1 = "",
                    Impimpdes1 = "",
                    Baseimpdes1 = "",
                    Tipoimpdes2 = "",
                    Tasaimpdes2 = "",
                    Impimpdes2 = "",
                    Baseimpdes2 = "",
                    Tipoimpdes3 = "",
                    Tasaimpdes3 = "",
                    Impimpdes3 = "",
                    Baseimpdes3 = "",
                    Tipoimpdes4 = "",
                    Tasaimpdes4 = "",
                    Impimpdes4 = "",
                    Baseimpdes4 = "",
                    Tipoimpdes5 = "",
                    Tasaimpdes5 = "",
                    Impimpdes5 = "",
                    Baseimpdes5 = "",
                    Paisby = "",
                    Regcritcaja = "",
                    Taxcat = "",
                    Taxcat1 = "",
                    Taxcat2 = "",
                    Taxcat3 = "",
                    Taxcat4 = "",
                    Taxcat5 = "",
                    Taxcat6 = "",
                    Autorizdev = "",
                    Numpedvend = "",
                    Fectax = "",
                    Prodproducto = "",
                    Regprodproducto = ""
                };


                if (queryResult == null)
                {
                    _logger.LogWarning($"Testata non trovata per il numero fattura: {donumdocTrimmed}");
                    return NotFound("Fattura non trovata");
                }

                var pattern = LocalDatePattern.CreateWithInvariantCulture("yyyyMMdd");
                var fechaFormatted = queryResult.Fecha.HasValue ? pattern.Format( queryResult.Fecha.Value) : string.Empty;
                var fechaEfeFormatted = queryResult.Fechaefe.HasValue ? pattern.Format(queryResult.Fechaefe.Value) : string.Empty;

                var csv = new StringBuilder();
                //csv.AppendLine("DOSERIAL,NUMFAC,VENDEDOR,EMISOR,COBRADOR,COMPRADO,DEPTO,RECEPTOR,CLIENTE,PAGADOR,PEDIDO,FECHA,NODO,FUNCION,RSOCIAL,CALLE,CIUDAD,CP,NIF,RAZON,ALBARAN,CONTRATO,NFACSUS,FPAG,DIVISA,SUMBRUTO,SUMNETOS,CARGOS,DESCUEN,BASEIMP1,TIPOIMP1,TASAIMP1,IMPIMP1,BASEIMP2,TIPOIMP2,TASAIMP2,IMPIMP2,BASEIMP3,TIPOIMP3,TASAIMP3,IMPIMP3,BASEIMP4,TIPOIMP4,TASAIMP4,IMPIMP4,BASEIMP5,TIPOIMP5,TASAIMP5,IMPIMP5,BASEIMP6,TIPOIMP6,TASAIMP6,IMPIMP6,BASIMPFA,TOTIMP,TOTAL,VTO1,IMPVTO1,VTO2,IMPVTO2,VTO3,IMPVTO3,TPVERDE,CALIF1,SECUEN1,TIPO1,PORCEN1,IMPDES1,CALIF2,SECUEN2,TIPO2,PORCEN2,IMPDES2,CALIF3,SECUEN3,TIPO3,PORCEN3,IMPDES3,CALIF4,SECUEN4,TIPO4,PORCEN4,IMPDES4,CALIF5,SECUEN5,TIPO5,PORCEN5,IMPDES5,ERSOCIAL,ECALLE,EPOBLAC,ECP,ENIF,ERMERCA,NOTAC,NUMREL,RECOGIDA,DESTINO,FECHAEFE,NCONFREC,NIDENTICKET,CONTACTO,TELEFONO,FAX,CODPROV,FECALB,NORMEXEN1,NORMEXEN2,NORMEXEN3,NORMEXEN4,NORMEXEN5,NORMEXEN6,FECHADOC,REFPAGO,ORIGEN,NIFII,NIFPE,NIFIV,NIFPR,NIFSU,NUMMOVI,NUMINCOR,IMPINCOR,FENTMER,FEMIMEN,NUMMEN,PERCFAC,FPEDIDO,CODAPROB,NUMDEVOL,FNUMDEVOL,NOTIFDEVOL,FNOTIFDEVOL,SEDESOC,BUQUE,FEMBARQUE,FORWARDER,RRSOCIAL,RCALLE,RCIUDAD,RCP,AGENENIF,ECAPSOCIAL,LUGCARGA,FCARGA,LUGDESCARGA,MATRICULA,FENTREGA,NUMVEN,CPEXT,ECPEXT,FECFACSUS,EPAIS,RELVTO,DIASVTO,PORCVTO,TIPOIMPDES1,TASAIMPDES1,IMPIMPDES1,BASEIMPDES1,TIPOIMPDES2,TASAIMPDES2,IMPIMPDES2,BASEIMPDES2,TIPOIMPDES3,TASAIMPDES3,IMPIMPDES3,BASEIMPDES3,TIPOIMPDES4,TASAIMPDES4,IMPIMPDES4,BASEIMPDES4,TIPOIMPDES5,TASAIMPDES5,IMPIMPDES5,BASEIMPDES5,PAISBY,REGCRITCAJA,TAXCAT,TAXCAT1,TAXCAT2,TAXCAT3,TAXCAT4,TAXCAT5,TAXCAT6,AUTORIZDEV,NUMPEDVEND,FECTAX,PRODPRODUCTO,REGPRODPRODUCTO");
                csv.AppendLine($"{queryResult.Numfac}; {queryResult.Vendedor}; {queryResult.Emisor}; {queryResult.Cobrador}; {queryResult.Comprado}; {queryResult.Depto}; {queryResult.Receptor}; {queryResult.Cliente}; {queryResult.Pagador}; {queryResult.Pedido}; {fechaFormatted}; {queryResult.Nodo}; {queryResult.Funcion}; {queryResult.Rsocial}; {queryResult.Calle}; {queryResult.Ciudad}; {queryResult.Cp}; {queryResult.Nif}; {queryResult.Razon}; {queryResult.Albaran}; {queryResult.Contrato}; {queryResult.Nfacsus}; {queryResult.Fpag}; {queryResult.Divisa}; {queryResult.Sumbruto}; {queryResult.Sumnetos}; {queryResult.Cargos}; {queryResult.Descuen}; {queryResult.Baseimp1}; {queryResult.Tipoimp1}; {queryResult.Tasaimp1}; {queryResult.Impimp1}; {queryResult.Baseimp2}; {queryResult.Tipoimp2}; {queryResult.Tasaimp2}; {queryResult.Impimp2}; {queryResult.Baseimp3}; {queryResult.Tipoimp3}; {queryResult.Tasaimp3}; {queryResult.Impimp3}; {queryResult.Baseimp4}; {queryResult.Tipoimp4}; {queryResult.Tasaimp4}; {queryResult.Impimp4}; {queryResult.Baseimp5}; {queryResult.Tipoimp5}; {queryResult.Tasaimp5}; {queryResult.Impimp5}; {queryResult.Baseimp6}; {queryResult.Tipoimp6}; {queryResult.Tasaimp6}; {queryResult.Impimp6}; {queryResult.Basimpfa}; {queryResult.Totimp}; {queryResult.Total}; {queryResult.Vto1}; {queryResult.Impvto1}; {queryResult.Vto2}; {queryResult.Impvto2}; {queryResult.Vto3}; {queryResult.Impvto3}; {queryResult.Tpverde}; {queryResult.Calif1}; {queryResult.Secuen1}; {queryResult.Tipo1}; {queryResult.Porcen1}; {queryResult.Impdes1}; {queryResult.Calif2}; {queryResult.Secuen2}; {queryResult.Tipo2}; {queryResult.Porcen2}; {queryResult.Impdes2}; {queryResult.Calif3}; {queryResult.Secuen3}; {queryResult.Tipo3}; {queryResult.Porcen3}; {queryResult.Impdes3}; {queryResult.Calif4}; {queryResult.Secuen4}; {queryResult.Tipo4}; {queryResult.Porcen4}; {queryResult.Impdes4}; {queryResult.Calif5}; {queryResult.Secuen5}; {queryResult.Tipo5}; {queryResult.Porcen5}; {queryResult.Impdes5}; {queryResult.Ersocial}; {queryResult.Ecalle}; {queryResult.Epoblac}; {queryResult.Ecp}; {queryResult.Enif}; {queryResult.Ermerca}; {queryResult.Notac}; {queryResult.Numrel}; {queryResult.Recogida}; {queryResult.Destino}; {fechaEfeFormatted}; {queryResult.Nconfrec}; {queryResult.Nidenticket}; {queryResult.Contacto}; {queryResult.Telefono}; {queryResult.Fax}; {queryResult.Codprov}; {queryResult.Fecalb}; {queryResult.Normexen1}; {queryResult.Normexen2}; {queryResult.Normexen3}; {queryResult.Normexen4}; {queryResult.Normexen5}; {queryResult.Normexen6}; {queryResult.Fechadoc}; {queryResult.Refpago}; {queryResult.Origen}; {queryResult.Nifii}; {queryResult.Nifpe}; {queryResult.Nifiv}; {queryResult.Nifpr}; {queryResult.Nifsu}; {queryResult.Nummovi}; {queryResult.Numincor}; {queryResult.Impincor}; {queryResult.Fentmer}; {queryResult.Femimen}; {queryResult.Nummen}; {queryResult.Percfac}; {queryResult.Fpedido}; {queryResult.Codaprob}; {queryResult.Numdevol}; {queryResult.Fnumdevol}; {queryResult.Notifdevol}; {queryResult.Fnotifdevol}; {queryResult.Sedesoc}; {queryResult.Buque}; {queryResult.Fembarque}; {queryResult.Forwarder}; {queryResult.Rrsocial}; {queryResult.Rcalle}; {queryResult.Rciudad}; {queryResult.Rcp}; {queryResult.Agenenif}; {queryResult.Ecapsocial}; {queryResult.Lugcarga}; {queryResult.Fcarga}; {queryResult.Lugdescarga}; {queryResult.Matricula}; {queryResult.Fentrega}; {queryResult.Numven}; {queryResult.Cpext}; {queryResult.Ecpext}; {queryResult.Fecfacsus}; {queryResult.Epais}; {queryResult.Relvto}; {queryResult.Diasvto}; {queryResult.Porcvto}; {queryResult.Tipoimpdes1}; {queryResult.Tasaimpdes1}; {queryResult.Impimpdes1}; {queryResult.Baseimpdes1}; {queryResult.Tipoimpdes2}; {queryResult.Tasaimpdes2}; {queryResult.Impimpdes2}; {queryResult.Baseimpdes2}; {queryResult.Tipoimpdes3}; {queryResult.Tasaimpdes3}; {queryResult.Impimpdes3}; {queryResult.Baseimpdes3}; {queryResult.Tipoimpdes4}; {queryResult.Tasaimpdes4}; {queryResult.Impimpdes4}; {queryResult.Baseimpdes4}; {queryResult.Tipoimpdes5}; {queryResult.Tasaimpdes5}; {queryResult.Impimpdes5}; {queryResult.Baseimpdes5}; {queryResult.Paisby}; {queryResult.Regcritcaja}; {queryResult.Taxcat}; {queryResult.Taxcat1}; {queryResult.Taxcat2}; {queryResult.Taxcat3}; {queryResult.Taxcat4}; {queryResult.Taxcat5}; {queryResult.Taxcat6}; {queryResult.Autorizdev}; {queryResult.Numpedvend}; {queryResult.Fectax}; {queryResult.Prodproducto}; {queryResult.Regprodproducto}");


                var fileBytes = Encoding.UTF8.GetBytes(csv.ToString());
                return File(fileBytes, "text/plain", $"CABFAC.txt");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la generazione del file di testata per il documento {Donumdoc}", donumdoc);
                return StatusCode(500, "Errore interno del server");
            }
        }

        [HttpGet("GeneraRighe")]
        public async Task<IActionResult> GeneraRighe([FromQuery] string donumdoc)
        {
            try
            {
                if (string.IsNullOrEmpty(donumdoc))
                {
                    return BadRequest("Il numero del documento non può essere vuoto");
                }
                
                var donumdocTrimmed = donumdoc.Trim().ToUpper();
                _logger.LogInformation($"Valori usati per la query: Dotipdoc = 'FAT', Doflcicl = 'VEN', Docodcau = 'FATVENINTR', Donumdoc = '{donumdocTrimmed}'");

                var query = await _context.BaDocume005s
                    .Join(_context.BaDocumeM005s,
                        ri => ri.Doserial,
                        te => te.Doserial,
                        (ri, te) => new { ri, te })
                    .Join(_context.BaArtkey005s,
                        combined => combined.ri.Dokeyart,
                        art => art.Kakeyart,
                        (combined, art) => new
                        {
                            combined.te.Doserial,
                            combined.te.Donumdoc,
                            combined.te.Dotipdoc,
                            combined.te.Doflcicl,
                            combined.te.Docodcau,
                            combined.ri.Cprownum,
                            Refean = "4311501086605",
                            Refcli = "4311501086605",
                            Refpro = "4311501086605",
                            art.Kadescri,
                            combined.ri.Doqtamov,
                            combined.ri.Doprezzo,
                            combined.ri.Doimpnet
                        })
                    .Where(combined => combined.Dotipdoc == "FAT" && combined.Doflcicl == "VEN" && combined.Docodcau == "FATVENINTR" && combined.Donumdoc.ToUpper() == donumdocTrimmed)
                    .OrderBy(combined => combined.Cprownum)
                    .ToListAsync();

                _logger.LogInformation($"Numero di righe trovare: {query.Count}");

                if (query == null || !query.Any())
                {
                    _logger.LogWarning($"Fattura non trovata o nessuna riga associata per la fattura: '{donumdocTrimmed}'");
                    return NotFound("Fattura non trovata o nessuna riga associata");
                }

                var  processedQuery = query.Select((combined, index) => new
                {
                        combined.Doserial,
                        Numfac = combined.Donumdoc,
                        Numlin = index + 1, // Simulate ROW_NUMBER() OVER (ORDER BY CPROWNUM ASC)
                        combined.Refean,
                        combined.Refcli,
                        combined.Refpro,
                        VP = "",
                        DUN14 = "",
                        Desc = combined.Kadescri,
                        Cfact = Math.Round((float)combined.Doqtamov, 2),
                        CENT = "",
                        Umedida = "",
                        PrecioB = Math.Round((float)combined.Doprezzo, 2),
                        PrecioN = Math.Round((float)(combined.Doimpnet / combined.Doqtamov), 2),
                        TipoImp1 = "",
                        TasaImp1 = "",
                        ImpTasa1 = "",
                        TipoImp2 = "",
                        TasaImp2 = "",
                        ImpTasa2 = "",
                        TipoImp3 = "",
                        TasaImp3 = "",
                        ImpTasa3 = "",
                        Calif1 = "",
                        Secuen1 = "",
                        Tipo1 = "",
                        Porcen1 = "",
                        ImpDto1 = "",
                        Calif2 = "",
                        Secuen2 = "",
                        Porcen2 = "",
                        ImpDto2 = "",
                        Calif3 = "",
                        Secuen3 = "",
                        Tipo3 = "",
                        Porcen3 = "",
                        ImpDto3 = "",
                        Calif4 = "",
                        Secuen4 = "",
                        Tipo4 = "",
                        Porcen4 = "",
                        ImpDto4 = "",
                        Cboni = "",
                        Neto = Math.Round((float)combined.Doimpnet, 2),
                        Pverde = "",
                        Pedido = "",
                        Albaran = "",
                        Lote = "",
                        Fcarga = "",
                        Fentrega = "",
                        Matricula = "",
                        Tipart = "",
                        Cantdev = "",
                        Seccion = "",
                        Nidenticket = "",
                        Bruto = "",
                        Fecalb = "",
                        Autor = "",
                        Titulo = "",
                        Cuexp = "",
                        Umcuexp = "",
                        FechaEfe = "",
                        Categoria = "",
                        SubLin = "",
                        Normexen1 = "",
                        Normexen2 = "",
                        Normexen3 = "",
                        TipoEmbalaje = "",
                        NumLinPed = "",
                        GrpLinCom = "",
                        PosLinInt = "",
                        Fpedido = "",
                        UbasePn = "",
                        UmedPn = "",
                        TaxCat = "",
                        Modelo = "",
                        Color = "",
                        ImpRappel = "",
                        PrecioDis = "",
                        Peso = "",
                        Upeso = "",
                        NumEmb = "",
                        NumSubLin = "",
                        DescTipo1 = "",
                        DescTipo2 = "",
                        DescTipo3 = "",
                        DescTipo4 = "",
                        KilosPlas = "",
                        RegProdProducto = ""
                }).ToList();


                var csv = new StringBuilder();
                //csv.AppendLine("NUMFAC,NUMLIN,REFEAN,REFCLI,REFPRO,VP,DUN14,DESC,CFACT,CENT,UMEDIDA,PRECIOB,PRECION,TIPOIMP1,TASAIMP1,IMPTASA1,TIPOIMP2,TASAIMP2,IMPTASA2,TIPOIMP3,TASAIMP3,IMPTASA3,CALIF1,SECUEN1,TIPO1,PORCEN1,IMPDES1,CALIF2,SECUEN2,PORCEN2,IMPDES2,CALIF3,SECUEN3,TIPO3,PORCEN3,IMPDES3,CALIF4,SECUEN4,TIPO4,PORCEN4,IMDTO4,CBONI,NETO,PVERDE,PEDIDO,ALBARAN,LOTE,FCARGA,FENTREGA,MATRICULA,TIPART,CANTDEV,SECCION,NIDENTICKET,BRUTO,FECALB,AUTOR,TITULO,CUEXP,UMCUEXP,FECHAEFE,CATEGORIA,SUBLIN,NORMEXEN1,NORMEXEN2,NORMEXEN3,TIPOEMBALAJE,NUMLINPED,GRPLINCOM,POSLININT,FPEDIDO,UBASEPN,UMEDPN,TAXCAT,MODELO,COLOR,IMPRAPPEL,PRECIODIS,PESO,UPESO,NUMEMB,NUMSUBLIN,DESCTIPO1,DESCTIPO2,DESCTIPO3,DESCTIPO4,KILOSPLAS,REGPRODPRODUCTO");
                
                foreach (var row in processedQuery)
                {
                    csv.AppendLine($"{row.Numfac}; {row.Numlin}; {row.Refean}; {row.Refcli}; {row.Refpro}; {row.VP}; {row.DUN14}; {row.Desc}; {row.Cfact}; {row.CENT}; {row.Umedida}; {row.PrecioB}; {row.PrecioN}; {row.TipoImp1}; {row.TasaImp1}; {row.ImpTasa1}; {row.TipoImp2}; {row.TasaImp2}; {row.ImpTasa2}; {row.TipoImp3}; {row.TasaImp3}; {row.ImpTasa3}; {row.Calif1}; {row.Secuen1}; {row.Tipo1}; {row.Porcen1}; {row.ImpDto1}; {row.Calif2}; {row.Secuen2}; {row.Porcen2}; {row.ImpDto2}; {row.Calif3}; {row.Secuen3}; {row.Tipo3}; {row.Porcen3}; {row.ImpDto3}; {row.Calif4}; {row.Secuen4}; {row.Tipo4}; {row.Porcen4}; {row.ImpDto4}; {row.Cboni}; {row.Neto}; {row.Pverde}; {row.Pedido}; {row.Albaran}; {row.Lote}; {row.Fcarga}; {row.Fentrega}; {row.Matricula}; {row.Tipart}; {row.Cantdev}; {row.Seccion}; {row.Nidenticket}; {row.Bruto}; {row.Fecalb}; {row.Autor}; {row.Titulo}; {row.Cuexp}; {row.Umcuexp}; {row.FechaEfe}; {row.Categoria}; {row.SubLin}; {row.Normexen1}; {row.Normexen2}; {row.Normexen3}; {row.TipoEmbalaje}; {row.NumLinPed}; {row.GrpLinCom}; {row.PosLinInt}; {row.Fpedido}; {row.UbasePn}; {row.UmedPn}; {row.TaxCat}; {row.Modelo}; {row.Color}; {row.ImpRappel}; {row.PrecioDis}; {row.Peso}; {row.Upeso}; {row.NumEmb}; {row.NumSubLin}; {row.DescTipo1}; {row.DescTipo2}; {row.DescTipo3}; {row.DescTipo4}; {row.KilosPlas}; {row.RegProdProducto}");

                }

                var fileBytes = Encoding.UTF8.GetBytes(csv.ToString());
                return File(fileBytes, "text/plain", $"LINFAC.txt");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la generazione del file delle righe");
                return StatusCode(500, "Errore interno del server");
            }
        }
    }
}
