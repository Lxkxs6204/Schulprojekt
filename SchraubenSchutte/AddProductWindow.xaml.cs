using System;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace SchraubenSchuette
{
    public partial class AddProductWindow : Window
    {
        private readonly ProductViewModel _viewModel;
        private string _bildPfad = ""; 

        public AddProductWindow(ProductViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;

            
            this.DataContext = new Product();
        }

        private void BtnHinzufuegen_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Button wurde geklickt");

            int anzahl;
            decimal einkaufspreis, verkaufspreis;

            if (int.TryParse(txtAnzahl.Text, out anzahl) &&
                decimal.TryParse(txtEinkaufspreis.Text, out einkaufspreis) &&
                decimal.TryParse(txtVerkaufspreis.Text, out verkaufspreis))
            {
                var neuesProdukt = new Product
                {
                    Artikelnummer = txtArtikelnummer.Text,
                    Bezeichnung = txtBezeichnung.Text,
                    Anzahl = anzahl,
                    Material = txtMaterial.Text,
                    Maße = txtMaße.Text,
                    Einkaufspreis = einkaufspreis,
                    Verkaufspreis = verkaufspreis,
                    Lieferant = txtLieferant.Text,
                    BildPfad = _bildPfad
                };

          
                DatabaseHelper.ProduktSpeichern(neuesProdukt);

                _viewModel.Produkte.Add(neuesProdukt);
                Console.WriteLine($"Produkt hinzugefügt: {neuesProdukt.Bezeichnung}");

                _viewModel.UpdateGefilterteProdukte();

                Close();
            }
            else
            {
                MessageBox.Show("Bitte geben Sie gültige Werte ein.");
            }
        }

        private void BtnBildAuswaehlen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Bilddateien (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

            if (dlg.ShowDialog() == true)
            {
                _bildPfad = dlg.FileName;
                lblBildPfad.Text = _bildPfad;

               
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_bildPfad);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                imgVorschau.Source = bitmap;
            }
        }
    }
}
