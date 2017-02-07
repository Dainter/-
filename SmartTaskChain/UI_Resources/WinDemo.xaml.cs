using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Windows.Controls.Ribbon;
using SmartTaskChain.Model;
using SmartTaskChain.Business;

namespace SmartTaskChain.UI_Resources
{
    /// <summary>
    /// WinDemo.xaml 的交互逻辑
    /// </summary>
    public partial class WinDemo
    {
        MainDataSet mainDataSet;

        List<IfTask> ClareTasks;
        List<IfTask> DouglasTasks;
        List<IfTask> EulerTasks;
        List<IfTask> FrankTasks;
        List<IfTask> GloriaTasks;

        public WinDemo(MainDataSet dataset)
        {
            InitializeComponent();
            mainDataSet = dataset;
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ClareTasks = new List<IfTask>();
            ControlInit();
            ClareListBox.ItemsSource = ClareTasks;
        }

        private void ControlInit()
        {
            ClareListBox.ItemsSource = ClareTasks;
            DouglasListBox.ItemsSource = DouglasTasks;
            EulerListBox.ItemsSource = EulerTasks;
            FrankListBox.ItemsSource = FrankTasks;
            GloriaListBox.ItemsSource = GloriaTasks;
        }

    }
}
