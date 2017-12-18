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
    public const int ROW_COUNT = 7;   // eg. string
    public const int COL_COUNT = 13;  // eg. fret
    #endregion

    #region Enums
    public enum Note {
      C   = 0,
      Db  = 1,
      D   = 2,
      Eb  = 3,
      E   = 4,
      F   = 5,
      Gb  = 6,
      G   = 7,
      Ab  = 8,
      A   = 9,
      Bb  = 10,
      B   = 11
    }
    #endregion

    private int activeNoteCount;
    private int _rootNoteID;
    public int RootNoteID { get { return _rootNoteID; } set { _rootNoteID = value; RaisePropertyChanged("RootNoteID"); } }

    public static bool[,] EnableChart = new bool[ROW_COUNT, COL_COUNT];
    public static bool[] Notes = new bool[NOTE_COUNT];
    public static string[] NoteStr = { "C", "Db", "D", "Eb", "E", "F", "Gb", "Ab", "A", "Bb", "B"};

    private bool _updateChart;
    private ObservableCollection<int> _rows = new ObservableCollection<int>();
    private ObservableCollection<int> _cols = new ObservableCollection<int>();
    private ObservableCollection<int> _notelist = new ObservableCollection<int>();
    private int _selectedRowStart = 1, _selectedRowEnd = (ROW_COUNT - 1), _selectedColStart = 1, _selectedColEnd = (COL_COUNT - 1);


    public ObservableCollection<int> Rows { get { return _rows; } set { _rows = value; RaisePropertyChanged("Rows"); } }
    public ObservableCollection<int> Cols { get { return _cols; } set { _cols = value; RaisePropertyChanged("Cols"); } }
    public ObservableCollection<int> Notelist { get { return _notelist; } set { _notelist = value; RaisePropertyChanged("Notelist"); } }

    public bool UpdateChart {
      get {
        return _updateChart;
      }
      set {
        _updateChart = value;
        RaisePropertyChanged("UpdateChart");
      }
    }

    public int SelectedRowStart {
      get {
        return _selectedRowStart;
      } set {
        _selectedRowStart = value;
        SetDisplayRange(_selectedRowStart, _selectedRowEnd, _selectedColStart, _selectedColEnd);
      }
    }

    public int SelectedRowEnd { 
      get {
        return _selectedRowEnd;
      }
      set {
        _selectedRowEnd = value;
        SetDisplayRange(_selectedRowStart, _selectedRowEnd, _selectedColStart, _selectedColEnd);
      }
    }

    public int SelectedColStart { 
      get {
        return _selectedColStart;
      }
      set {
        _selectedColStart = value;
        SetDisplayRange(_selectedRowStart, _selectedRowEnd, _selectedColStart, _selectedColEnd);
      }
    }

    public int SelectedColEnd {
      get {
        return _selectedColEnd;
      }
      set {
        _selectedColEnd = value;
        SetDisplayRange(_selectedRowStart, _selectedRowEnd, _selectedColStart, _selectedColEnd);
      }
    }


    public ChartModel(){
      for(int i = 1; i < ROW_COUNT; i++) {
        _rows.Add(i);
      }

      for (int i = 1; i < COL_COUNT; i++) {
        _cols.Add(i);
      }

      for (int i = 0; i < NOTE_COUNT; i++) {
        _notelist.Add(i);
      }

      SetDisplayRangeAll(true);
    }

    public void RefreshChart() {
      UpdateChart = !UpdateChart;
    }

    public void ToggleNote(string str) {
      int id = Convert.ToInt32(str.Split(':')[0]);      // zero based id
      int row = Convert.ToInt32(str.Split(':')[1]);     // one based row(e.g guitar string)
      int col = Convert.ToInt32(str.Split(':')[2]);     // zero based col(e.g guitar fret)
      int nextID;

      if (!Notes[id]) {
        if (activeNoteCount == 0) RootNoteID = id;      // assign root
        Notes[id] = true;
        activeNoteCount++;
      }
      else {
        Notes[id] = false;
        activeNoteCount--;

        // Reassign root
        if(id == RootNoteID) {
          for(int i = 1; i < NOTE_COUNT; i++) {
            nextID = (id + i + NOTE_COUNT) % NOTE_COUNT;
            if (Notes[nextID] == true) {
              RootNoteID = nextID;
              break;
            }

          }
        }
      }
      RefreshChart();
    }


    public void SetDisplayRange(int startRow, int endRow, int startCol, int endCol) {
      for(int i = 0; i < ROW_COUNT; i++) {
        for(int j = 0; j < COL_COUNT; j++) {
          EnableChart[i, j] = (i >= startRow && i <= endRow && j >= startCol && j <= endCol) ? true : false;
        }
      }
      RefreshChart();
    }

    public void SetDisplayRangeAll(bool state) {
      for (int i = 0; i < ROW_COUNT; i++) {
        for (int j = 0; j < COL_COUNT; j++) {
          EnableChart[i, j] = state;
        }
      }
    }

    public void SaveScale() {
      int id;
      string strScale;

      for (int i = RootNoteID; i < NOTE_COUNT; i++) {
        id = (RootNoteID + i + NOTE_COUNT) % NOTE_COUNT;
        if (Notes[id] == true) {
          strScale = id + ":";
        }
      }
    }

    // Load Scale

  }


}
