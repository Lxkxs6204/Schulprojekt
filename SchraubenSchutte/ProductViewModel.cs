using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ClosedXML.Excel;
using Microsoft.Win32;

namespace SchraubenSchuette
{
    public class ProductViewModel
    {
        public ObservableCollection<Product> Produkte { get; set; }
        public ObservableCollection<Product> GefilterteProdukte { get; set; }
        public ICommand AddProductCommand { get; set; }
        public string Suchtext { get; set; }
        public Product SelectedProduct { get; set; }

        public ProductViewModel()
        {
            Produkte = new ObservableCollection<Product>();
            GefilterteProdukte = new ObservableCollection<Product>();

            AddProductCommand = new RelayCommand<Product>((product) => ProduktHinzufuegen(product));
        }

        private void ProduktHinzufuegen(Product product)
        {
            Produkte.Add(product);
            DatabaseHelper.ProduktSpeichern(product);
            UpdateGefilterteProdukte();
        }

        public void LadeProdukteAusDatenbank()
        {
            var geladeneProdukte = DatabaseHelper.ProdukteLaden();
            Produkte.Clear();
            foreach (var produkt in geladeneProdukte)
            {
                Produkte.Add(produkt);
            }
            UpdateGefilterteProdukte();
        }

        public void UpdateGefilterteProdukte()
        {
            GefilterteProdukte.Clear();

            foreach (var produkt in Produkte.Where(p =>
                string.IsNullOrEmpty(Suchtext) ||
                p.Bezeichnung.IndexOf(Suchtext, StringComparison.OrdinalIgnoreCase) >= 0 ||
                p.Artikelnummer.IndexOf(Suchtext, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                GefilterteProdukte.Add(produkt);
            }
        }

        // Neue Funktion: Exportiere Produkte als Excel-Datei
        public void ExportiereProdukteNachExcel()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel-Dateien (*.xlsx)|*.xlsx",
                FileName = "ProdukteExport.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Produkte");

                    // Kopfzeile
                    worksheet.Cell(1, 1).Value = "Artikelnummer";
                    worksheet.Cell(1, 2).Value = "Bezeichnung";
                    worksheet.Cell(1, 3).Value = "Anzahl";
                    worksheet.Cell(1, 4).Value = "Material";
                    worksheet.Cell(1, 5).Value = "Maße";
                    worksheet.Cell(1, 6).Value = "Einkaufspreis";
                    worksheet.Cell(1, 7).Value = "Verkaufspreis";
                    worksheet.Cell(1, 8).Value = "Lieferant";
                    worksheet.Cell(1, 9).Value = "BildPfad";

                    // Inhalte
                    for (int i = 0; i < Produkte.Count; i++)
                    {
                        var p = Produkte[i];
                        worksheet.Cell(i + 2, 1).Value = p.Artikelnummer;
                        worksheet.Cell(i + 2, 2).Value = p.Bezeichnung;
                        worksheet.Cell(i + 2, 3).Value = p.Anzahl;
                        worksheet.Cell(i + 2, 4).Value = p.Material;
                        worksheet.Cell(i + 2, 5).Value = p.Maße;
                        worksheet.Cell(i + 2, 6).Value = p.Einkaufspreis;
                        worksheet.Cell(i + 2, 7).Value = p.Verkaufspreis;
                        worksheet.Cell(i + 2, 8).Value = p.Lieferant;
                        worksheet.Cell(i + 2, 9).Value = p.BildPfad;
                    }

                    worksheet.Columns().AdjustToContents();
                    workbook.SaveAs(saveFileDialog.FileName);
                }
            }
        }
    }

    public class Product
    {
        public string Artikelnummer { get; set; }
        public string Bezeichnung { get; set; }
        public int Anzahl { get; set; }
        public string Material { get; set; }
        public string Maße { get; set; }
        public decimal Einkaufspreis { get; set; }
        public decimal Verkaufspreis { get; set; }
        public string Lieferant { get; set; }
        public string BildPfad { get; set; } // Optionales Bild
    }
}
