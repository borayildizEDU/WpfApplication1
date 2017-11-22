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
    private ChartModel chart;

    public ChartModel Chart {
      get { return chart;}
      set { chart = value; }
    }
    
    public ChartViewModel() {
      chart = new ChartModel();
      ToggleCommand = new MyICommand(OnToggle);
    }

    public MyICommand ToggleCommand { get; set; }

    public void OnToggle(object obj) {
      Chart.ToggleNote(obj.ToString());
    }
  }
}
