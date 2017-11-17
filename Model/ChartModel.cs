using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WpfApplication1.Model{

  public class ChartModel { }

  public class SelectedNote : INotifyPropertyChanged {
    private string shortName;

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string propertyName) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion


    public string ShortName {
      get { return shortName; }

      set {
        if (shortName != value) {
          shortName = value;
          RaisePropertyChanged("ShortName");
        }
      }
    }

  }
}
