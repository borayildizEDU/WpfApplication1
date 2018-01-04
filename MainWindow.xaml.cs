using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
    {
        WpfApplication1.ViewModel.ChartViewModel chartViewModelObject = new WpfApplication1.ViewModel.ChartViewModel();

        public MainWindow()
        {
          InitializeComponent();
          chartViewModelObject = new WpfApplication1.ViewModel.ChartViewModel();
        }

    private void ChartView_Loaded(object sender, RoutedEventArgs e) {
      ChartViewControl.DataContext = chartViewModelObject;
    }

    private void SettingsView_Loaded(object sender, RoutedEventArgs e) {
      SettingsViewControl.DataContext = chartViewModelObject;
    }

    private void InfoView_Loaded(object sender, RoutedEventArgs e) {
      InfoViewControl.DataContext = chartViewModelObject;
    }

    private void StatusView_Loaded(object sender, RoutedEventArgs e) {
      StatusViewControl.DataContext = chartViewModelObject;
    }

  }

}
