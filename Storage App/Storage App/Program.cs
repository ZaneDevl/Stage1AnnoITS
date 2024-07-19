using System;

class ProdottoMagazzino
{
    private double quantitaMagazzino;

    public ProdottoMagazzino()
    {
        quantitaMagazzino = 0;
    }

    public void aggiuntaMagazzino(double amount)
    {
        quantitaMagazzino += amount;
        Console.WriteLine($"Aggiunti {amount} grammi. Scorta attuale: {quantitaMagazzino} grammi");
    }

    public void VenditaProdotto(double soldi, double prezzoPerProdotto)
    {
        double prodottoDaVendere = soldi / prezzoPerProdotto;
        if (prodottoDaVendere > quantitaMagazzino)
        {
            Console.WriteLine("Non c'è abbastanza prodotto per completare la vendita.");
            return;
        }

        quantitaMagazzino -= prodottoDaVendere;
        Console.WriteLine($"Venduti {prodottoDaVendere:F2} grammi per {soldi} euro. Quantità rimanente {quantitaMagazzino:F2} grammi");
    }

    public void SottraDallaScorta(double amount)
    {
        if (amount > quantitaMagazzino)
        {
            Console.WriteLine("La quantità da sottrarre è maggiore è maggiore della quantità in magazzino.");
            return;
        }

        quantitaMagazzino -= amount;
        Console.WriteLine($"Sottratti {amount:F2} grammi dalla scorta, quantità rimanente: {quantitaMagazzino} grammi.");
    }

    public string GetQuantitaEGuadagno()
    {
        double guadagnoMinimo = quantitaMagazzino * 10.5;
        double guadagnoMassimo = quantitaMagazzino * 12.5;

        return $"Quantità rimanente: {quantitaMagazzino:F2}g, guadagno minimo: {guadagnoMinimo:F2} euro, guadagno massimo: {guadagnoMassimo:F2} euro";
    }

}

partial class Program
{
    static void Main(string[] args)
    {
        ProdottoMagazzino magazzino = new ProdottoMagazzino();

        while (true)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Aggiungi alla scorta");
            Console.WriteLine("2. Vendi prodotto");
            Console.WriteLine("3. Sottrai alla scorta");
            Console.WriteLine("4. Visualizza la scorta rimanente");
            Console.WriteLine("5. Esci");

            Console.Write("Scegli un'opzione: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.WriteLine("Inserisci la quantità del prodotto da aggiungere:  ");
                double amount = Convert.ToDouble(Console.ReadLine());
                magazzino.aggiuntaMagazzino(amount);
            }
            else if (choice == "2")
            {
                Console.WriteLine("Inserisci l'importo di denaro per la vendita: ");
                double soldi = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Inserisci il prezzo per grammo: ");
                double prezzoPerProdotto = Convert.ToDouble(Console.ReadLine());
                magazzino.VenditaProdotto(soldi, prezzoPerProdotto);
            }
            else if (choice == "3")
            {
                Console.WriteLine("Inserisci la quantità da sottrarre alla scorta: ");
                double amount = Convert.ToDouble(Console.ReadLine());
                magazzino.SottraDallaScorta(amount);
            }
            else if (choice == "4")
            {
                string quantitaEGuadagno = magazzino.GetQuantitaEGuadagno();
                Console.WriteLine(quantitaEGuadagno);
            }
            else if (choice == "5")
            {
                Console.WriteLine("Uscita dal programma.");
                break;
            }
            else
            {
                Console.WriteLine("Opzione non valida. Riprova.");
            }
        }
    }
}