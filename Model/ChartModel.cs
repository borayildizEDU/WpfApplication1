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
using System.Xml;



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
    private string activeScaleNotes;
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

    private ObservableCollection<string> _typelist = new ObservableCollection<string>();
    public ObservableCollection<string> Typelist { get { return _typelist; } set { _typelist = value; RaisePropertyChanged("Typelist"); } }

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

    private string _scaleType;
    public string ScaleType {
      get { return _scaleType; }
      set {
        _scaleType = value;
        LoadScaleList(_scaleType);
        RaisePropertyChanged("ScaleType");
      }
    }


    private int _rootNoteID;
    public int RootNoteID {
      get { return _rootNoteID; }
      set {
        ReorderNotes((value - _rootNoteID), Notes);    // reorder selected notes by diff        
        _rootNoteID = value;
        RaisePropertyChanged("RootNoteID");
        RefreshChart();
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



    public void LoadTypeList() {
      string strPath = "Scales\\Scales.xml";
      string type;

      _typelist.Clear();
      _typelist.Add("Any");

      using (XmlReader reader = XmlReader.Create(strPath)) {
        while (reader.ReadToFollowing("scale")) {
          reader.MoveToFirstAttribute();
          type = reader.Value;
          if (!_typelist.Contains(type)) {
            _typelist.Add(type);
          }
        }
      }
    }


    public void LoadScaleList(string type) {
      string strPath = "Scales\\Scales.xml";
      string name, type_;

      _scalelist.Clear();

      using (XmlReader reader = XmlReader.Create(strPath)) {
        while (reader.ReadToFollowing("scale")) {
          reader.MoveToFirstAttribute();
          type_ = reader.Value;

          if(type == "Any") {
            reader.ReadToFollowing("name");
            name = reader.ReadElementContentAsString();
            _scalelist.Add(type_ + ":" + name);
          }
          else if(type_ == type) {
            reader.ReadToFollowing("name");
            name = reader.ReadElementContentAsString();
            _scalelist.Add(name);
          }

        }
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
      
      LoadTypeList();
      _scaleType = "Any";
      LoadScaleList(_scaleType);

      SetDisplayRangeAll(true);

      // Set status timer
      statusTimer.Interval = TimeSpan.FromSeconds(3);
      statusTimer.Tick += StatusTick;

    }


    // RefreshChart
    public void RefreshChart() {
      UpdateChart = !UpdateChart;
      UpdateInfo();   // Info field is binded to the chart 
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
    private void ReorderNotes(int diff, bool[] notes) {
      int j;
      bool[] NotesBuf = new bool[NOTE_COUNT];

      Array.Copy(notes, NotesBuf, notes.Length);
      Array.Clear(notes, 0, notes.Length);

      for (int i = 0; i < NotesBuf.Length; i++) {
        if (NotesBuf[i]) {
          j = (i + diff + NOTE_COUNT) % NOTE_COUNT;
          notes[j] = true;
        }
      }
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
      string strPath = "Scales\\Scales";
      ArrayList notesList = new ArrayList();

      // Set scale string
      for (int i = 0; i < Notes.Length; i++) {    // reorder selected notes by taking RootId as zero
        if (Notes[i]) {
          j = (i - diff + NOTE_COUNT) % NOTE_COUNT;
          notesList.Add(j);
        }
      }
      notesList.Sort();

      strScale += "User:" + _saveScale + ":BothWay:";
      for (int i = 0; i < notesList.Count; i++) {
        strScale += notesList[i];
        if (i != notesList.Count - 1) strScale += ",";
      }
      strScale += Environment.NewLine;

      // WriteFile
      using (StreamWriter writer = new StreamWriter(strPath, append:true)) {
        writer.Write(strScale);
      }

      // DisplayStatus
      DisplayStatus(SaveScale + " scale is saved.");
      
    }


    private string GetChords(string notes) {
      string chords = "";
      string[] suffix = new string[NOTE_COUNT];
      ArrayList noteList = new ArrayList();
      int firstNote, thirdNote, fifthNote;
      int diff1, diff2;
      int noteId;

      if (notes == null) return null;

      foreach (string noteID in notes.Split(',')) {
        noteList.Add(Convert.ToInt32(noteID));
      }

      for(int i = 0; i < noteList.Count; i++) {
        firstNote = (int)noteList[i];
        thirdNote = (int)noteList[((i + 2 + noteList.Count) % noteList.Count)];
        fifthNote = (int)noteList[((i + 4 + noteList.Count) % noteList.Count)];

        diff1 = (thirdNote - firstNote + NOTE_COUNT) % NOTE_COUNT;
        diff2 = (fifthNote - thirdNote + NOTE_COUNT) % NOTE_COUNT;

        if (diff1 == 4) {
          // major, no additional suffix    
        }
        else if (diff1 == 3) {
          suffix[i] = "m";    // minor
          if(diff2 == 4) {
            // no additional suffix
          }
          else if(diff2 == 3) {
            suffix[i] += "b5";
          }
          else {
            suffix[i] += "?";
          }
        }
        else {
          suffix[i] = "?";
        }

        noteId = ((int)noteList[i] + NOTE_COUNT) % NOTE_COUNT;
        chords += (NoteStr[noteId] + suffix[i]);
        if (i != noteList.Count - 1) chords += " ,";
      }

      return chords;
    }


    // Get active notes string in root C
    private string Get_ActiveNotes_String(int in_root) {
      string active_notes = null;
      bool[] cpy_notes = new bool[NOTE_COUNT];
      int count = 0;
      int diff = in_root - _rootNoteID;

      // reordering back according to root
      Array.Copy(Notes, cpy_notes, NOTE_COUNT);
      if(diff != 0)
        ReorderNotes(diff, cpy_notes);

      for(int i = 0; i < NOTE_COUNT; i++) {
        if (cpy_notes[i]) {
          if (count > 0) active_notes += ",";
          active_notes += i;
          count++;
        }
      }

      return active_notes;
    }


    private void UpdateInfo() {
      string strPath = "Scales\\Scales.xml";
      string type, name, direction, notes;
      string equalScales = "", childScales = "", parentScales = "", nearScalesDegreeOne = "", nearScalesDegreeTwo = "";
      string chords;
      int equalScale_count = 0;
      string active_notes = Get_ActiveNotes_String(0);    // get active notes string according to 0 root

      Info = "";

      // Check for equal scales
      using (XmlReader reader = XmlReader.Create(strPath)) {
        while (reader.ReadToFollowing("scale")) {
          reader.MoveToFirstAttribute();
          type = reader.Value;
          reader.ReadToFollowing("name");
          name = reader.ReadElementContentAsString();
          reader.ReadToFollowing("direction");
          direction = reader.ReadElementContentAsString();
          reader.ReadToFollowing("notes");
          notes = reader.ReadElementContentAsString();

          if (notes == active_notes) {
            if (equalScale_count > 0) equalScales += ", ";

            if(equalScale_count == 1) {   // set combobox
              _loadScale = (type + ":" + name);
              RaisePropertyChanged("LoadScale");
            }

            equalScales += (type + ":" + name);
            equalScale_count++;
          }
        }
      }

      if (equalScale_count == 0) {
        _loadScale = "";
        RaisePropertyChanged("LoadScale");
      }

      // Get chords
      active_notes = Get_ActiveNotes_String(_rootNoteID);   // get active notes string according to selected root
      chords = GetChords(active_notes);

      Info = "Equal Scales-> " + equalScales + Environment.NewLine;
      Info += "Chords-> " + chords + Environment.NewLine;


    }


    public void LoadScale_() {
      string strPath = "Scales\\Scales.xml";
      string type, name;
      string load_type, load_name;
      int id;

      activeScaleNotes = "";

      if (_loadScale == null) return;

      if(_scaleType == "Any") {
        load_type = _loadScale.Split(':')[0];
        load_name = _loadScale.Split(':')[1];
      }
      else {
        load_type = _scaleType;
        load_name = _loadScale;
      }

      Array.Clear(Notes, 0, Notes.Length);

      using (XmlReader reader = XmlReader.Create(strPath)) {
        while (reader.ReadToFollowing("scale")) {
          reader.MoveToFirstAttribute();
          type = reader.Value;

          if(type == load_type) {
            reader.ReadToFollowing("name");
            name = reader.ReadElementContentAsString();
            if (name == load_name) {
              reader.ReadToFollowing("notes");
              activeScaleNotes = reader.ReadElementContentAsString();

              foreach (string noteID in activeScaleNotes.Split(',')) {
                id = Convert.ToInt32(noteID);
                Notes[id] = true;
              }
              ReorderNotes(_rootNoteID, Notes);    // reorder selected notes according to rootNoteID
              RefreshChart();
              break;
            }
          }

        }
      }
    }

  }
}
