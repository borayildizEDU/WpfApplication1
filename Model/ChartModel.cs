using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WpfApplication1.Model{

  public class ChartModel : INotifyPropertyChanged {
    private string selectedNotesText;

    public string SelectedNotesText {
      get { return selectedNotesText; }
      set {
        selectedNotesText = value;
        RaisePropertyChanged("SelectedNotesText");
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string property) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
      }
    }

    public void ToggleNote(string str) {
      SelectedNotesText += str;
    }

  }


}
