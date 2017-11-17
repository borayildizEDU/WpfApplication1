using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WpfApplication1.Model{

  public class ChartModel { }

  public class SelectedNote : INotifyPropertyChanged {
    private string m_ShortName;

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion


    public string ShortName {
      get { return m_ShortName; }

      set {
        if (m_ShortName != value) {
          m_ShortName = value;
          OnPropertyChanged("ShortName");
        }
      }
    }

  }
}
