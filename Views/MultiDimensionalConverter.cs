using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace WpfApplication1.Views {
  /*
  reference: https://stackoverflow.com/questions/16119200/wpf-binding-to-multidimensional-array-in-the-xaml
  */
  public class MultiDimensionalConverter : IMultiValueConverter {
    #region Constants
    public const int ROW_COUNT = 256;
    public const int COL_COUNT = 256;
    #endregion

    static public bool[,] enable = new bool[ROW_COUNT, COL_COUNT];

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
      bool note = (bool)values[0];
      int row = (int)values[1];
      int col = (int)values[2];

      if ((bool)values[0] && enable[row, col]) {
        return 1;
      }
      else {
        return 0.2;
      }
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
