using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WpfApplication1.Model{

  public class Note : INotifyPropertyChanged {
    private bool enable;
    public bool Enable { get { return enable; } set { enable = value; RaisePropertyChanged("Enable"); } }


    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string property) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
      }
    }


  }


  public class ChartModel : INotifyPropertyChanged {
    private Note note_A = new Note();
    public Note Note_A { get { return note_A; } set { note_A = value; RaisePropertyChanged("Note_A"); } }


    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string property) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
      }
    }

    public void ToggleNote(string str) {
      Note note = new Note();
      note.Enable = true;

      Note_A = note;
    }

  }


}
