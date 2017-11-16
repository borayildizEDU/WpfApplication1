using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WpfApplication1.Model{
  public class ChartModel { }

  public class Chart : INotifyPropertyChanged {
    private string note;

    public string Note {
      get { return note; }

      set {
        if (note != value) {
          note = value;
          RaisePropertyChanged("Note");
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string property) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
      }
    }
  }
}
