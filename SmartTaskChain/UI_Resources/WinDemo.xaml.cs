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
        List<IfTask> AliceTasks;
        List<IfTask> BobTasks;
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
            mainDataSet.DataUpdated += OnDataUpdate;
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void OnDataUpdate(object sender, MainDataSet.DataUpdateEvenArgs e)
        {
            AliceTasks = mainDataSet.GetTaskList("Alice");
            BobTasks = mainDataSet.GetTaskList("Bob");
            ClareTasks = mainDataSet.GetTaskList("Clare");
            DouglasTasks = mainDataSet.GetTaskList("Douglas");
            EulerTasks = mainDataSet.GetTaskList("Euler");
            FrankTasks = mainDataSet.GetTaskList("Frank");
            GloriaTasks = mainDataSet.GetTaskList("Gloria");
            ControlInit();
        }

        private void ControlInit()
        {
            AliceListBox.ItemsSource = AliceTasks;
            BobListBox.ItemsSource = BobTasks;
            ClareListBox.ItemsSource = ClareTasks;
            DouglasListBox.ItemsSource = DouglasTasks;
            EulerListBox.ItemsSource = EulerTasks;
            FrankListBox.ItemsSource = FrankTasks;
            GloriaListBox.ItemsSource = GloriaTasks;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
            
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
            mainDataSet.DataUpdateProcedure();
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
        //提交Procedure任务
        private void SubmitPTaskCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string strSubmitter = (string)e.Parameter;
            return;
        }
        //提交Custom任务
        private void SubmitCTaskCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string strSubmitter = (string)e.Parameter;
            return;
        }

        private void CompleteTaskCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string strName = (string)e.Parameter;
            return;
        }
        #endregion


    }
}
