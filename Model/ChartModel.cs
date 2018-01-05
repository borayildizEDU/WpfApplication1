﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Globalization;
using System.IO;
using System.Windows.Threading;



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

    #region PrivateFields
    private int activeNoteCount;
    private DispatcherTimer statusTimer = new DispatcherTimer();
    #endregion

    #region PublicStaticFields
    public static bool[,] EnableChart = new bool[ROW_COUNT, COL_COUNT];
    public static bool[] Notes = new bool[NOTE_COUNT];
    public static string[] NoteStr = { "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B" };
    #endregion


    #region Properties
    private ObservableCollection<int> _rows = new ObservableCollection<int>();
    public ObservableCollection<int> Rows { get { return _rows; } set { _rows = value; RaisePropertyChanged("Rows"); } }

    private ObservableCollection<int> _cols = new ObservableCollection<int>();
    public ObservableCollection<int> Cols { get { return _cols; } set { _cols = value; RaisePropertyChanged("Cols"); } }

    private ObservableCollection<string> _notelist = new ObservableCollection<string>();
    public ObservableCollection<string> Notelist { get { return _notelist; } set { _notelist = value; RaisePropertyChanged("Notelist"); } }

    private ObservableCollection<string> _scalelist = new ObservableCollection<string>();
    public ObservableCollection<string> Scalelist { get { return _scalelist; } set { _scalelist = value; RaisePropertyChanged("Scalelist"); } }

    private string _saveScale;
    public string SaveScale{ get { return _saveScale; } set { _saveScale = value;  RaisePropertyChanged("SaveScale"); } }

    private string _info;
    public string Info { get { return _info; } set { _info = value; RaisePropertyChanged("Info"); } }

    private string _status;
    public string Status { get { return _status; } set { _status = value; RaisePropertyChanged("Status"); } }

    private string _loadScale;
    public string LoadScale {
      get { return _loadScale; }
      set {
        _loadScale = value;
        LoadScale_();
        RaisePropertyChanged("LoadScale");
      }
    }


    private int _rootNoteID;
    public int RootNoteID {
      get { return _rootNoteID; }
      set {
        ReorderNotes((value - _rootNoteID));    // reorder selected notes by diff

        _rootNoteID = value;
        RaisePropertyChanged("RootNoteID");        
      }
    }

    private int _selectedRowStart = 1;
    public int SelectedRowStart {
      get {
        return _selectedRowStart;
      }
      set {
        _selectedRowStart = value;
        SetDisplayRange(_selectedRowStart, _selectedRowEnd, _selectedColStart, _selectedColEnd);
      }
    }

    private int _selectedRowEnd = (ROW_COUNT - 1);
    public int SelectedRowEnd {
      get {
        return _selectedRowEnd;
      }
      set {
        _selectedRowEnd = value;
        SetDisplayRange(_selectedRowStart, _selectedRowEnd, _selectedColStart, _selectedColEnd);
      }
    }

    private int _selectedColStart = 1;
    public int SelectedColStart {
      get {
        return _selectedColStart;
      }
      set {
        _selectedColStart = value;
        SetDisplayRange(_selectedRowStart, _selectedRowEnd, _selectedColStart, _selectedColEnd);
      }
    }

    private int _selectedColEnd = (COL_COUNT - 1);
    public int SelectedColEnd {
      get {
        return _selectedColEnd;
      }
      set {
        _selectedColEnd = value;
        SetDisplayRange(_selectedRowStart, _selectedRowEnd, _selectedColStart, _selectedColEnd);
      }
    }

    private bool _updateChart;
    public bool UpdateChart {
      get {
        return _updateChart;
      }
      set {
        _updateChart = value;
        RaisePropertyChanged("UpdateChart");
      }
    }

    #endregion


    public void LoadScaleList() {
      string strPath = "Scales\\";
      DirectoryInfo dir = new DirectoryInfo(strPath);

      _scalelist.Clear();

      foreach (FileInfo fileInfo in dir.GetFiles()) {
        _scalelist.Add(fileInfo.Name);
      }
    }

    // Constructor
    public ChartModel(){
      for(int i = 1; i < ROW_COUNT; i++) {
        _rows.Add(i);
      }

      for (int i = 1; i < COL_COUNT; i++) {
        _cols.Add(i);
      }

      for (int i = 0; i < NOTE_COUNT; i++) {
        _notelist.Add(NoteStr[i]);
      }

      LoadScaleList();

      SetDisplayRangeAll(true);

      // Set status timer
      statusTimer.Interval = TimeSpan.FromSeconds(3);
      statusTimer.Tick += StatusTick;

    }





    // RefreshChart
    public void RefreshChart() {
      UpdateChart = !UpdateChart;
    }

    // ToggleNote
    public void ToggleNote(string str) {
      int id = Convert.ToInt32(str.Split(':')[0]);      // zero based id
      int row = Convert.ToInt32(str.Split(':')[1]);     // one based row(e.g guitar string)
      int col = Convert.ToInt32(str.Split(':')[2]);     // zero based col(e.g guitar fret)

      if (!Notes[id]) {
        if (activeNoteCount == 0) RootNoteID = id;      // assign root
        Notes[id] = true;
        activeNoteCount++;
      }
      else {
        Notes[id] = false;
        activeNoteCount--;

        // root removed, all selected notes will be removed
        if(id == RootNoteID) {
          Array.Clear(Notes, 0, Notes.Length);
          activeNoteCount = 0;
          RootNoteID = 0;
        }
      }

      RefreshChart();
    }

    // Set Display Range
    public void SetDisplayRange(int startRow, int endRow, int startCol, int endCol) {
      for(int i = 0; i < ROW_COUNT; i++) {
        for(int j = 0; j < COL_COUNT; j++) {
          EnableChart[i, j] = (i >= startRow && i <= endRow && j >= startCol && j <= endCol) ? true : false;
        }
      }

      RefreshChart();
    }

    // Set Display Range All
    public void SetDisplayRangeAll(bool state) {
      for (int i = 0; i < ROW_COUNT; i++) {
        for (int j = 0; j < COL_COUNT; j++) {
          EnableChart[i, j] = state;
        }
      }
    }

    // Reorder selected notes
    private void ReorderNotes(int diff) {
      int j;
      bool[] NotesBuf = new bool[NOTE_COUNT];

      Array.Copy(Notes, NotesBuf, Notes.Length);
      Array.Clear(Notes, 0, Notes.Length);

      for (int i = 0; i < NotesBuf.Length; i++) {
        if (NotesBuf[i]) {
          j = (i + diff + NOTE_COUNT) % NOTE_COUNT;
          Notes[j] = true;
        }
      }

      RefreshChart();
    }


    private void DisplayStatus(string str) {
      Status = str;
      statusTimer.Start();    
    }

    private void StatusTick(object sender, EventArgs e) {
      Status = "";
      statusTimer.Stop();
    }



    public void SaveScale_() {
      int j;
      int diff = _rootNoteID;
      string strScale = "";
      string strPath = "Scales\\" + SaveScale;
      ArrayList notesList = new ArrayList();

      // Set scale string
      for (int i = 0; i < Notes.Length; i++) {    // reorder selected notes by taking RootId as zero
        if (Notes[i]) {
          j = (i - diff + NOTE_COUNT) % NOTE_COUNT;
          notesList.Add(j);
        }
      }
      notesList.Sort();

      for(int i = 0; i < notesList.Count; i++) {
        strScale += notesList[i];
        if (i != notesList.Count - 1) strScale += ":";
      }
      

      // WriteFile
      using (StreamWriter writer = new StreamWriter(strPath)) {
        writer.Write(strScale);
      }

      // DisplayStatus
      DisplayStatus(SaveScale + " scale is saved.");
      
    }


    public void LoadScale_() {
      string strPath = "Scales\\" + _loadScale;
      string text;
      int id;

      Array.Clear(Notes, 0, Notes.Length);

      using (StreamReader reader = new StreamReader(strPath)) {
        text = reader.ReadToEnd();
      }

      foreach (string noteID in text.Split(':')) {
        id = Convert.ToInt32(noteID);
        Notes[id] = true;
      }


      ReorderNotes(_rootNoteID);    // reorder selected notes according to rootNoteID

      RefreshChart();

    }

  }
}
