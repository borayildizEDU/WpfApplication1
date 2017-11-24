using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WpfApplication1.Model{

  // reference: https://nicoschertler.wordpress.com/2014/05/22/binding-to-a-2d-array-in-wpf/
  public class BindableThreeDArray<T> : INotifyPropertyChanged {
    public event PropertyChangedEventHandler PropertyChanged;
    private void Notify(string property) {
      var pc = PropertyChanged;
      if (pc != null)
        pc(this, new PropertyChangedEventArgs(property));
    }

    T[,,] data;

    public T this[int c1, int c2, int c3] {
      get { return data[c1, c2, c3]; }
      set {
        data[c1, c2, c3] = value;
        Notify(Binding.IndexerName);
      }
    }

    public string GetStringIndex(int c1, int c2, int c3) {
      return c1.ToString() + ":" + c2.ToString() + ":" + c3.ToString();
    }

    private void SplitIndex(string index, out int c1, out int c2, out int c3) {
      var parts = index.Split(':');
      if (parts.Length != 3)
        throw new ArgumentException("The provided index is not valid");

      c1 = int.Parse(parts[0]);
      c2 = int.Parse(parts[1]);
      c3 = int.Parse(parts[2]);
    }

    public T this[string index] {
      get {
        int c1, c2, c3;
        SplitIndex(index, out c1, out c2, out c3);
        return data[c1, c2, c3];
      }
      set {
        int c1, c2, c3;
        SplitIndex(index, out c1, out c2, out c3);
        data[c1, c2, c3] = value;
        Notify(Binding.IndexerName);
      }
    }

    public BindableThreeDArray(int size1, int size2, int size3) {
      data = new T[size1, size2, size3];
    }

    public static implicit operator T[,,] (BindableThreeDArray<T> a) {
      return a.data;
    }
  }



  public class ChartModel : INotifyPropertyChanged {
    public const int NOTE_COUNT = 12;
    public const int ROW_COUNT = 6;
    public const int COL_COUNT = 12;
    public BindableThreeDArray<bool>[,,] Notes = new BindableThreeDArray<bool>[NOTE_COUNT, ROW_COUNT, COL_COUNT];



    public event PropertyChangedEventHandler PropertyChanged;

    private void RaisePropertyChanged(string property) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(property));
      }
    }



    public void ToggleNote(string str) {
      int id = Convert.ToInt32(str.Split(':')[0]);
      int row = Convert.ToInt32(str.Split(':')[1]);
      int col = Convert.ToInt32(str.Split(':')[2]);

      if (Notes.) {
        Notes[id,row,col] = true;
      }
      else {
        Notes[id, row, col] = false;
      }

      RaisePropertyChanged("Notes");

    }

  }


}
