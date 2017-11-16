using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace WpfApplication1.ViewModel{

  public class ChartViewModel{
    ChartModel ChartModel = new ChartModel();

    // Update handler
    public ObservableCollection<ChartModel> Chart { get; set; }


    // Command handler
    private ICommand m_ButtonCommand;
    public ICommand ButtonCommand {
      get {
        return m_ButtonCommand;
      }
      set {
        m_ButtonCommand = value;
      }
    }

    public ChartViewModel() {
      ButtonCommand = new ClickCommand(new Action<object>(OnClick));
    }

    public void LoadChart() {

    }


    public void OnClick(object obj) {
      //MessageBox.Show(obj.ToString());
      ChartModel.Note += obj.ToString();


    }




  }





}
