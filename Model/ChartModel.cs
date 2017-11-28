using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;

namespace WpfApplication1.Model {



  public class ChartModel : INotifyPropertyChanged {
    public const int NOTE_COUNT = 12;
    public const int ROW_COUNT = 256;
    public const int COL_COUNT = 256;
    private bool[,,] notes = new bool[NOTE_COUNT, ROW_COUNT, COL_COUNT];
    public bool[,,] Notes { get { return notes; } set { notes = value; RaisePropertyChanged("Notes"); } }



    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string property) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
  }
    }



    public void ToggleNote(string str) {
      int id = Convert.ToInt32(str.Split(':')[0]);      // zero based id
      int row = Convert.ToInt32(str.Split(':')[1]);     // one based row(e.g guitar string)
      int col = Convert.ToInt32(str.Split(':')[2]);     // zero based col(e.g guitar fret)

      if (id >= NOTE_COUNT || row > ROW_COUNT || col >= COL_COUNT)
        return;   // TODO: throw exception

      // Toggle Note
      if (!Notes[id, row, col]) {
        Notes[id, row, col] = true;
      }
      else {
        Notes[id, row, col] = false;
      }
      RaisePropertyChanged("Notes");


    }

  }


}
