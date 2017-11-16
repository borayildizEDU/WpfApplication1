using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WpfApplication1.Model{

  

  public class ChartModel : INotifyPropertyChanged {
    private string m_Note;

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    #endregion


    public string Note {
      get { return m_Note; }

      set {
        if (m_Note != value) {
          m_Note = value;
          OnPropertyChanged("Note");
        }
      }
    }

  }
}
