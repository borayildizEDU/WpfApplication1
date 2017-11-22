using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WpfApplication1.Model{

  public class Note : INotifyPropertyChanged {
    private string text;
    public string Text { get { return text; } set { text = value; } }

    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string property) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
      }
    }


  }


  public class ChartModel : INotifyPropertyChanged {
    private ObservableCollection<Note> notes = new ObservableCollection<Note>();
    public ObservableCollection<Note> Notes { get { return notes; } set { notes = value; } }


    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string property) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
      }
    }

    public void DisplayNotes() {
      for(int i = 0; i < Notes.Count; i++) {
        RaisePropertyChanged(Notes.ElementAt(i).Text);
      }
    }

    public void ToggleNote(string str) {
      string note = str.Split(':')[0];
      int row = Convert.ToInt32(str.Split(':')[1]);
      int col = Convert.ToInt32(str.Split(':')[2]);

      Notes.Add(new Note() { Text = note });
      DisplayNotes();
    }

  }


}
