using DataApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfAppTest01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ILogger<MainWindow> _logger;
        private EFDatabaseContext _dbContext;

        public MainWindow(ILogger<MainWindow> logger, EFDatabaseContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _logger.LogInformation("MainWindow");
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button button = e.Source as Button;
            if (button == null) return;
            if (button.Name == nameof(btnQueryData))
            {
                _logger.LogDebug(_dbContext.Products.Count().ToString());
                var products = _dbContext.Products.Include(x => x.ProductShipments).ThenInclude(x => x.Shipment).ToList();
                foreach (var product in products)
                {
                    _logger.LogDebug($"{product.Name}");
                    foreach (var c in product.ProductShipments)
                    {
                        var shipment = c.Shipment;
                        _logger.LogDebug($">>{shipment.ShipperName},{shipment.StartCity}-{shipment.EndCity}");
                    }
                }
            }
        }
    }
}