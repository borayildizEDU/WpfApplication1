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
    #region RaisePropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string property) {
      PropertyChanged(this, new PropertyChangedEventArgs(property));
    }
    #endregion

    #region Constants
    public const int NOTE_COUNT = 12;
    public const int ROW_COUNT = 256;
    public const int COL_COUNT = 256;
    #endregion

    public static bool[,] EnableChart = new bool[ROW_COUNT, COL_COUNT];
    public static bool[] Notes = new bool[NOTE_COUNT];
    private bool _updateChart;
    public bool UpdateChart { get { return _updateChart; } set { _updateChart = value; RaisePropertyChanged("UpdateChart"); } }


    public void ToggleNote(string str) {
      int id = Convert.ToInt32(str.Split(':')[0]);      // zero based id
      int row = Convert.ToInt32(str.Split(':')[1]);     // one based row(e.g guitar string)
      int col = Convert.ToInt32(str.Split(':')[2]);     // zero based col(e.g guitar fret)

      if (!Notes[id]) {
        Notes[id] = true;
        EnableChart[row, col] = true;
      }
      else {
        Notes[id] = false;
        EnableChart[row, col] = false;
      }
      UpdateChart = true;
    }


    public void SetEnableChart(int startRow, int endRow, int startCol, int endCol) {
      for(int i = 0; i < ROW_COUNT; i++) {
        for(int j = 0; j < COL_COUNT; j++) {
          EnableChart[i, j] = (startRow >= i && endRow <= i && startCol >= j && endCol <= j) ? true : false;
        }
      }
      UpdateChart = true;
    }

  }


}
