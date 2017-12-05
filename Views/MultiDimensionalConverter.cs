using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;

namespace WpfApplication1.Views {
  /*
  reference: https://stackoverflow.com/questions/16119200/wpf-binding-to-multidimensional-array-in-the-xaml
  */
  public class MultiDimensionalConverter : IMultiValueConverter {
    #region Constants
    public const double OPACITY_ON = 1.0;
    public const double OPACITY_OFF = 0.2;
    #endregion


    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
      Button btn = (Button)values[1];
      string btnCmd = btn.CommandParameter.ToString();
      int noteID = System.Convert.ToInt32(btnCmd.Split(':')[0]);
      int row = System.Convert.ToInt32(btnCmd.Split(':')[1]);
      int col = System.Convert.ToInt32(btnCmd.Split(':')[2]);

      if (WpfApplication1.Model.ChartModel.Notes[noteID] && WpfApplication1.Model.ChartModel.EnableChart[row, col]) {
        return OPACITY_ON;
      }
      else {
        return OPACITY_OFF;
      }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
