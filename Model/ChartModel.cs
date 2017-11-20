using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WpfApplication1.Model{

  public class ChartModel : INotifyPropertyChanged {
    private bool note_E;
    public bool Note_E { get { return note_E; } set { note_E = value; RaisePropertyChanged("Note_E"); } } 

    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string property) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
      }
    }

    public void ToggleNote(string str) {
      note_E = true;
    }

  }


}
