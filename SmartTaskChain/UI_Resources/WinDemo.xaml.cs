using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Threading;
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
        DispatcherTimer StatusUpadteTimer;
        BackgroundWorker backGroundWorker;
        List<IfTask> DispatchTasks;
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
            //订阅mainDataSet的数据更新消息
            mainDataSet.DataUpdated += OnDataUpdate;
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StatusUpdateTimer_Init();
            mainDataSet.DataUpdateProcedure();
        }

        private void OnDataUpdate(object sender, MainDataSet.DataUpdateEvenArgs e)
        {
            DispatchTasks = mainDataSet.GetTaskListByStatus("Wait");
            AliceTasks = mainDataSet.GetTaskListByHandler("Alice");
            BobTasks = mainDataSet.GetTaskListByHandler("Bob");
            ClareTasks = mainDataSet.GetTaskListByHandler("Clare");
            DouglasTasks = mainDataSet.GetTaskListByHandler("Douglas");
            EulerTasks = mainDataSet.GetTaskListByHandler("Euler");
            FrankTasks = mainDataSet.GetTaskListByHandler("Frank");
            GloriaTasks = mainDataSet.GetTaskListByHandler("Gloria");
            ControlInit();
        }

        private void ControlInit()
        {
            DispatcherListBox.ItemsSource = DispatchTasks;
            AliceListBox.ItemsSource = AliceTasks;
            BobListBox.ItemsSource = BobTasks;
            ClareListBox.ItemsSource = ClareTasks;
            DouglasListBox.ItemsSource = DouglasTasks;
            EulerListBox.ItemsSource = EulerTasks;
            FrankListBox.ItemsSource = FrankTasks;
            GloriaListBox.ItemsSource = GloriaTasks;
        }

        #region StatusTimer
        private void StatusUpdateTimer_Init()
        {
            StatusUpadteTimer = new DispatcherTimer();
            StatusUpadteTimer.Interval = new TimeSpan(0, 0, 3);
            StatusUpadteTimer.Tick += new EventHandler(StatusUpdateTimer_Tick);
            StatusUpadteTimer.IsEnabled = false;
        }

        private void StatusUpdateTimer_Tick(object sender, EventArgs e)
        {
            StatusLabel.Content = "Ready";
            StatusUpadteTimer.IsEnabled = false;
        }

        public void ShowStatus(string sStatus)
        {
            StatusLabel.Content = sStatus;
            StatusUpadteTimer.Start();
        }
        #endregion

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

        private void AlicSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string strName = "维修任务_" +DateTime.Now.ToShortTimeString();
            if (mainDataSet.GetTaskItem(strName) != null)
            {
                ShowStatus("Task: " + strName + " already exists.");
                return;
            }
            DateTime dDate = DateTime.Now + new TimeSpan(14, 0, 0, 0);
            ProcedureTask newTask = new ProcedureTask(strName, DateTime.Now, dDate, strName);
            TaskType curType = mainDataSet.GetTypeItem("维修任务");
            if (curType == null)
            {
                ShowStatus("Task Type: " + curType.Name + " isn't exists.");
                return;
            }
            newTask.BusinessType = curType;
            newTask.Submitter = mainDataSet.GetUserItem("Alice");
            newTask.CurrentStep = curType.BindingProcedure.GetFirstStep();
            newTask.QLevel = mainDataSet.GetQlevelItem("Q1");

            mainDataSet.InsertProcedureTask(newTask);
        }

        private void BobSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string strName = "咨询任务_" + DateTime.Now.ToShortTimeString();
            if (mainDataSet.GetTaskItem(strName) != null)
            {
                ShowStatus("Task: " + strName + " already exists.");
                return;
            }
            DateTime dDate = DateTime.Now + new TimeSpan(3, 0, 0, 0);
            ProcedureTask newTask = new ProcedureTask(strName, DateTime.Now, dDate, strName);
            TaskType curType = mainDataSet.GetTypeItem("咨询任务");
            if (curType == null)
            {
                ShowStatus("Task Type: " + curType.Name + " isn't exists.");
                return;
            }
            newTask.BusinessType = curType;
            newTask.Submitter = mainDataSet.GetUserItem("Bob");
            newTask.CurrentStep = curType.BindingProcedure.GetFirstStep();
            newTask.QLevel = mainDataSet.GetQlevelItem("Q2");

            mainDataSet.InsertProcedureTask(newTask);
        }

        //提交Custom任务
        private void SubmitCTaskCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string strSubmitter = (string)e.Parameter;
            if(strSubmitter != "Gloria")
            {
                return;
            }
            Manager mg = (Manager)mainDataSet.GetUserItem(strSubmitter);
            foreach(IfUser staff in mg.Inferiors)
            {
                SubmitWorkTimeTask(mg, staff);
            }
            return;
        }

        private void SubmitWorkTimeTask(IfUser Submitter,IfUser Handler)
        {
            string strName = "填报工时_" + Handler.Name + "_"+ DateTime.Today.ToShortDateString();
            if (mainDataSet.GetTaskItem(strName) != null)
            {
                ShowStatus("Task: "+ strName+" already exists.");
                return;
            }
            DateTime dDate = DateTime.Now + new TimeSpan(3,0,0,0);
            CustomTask newTask = new CustomTask(strName, DateTime.Now, dDate, strName);
            TaskType workTime = mainDataSet.GetTypeItem("填报工时");
            if (workTime == null)
            {
                workTime = new TaskType("填报工时", 70);
            }
            newTask.BusinessType = workTime;
            newTask.Submitter = Submitter;
            newTask.Handler = Handler;
            newTask.QLevel = mainDataSet.GetQlevelItem("Q3");

            mainDataSet.InsertCustomTask(newTask, workTime);
        }

        private void CompleteTaskCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string strName = (string)e.Parameter;
            return;
        }

        #endregion

        
    }
}
