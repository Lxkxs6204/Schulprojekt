namespace SchraubenSchuette.Models
{
    public class Product
    {
        public int Id { get; set; } // Required for EF Core
        public string Artikelnummer { get; set; }
        public string Bezeichnung { get; set; }
        public int Anzahl { get; set; }
        public string Material { get; set; }
        public string Maße { get; set; }
        public decimal Einkaufspreis { get; set; }
        public decimal Verkaufspreis { get; set; }
        public string Lieferant { get; set; }
        public string BildPfad { get; set; }
    }
}