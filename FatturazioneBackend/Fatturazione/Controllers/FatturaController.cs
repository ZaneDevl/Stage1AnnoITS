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
        public async Task<IActionResult> GeneraTestata([FromQuery] string donumdoc)
        {
            try
            {
                if (string.IsNullOrEmpty(donumdoc)) 
                {
                    return BadRequest("Il numero del documento non può essere vuoto");
                }

                var donumdocTrimmed = donumdoc.Trim().ToUpper();
                _logger.LogInformation($"Valori usati per la query: Dotipdoc = 'FAT', Doflcicl = 'VEN', Docodcau = 'FATVENINTR', Donumdoc = '{donumdocTrimmed}'");

                var query = await _context.BaDocumeM005s
                    .Where(doc => doc.Dotipdoc == "FAT" && doc.Doflcicl == "VEN" && doc.Docodcau == "FATVENINTR" && doc.Donumdoc.ToUpper()==donumdocTrimmed)
                    .Select(doc => new
                    {
                        doc.Doserial,
                        Numfac = doc.Donumdoc,
                        Vendedor = "8057506459995",
                        Emisor = "8057506459995",
                        Cobrador = "4311501086605",
                        Comprado = "4311501086605",
                        Depto = "",
                        Receptor = "4311501086605",
                        Cliente = "4311501086605",
                        Pagador = "",
                        Pedido = "#ORDINECLIENTE#",
                        Fecha = doc.Dodatdoc,
                        Nodo = "380",
                        Funcion = "9",
                        Rsocial = "",
                        Calle = "",
                        Ciudad = "",
                        Cp = "",
                        Nif = "DE811135425",
                        Razon = "",
                        Albaran = "#DDTNUM#",
                        Contrato = "",
                        Nfacsus = "",
                        Fpag = "",
                        Divisa = "EUR",
                        Sumbruto = 0.0,
                        Sumnetos = _context.BaDocume005s.Where(ri => ri.Doserial == doc.Doserial).Sum(ri => (double?)ri.Doimplor) ?? 0.0,
                        Cargos = 0.0,
                        Descuen = _context.BaDocume005s.Where(ri => ri.Doserial == doc.Doserial).Sum(ri => (double?)ri.Doscorig) ?? 0.0,
                        Baseimp1 = (double?)doc.Dototimp ?? 0.0,
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
                        Total = (double?)doc.Dototimp ?? 0.0,
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
                        Fechaefe = "#DATASPEDIZIONE#",
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
                    })
                    .FirstOrDefaultAsync();

                if (query == null)
                {
                    _logger.LogWarning($"Testata non trovata per il numero fattura: {donumdocTrimmed}");
                    return NotFound("Fattura non trovata");
                }

                var fechaFormatted = query.Fecha.ToString();

                var csv = new StringBuilder();
                //csv.AppendLine("DOSERIAL,NUMFAC,VENDEDOR,EMISOR,COBRADOR,COMPRADO,DEPTO,RECEPTOR,CLIENTE,PAGADOR,PEDIDO,FECHA,NODO,FUNCION,RSOCIAL,CALLE,CIUDAD,CP,NIF,RAZON,ALBARAN,CONTRATO,NFACSUS,FPAG,DIVISA,SUMBRUTO,SUMNETOS,CARGOS,DESCUEN,BASEIMP1,TIPOIMP1,TASAIMP1,IMPIMP1,BASEIMP2,TIPOIMP2,TASAIMP2,IMPIMP2,BASEIMP3,TIPOIMP3,TASAIMP3,IMPIMP3,BASEIMP4,TIPOIMP4,TASAIMP4,IMPIMP4,BASEIMP5,TIPOIMP5,TASAIMP5,IMPIMP5,BASEIMP6,TIPOIMP6,TASAIMP6,IMPIMP6,BASIMPFA,TOTIMP,TOTAL,VTO1,IMPVTO1,VTO2,IMPVTO2,VTO3,IMPVTO3,TPVERDE,CALIF1,SECUEN1,TIPO1,PORCEN1,IMPDES1,CALIF2,SECUEN2,TIPO2,PORCEN2,IMPDES2,CALIF3,SECUEN3,TIPO3,PORCEN3,IMPDES3,CALIF4,SECUEN4,TIPO4,PORCEN4,IMPDES4,CALIF5,SECUEN5,TIPO5,PORCEN5,IMPDES5,ERSOCIAL,ECALLE,EPOBLAC,ECP,ENIF,ERMERCA,NOTAC,NUMREL,RECOGIDA,DESTINO,FECHAEFE,NCONFREC,NIDENTICKET,CONTACTO,TELEFONO,FAX,CODPROV,FECALB,NORMEXEN1,NORMEXEN2,NORMEXEN3,NORMEXEN4,NORMEXEN5,NORMEXEN6,FECHADOC,REFPAGO,ORIGEN,NIFII,NIFPE,NIFIV,NIFPR,NIFSU,NUMMOVI,NUMINCOR,IMPINCOR,FENTMER,FEMIMEN,NUMMEN,PERCFAC,FPEDIDO,CODAPROB,NUMDEVOL,FNUMDEVOL,NOTIFDEVOL,FNOTIFDEVOL,SEDESOC,BUQUE,FEMBARQUE,FORWARDER,RRSOCIAL,RCALLE,RCIUDAD,RCP,AGENENIF,ECAPSOCIAL,LUGCARGA,FCARGA,LUGDESCARGA,MATRICULA,FENTREGA,NUMVEN,CPEXT,ECPEXT,FECFACSUS,EPAIS,RELVTO,DIASVTO,PORCVTO,TIPOIMPDES1,TASAIMPDES1,IMPIMPDES1,BASEIMPDES1,TIPOIMPDES2,TASAIMPDES2,IMPIMPDES2,BASEIMPDES2,TIPOIMPDES3,TASAIMPDES3,IMPIMPDES3,BASEIMPDES3,TIPOIMPDES4,TASAIMPDES4,IMPIMPDES4,BASEIMPDES4,TIPOIMPDES5,TASAIMPDES5,IMPIMPDES5,BASEIMPDES5,PAISBY,REGCRITCAJA,TAXCAT,TAXCAT1,TAXCAT2,TAXCAT3,TAXCAT4,TAXCAT5,TAXCAT6,AUTORIZDEV,NUMPEDVEND,FECTAX,PRODPRODUCTO,REGPRODPRODUCTO");
                csv.AppendLine($"{query.Doserial}; {query.Numfac}; {query.Vendedor}; {query.Emisor}; {query.Cobrador}; {query.Comprado}; {query.Depto}; {query.Receptor}; {query.Cliente}; {query.Pagador}; {query.Pedido}; {fechaFormatted}; {query.Nodo}; {query.Funcion}; {query.Rsocial}; {query.Calle}; {query.Ciudad}; {query.Cp}; {query.Nif}; {query.Razon}; {query.Albaran}; {query.Contrato}; {query.Nfacsus}; {query.Fpag}; {query.Divisa}; {query.Sumbruto}; {query.Sumnetos}; {query.Cargos}; {query.Descuen}; {query.Baseimp1}; {query.Tipoimp1}; {query.Tasaimp1}; {query.Impimp1}; {query.Baseimp2}; {query.Tipoimp2}; {query.Tasaimp2}; {query.Impimp2}; {query.Baseimp3}; {query.Tipoimp3}; {query.Tasaimp3}; {query.Impimp3}; {query.Baseimp4}; {query.Tipoimp4}; {query.Tasaimp4}; {query.Impimp4}; {query.Baseimp5}; {query.Tipoimp5}; {query.Tasaimp5}; {query.Impimp5}; {query.Baseimp6}; {query.Tipoimp6}; {query.Tasaimp6}; {query.Impimp6}; {query.Basimpfa}; {query.Totimp}; {query.Total}; {query.Vto1}; {query.Impvto1}; {query.Vto2}; {query.Impvto2}; {query.Vto3}; {query.Impvto3}; {query.Tpverde}; {query.Calif1}; {query.Secuen1}; {query.Tipo1}; {query.Porcen1}; {query.Impdes1}; {query.Calif2}; {query.Secuen2}; {query.Tipo2}; {query.Porcen2}; {query.Impdes2}; {query.Calif3}; {query.Secuen3}; {query.Tipo3}; {query.Porcen3}; {query.Impdes3}; {query.Calif4}; {query.Secuen4}; {query.Tipo4}; {query.Porcen4}; {query.Impdes4}; {query.Calif5}; {query.Secuen5}; {query.Tipo5}; {query.Porcen5}; {query.Impdes5}; {query.Ersocial}; {query.Ecalle}; {query.Epoblac}; {query.Ecp}; {query.Enif}; {query.Ermerca}; {query.Notac}; {query.Numrel}; {query.Recogida}; {query.Destino}; {query.Fechaefe}; {query.Nconfrec}; {query.Nidenticket}; {query.Contacto}; {query.Telefono}; {query.Fax}; {query.Codprov}; {query.Fecalb}; {query.Normexen1}; {query.Normexen2}; {query.Normexen3}; {query.Normexen4}; {query.Normexen5}; {query.Normexen6}; {query.Fechadoc}; {query.Refpago}; {query.Origen}; {query.Nifii}; {query.Nifpe}; {query.Nifiv}; {query.Nifpr}; {query.Nifsu}; {query.Nummovi}; {query.Numincor}; {query.Impincor}; {query.Fentmer}; {query.Femimen}; {query.Nummen}; {query.Percfac}; {query.Fpedido}; {query.Codaprob}; {query.Numdevol}; {query.Fnumdevol}; {query.Notifdevol}; {query.Fnotifdevol}; {query.Sedesoc}; {query.Buque}; {query.Fembarque}; {query.Forwarder}; {query.Rrsocial}; {query.Rcalle}; {query.Rciudad}; {query.Rcp}; {query.Agenenif}; {query.Ecapsocial}; {query.Lugcarga}; {query.Fcarga}; {query.Lugdescarga}; {query.Matricula}; {query.Fentrega}; {query.Numven}; {query.Cpext}; {query.Ecpext}; {query.Fecfacsus}; {query.Epais}; {query.Relvto}; {query.Diasvto}; {query.Porcvto}; {query.Tipoimpdes1}; {query.Tasaimpdes1}; {query.Impimpdes1}; {query.Baseimpdes1}; {query.Tipoimpdes2}; {query.Tasaimpdes2}; {query.Impimpdes2}; {query.Baseimpdes2}; {query.Tipoimpdes3}; {query.Tasaimpdes3}; {query.Impimpdes3}; {query.Baseimpdes3}; {query.Tipoimpdes4}; {query.Tasaimpdes4}; {query.Impimpdes4}; {query.Baseimpdes4}; {query.Tipoimpdes5}; {query.Tasaimpdes5}; {query.Impimpdes5}; {query.Baseimpdes5}; {query.Paisby}; {query.Regcritcaja}; {query.Taxcat}; {query.Taxcat1}; {query.Taxcat2}; {query.Taxcat3}; {query.Taxcat4}; {query.Taxcat5}; {query.Taxcat6}; {query.Autorizdev}; {query.Numpedvend}; {query.Fectax}; {query.Prodproducto}; {query.Regprodproducto}");


                var fileBytes = Encoding.UTF8.GetBytes(csv.ToString());
                return File(fileBytes, "text/csv", $"TestataFattura_{donumdocTrimmed}.csv");
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
                return File(fileBytes, "text/csv", $"RigheFattura_{donumdocTrimmed}.csv");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la generazione del file delle righe");
                return StatusCode(500, "Errore interno del server");
            }
        }
    }
}
