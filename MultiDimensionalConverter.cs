using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace WpfApplication1 {
  /*
  reference: https://stackoverflow.com/questions/16119200/wpf-binding-to-multidimensional-array-in-the-xaml
  */
  public class MultiDimensionalConverter : IMultiValueConverter {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
      return (values[0] as bool[,,])[(int)values[1], (int)values[2], (int)values[3]];
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
