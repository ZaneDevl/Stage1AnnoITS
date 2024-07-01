namespace Fatturazione.Models
{
    public class FatturaResponseDTO
    {
        public TestataFatturaDTO Testata { get; set; }
        public List<RigaFatturaDTO> Righe { get; set; }
    }

    public class TestataFatturaDTO
    {
        public string Numfac { get; set; }
        public DateTime Fentrega { get; set; }
        public decimal Baseimp1 { get; set; }
        public decimal Sumnetos { get; set; }
        public decimal Descuen { get; set; }
    }


    public class RigaFatturaDTO
    {
        public string Desc { get; set; }
        public decimal Cfact { get; set; }
        public decimal Preciob { get; set; }
        public decimal Precion { get; set; }
        public decimal Neto { get; set; }
    }

}