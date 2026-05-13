using System.Windows;

namespace SchraubenSchuette
{
    public partial class MainWindow : Window
    {
        private readonly ProductViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            DatabaseHelper.InitializeDatabase(); // <--- Datenbank anlegen
            _viewModel = new ProductViewModel();
            _viewModel.LadeProdukteAusDatenbank(); // <--- Produkte laden
            this.DataContext = _viewModel;
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddProductWindow(_viewModel); // Hier wird kein ausgewähltes Produkt übergeben
            addWindow.ShowDialog();
        }

        private void BtnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ExportiereProdukteNachExcel();
        }

    }
}