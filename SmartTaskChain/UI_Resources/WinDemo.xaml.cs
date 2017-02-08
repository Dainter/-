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
using System.ComponentModel;
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
        BackgroundWorker backGroundWorker;
        List<IfTask> ClareTasks;
        List<IfTask> DouglasTasks;
        List<IfTask> EulerTasks;
        List<IfTask> FrankTasks;
        List<IfTask> GloriaTasks;

        public WinDemo(MainDataSet dataset)
        {
            InitializeComponent();
            mainDataSet = dataset;
            backGroundWorker = (BackgroundWorker)this.FindResource("backGroundWorker");
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ClareTasks = new List<IfTask>();
            ControlInit();
            ClareTasks.Add(new ProcedureTask("a",DateTime.Now,DateTime.Now,"a"));
        }

        private void ControlInit()
        {
            ClareListBox.ItemsSource = ClareTasks;
            DouglasListBox.ItemsSource = DouglasTasks;
            EulerListBox.ItemsSource = EulerTasks;
            FrankListBox.ItemsSource = FrankTasks;
            GloriaListBox.ItemsSource = GloriaTasks;
        }

        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //通过按钮触发
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        #region UI Commands
        //DispatchRun执行使能
        private void RunCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(backGroundWorker == null)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = !backGroundWorker.IsBusy;
        }
        //DispatchRun执行
        private void RunCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }
        //DispatchPause执行使能
        private void PauseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (backGroundWorker == null)
            {
                e.CanExecute = false;
                return;
            }
            e.CanExecute = backGroundWorker.IsBusy;
        }
        //DispatchPause执行
        private void PauseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }
        #endregion
    }
}
