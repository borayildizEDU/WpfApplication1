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
    private string text;
    public string Text { get { return text; } set { text = value; RaisePropertyChanged(text); } }


    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string property) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
      }
    }


  }


  public class ChartModel : INotifyPropertyChanged {
    private Note note_C = new Note();
    public  Note Note_C { get { return note_C; } set { note_C = value; RaisePropertyChanged("Note_C"); } }
    private Note note_D = new Note();
    public  Note Note_D { get { return note_D; } set { note_D = value; RaisePropertyChanged("Note_D"); } }
    private Note note_E = new Note();
    public  Note Note_E { get { return note_E; } set { note_E = value; RaisePropertyChanged("Note_E"); } }
    private Note note_F = new Note();
    public  Note Note_F { get { return note_F; } set { note_F = value; RaisePropertyChanged("Note_F"); } }
    private Note note_G = new Note();
    public  Note Note_G { get { return note_G; } set { note_G = value; RaisePropertyChanged("Note_G"); } }
    private Note note_A = new Note();
    public  Note Note_A { get { return note_A; } set { note_A = value; RaisePropertyChanged("Note_A"); } }
    private Note note_B = new Note();
    public  Note Note_B { get { return note_B; } set { note_B = value; RaisePropertyChanged("Note_B"); } }



    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string property) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
      }
    }

    public void ToggleNote(string str) {
      Note note = new Note();
      note.Enable = true;
      note.Text = str.Split(':')[0];

      Note_A = note;
    }

  }


}
