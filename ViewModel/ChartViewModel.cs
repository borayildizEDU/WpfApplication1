using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

/*
References: 
1.) https://www.tutorialspoint.com/mvvm/mvvm_view_viewmodel_communication.htm
*/

namespace WpfApplication1.ViewModel{

  public class ChartViewModel{
    // Update handler
    ObservableCollection<SelectedNote> SelectedNotes = new ObservableCollection<SelectedNote>();


    // Command handler
    public MyICommand ToggleCommand { get; set; }


    public ChartViewModel() {
      ToggleCommand = new MyICommand(OnToggle);
    }

    public void OnToggle(object obj) {
      SelectedNotes.Add(new SelectedNote { ShortName = obj.ToString() });
    }




  }





}
