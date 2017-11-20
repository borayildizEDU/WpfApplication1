using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WpfApplication1.Model{

  public class ChartModel : INotifyPropertyChanged {
    private string selectedNotesText;
    private bool[] selectedNotes = new bool[12];

    public string SelectedNotesText {
      get { return selectedNotesText; }
      set {
        selectedNotesText = value;
        RaisePropertyChanged("SelectedNotesText");
      }
    }

    public bool[] SelectedNotes {
      get { return selectedNotes; }
      set {
        selectedNotes = value;
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
      SelectedNotes[0] = true;
      RaisePropertyChanged("SelectedNotes");
    }

  }


}
