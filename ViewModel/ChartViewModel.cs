using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls.Primitives;

/*
References: 
1.) https://www.tutorialspoint.com/mvvm/mvvm_view_viewmodel_communication.htm
*/

namespace WpfApplication1.ViewModel{

  public class ChartViewModel{
    private ChartModel _chart;

    public ChartModel Chart {
      get { return _chart;}
      set { _chart = value; }
    }
    
    public ChartViewModel() {
      _chart = new ChartModel();
      ToggleCommand = new MyICommand(OnToggle);
    }

    public MyICommand ToggleCommand { get; set; }

    public void OnToggle(object obj) {
      Chart.Toggle_Note(obj.ToString());
    }
  }
}
