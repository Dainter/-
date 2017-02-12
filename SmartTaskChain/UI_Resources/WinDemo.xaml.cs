using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Controls;
using Microsoft.Windows.Controls.Ribbon;
using SmartTaskChain.Model;
using SmartTaskChain.Business;
using SmartTaskChain.Config.Dialogs;

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
            mainDataSet.RuntimeDataUpdated += OnDataUpdate;
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StatusUpdateTimer_Init();
            OnDataUpdate(null, null);
        }

        private void OnDataUpdate(object sender, MainDataSet.RuntimeDataUpdateEvenArgs e)
        {
            DispatchTasks = mainDataSet.GetTaskListByStatus("Wait");
            AliceTasks = mainDataSet.GetTaskListByHandler("Alice");
            BobTasks = mainDataSet.GetTaskListByHandler("Bob");
            ClareTasks = mainDataSet.GetTaskListByHandler("Clare");
            DouglasTasks = mainDataSet.GetTaskListByHandler("Douglas");
            EulerTasks = mainDataSet.GetTaskListByHandler("Euler");
            FrankTasks = mainDataSet.GetTaskListByHandler("Frank");
            GloriaTasks = mainDataSet.GetTaskListByHandler("Gloria");
            TaskListSort();
            ControlInit();
        }

        private void TaskListSort()
        {
            DispatchTasks.Sort();
            AliceTasks.Sort();
            BobTasks.Sort();
            ClareTasks.Sort();
            DouglasTasks.Sort();
            EulerTasks.Sort();
            FrankTasks.Sort();
            GloriaTasks.Sort();
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

        #region Dispatcher
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void TaskDispatch()
        {
            ProcedureStep nextStep;
            UserGroup curGroup;

            foreach (ProcedureTask curTask in DispatchTasks)
            {
                //物勒工名
                string strLog;
                if (curTask.Handler == null)
                {
                    strLog = "\nStep: " + curTask.CurrentStep.Name + " Handler: null";
                }
                else
                {
                    strLog = "\nStep: " + curTask.CurrentStep.Name + " Handler: " + curTask.Handler.Name;
                    //从原handler任务列表中删除
                    curTask.Handler.HandleTasks.Remove(curTask);
                }
                curTask.Description += strLog;
                //获取下一步Handler
                nextStep = curTask.CurrentStep.NextStep;
                if (nextStep == null)
                {
                    //完成，存入归档数据库
                    mainDataSet.ArchiveTask(curTask);
                    mainDataSet.UpdateRuntimeDataSet();
                    mainDataSet.UpdateArchiveDataSet();
                }
                curTask.CurrentStep = nextStep;
                //获取负责人组（单向）
                curGroup = nextStep.HandleRole;
                //智能分配负责人(双向)
                curTask.Handler = GetHandler(curGroup);
                curTask.Handler.HandleTasks.Add(curTask);
                curTask.Status = "Process";
            }
            mainDataSet.UpdateRuntimeDataSet();
        }


        private IfUser GetHandler(UserGroup curGroup)
        {
            if(curGroup.Users.Count == 0)
            {
                return null;
            }
            if(curGroup.Users.Count == 1)
            {
                return curGroup.Users[0];
            }
            int intMin = 10000000, intIndex = 0,intCount, intWork;
            intCount = 0;
            foreach(IfUser curUser in curGroup.Users)
            {
                intWork = curUser.GetTotalWork();
                if(intWork < intMin)
                {
                    intMin = intWork;
                    intIndex = intCount;
                }
                intCount++;
            }
            return curGroup.Users[intIndex];
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }
        #endregion

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
            TaskDispatch();


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

        private void AliceSubmitButton_Click(object sender, RoutedEventArgs e)
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
            newTask.UpdateRealtion(curType,
                                                    mainDataSet.GetUserItem("Alice"),
                                                    curType.BindingProcedure.GetFirstStep(),
                                                    mainDataSet.GetQlevelItem("Q1"));
            mainDataSet.InsertProcedureTask(newTask);
            mainDataSet.UpdateRuntimeDataSet();
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
            newTask.UpdateRealtion(curType,
                                                    mainDataSet.GetUserItem("Bob"),
                                                    curType.BindingProcedure.GetFirstStep(),
                                                    mainDataSet.GetQlevelItem("Q2"));
            mainDataSet.InsertProcedureTask(newTask);
            mainDataSet.UpdateRuntimeDataSet();
        }
        //提交Custom任务
        private void GloriaSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string strSubmitter = "Gloria";
            Manager mg = (Manager)mainDataSet.GetUserItem(strSubmitter);
            foreach (IfUser staff in mg.Inferiors)
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
            newTask.UpdateRealtion(workTime,
                                                    Submitter,
                                                    Handler,
                                                    mainDataSet.GetQlevelItem("Q3"));
            mainDataSet.InsertCustomTask(newTask, workTime);
            mainDataSet.UpdateRuntimeDataSet();
        }

        private void AliceOperation_Click(object sender, RoutedEventArgs e)
        {
            WinCreateTask createWindow = new WinCreateTask(this.mainDataSet,"Alice");
            createWindow.Owner = this;
            if (createWindow.ShowDialog() == false)
            {
                return;
            }
        }

        private void BobOperation_Click(object sender, RoutedEventArgs e)
        {
            WinCreateTask createWindow = new WinCreateTask(this.mainDataSet, "Bob");
            createWindow.Owner = this;
            if (createWindow.ShowDialog() == false)
            {
                return;
            }
        }

        private void GloriaOperation_Click(object sender, RoutedEventArgs e)
        {
            WinCreateTask createWindow = new WinCreateTask(this.mainDataSet, "Gloria");
            createWindow.Owner = this;
            if (createWindow.ShowDialog() == false)
            {
                return;
            }
        }

        private void AliceCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(AliceListBox.SelectedIndex == -1)
            {
                return;
            }
            IfTask curTask = AliceTasks[AliceListBox.SelectedIndex];
            CompleteTask(curTask);
            
        }

        private void BobCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (BobListBox.SelectedIndex == -1)
            {
                return;
            }
            IfTask curTask = BobTasks[BobListBox.SelectedIndex];
            CompleteTask(curTask);
        }

        private void ClareCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClareListBox.SelectedIndex == -1)
            {
                return;
            }
            IfTask curTask = ClareTasks[ClareListBox.SelectedIndex];
            CompleteTask(curTask);
        }

        private void DouglasCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (DouglasListBox.SelectedIndex == -1)
            {
                return;
            }
            IfTask curTask = DouglasTasks[DouglasListBox.SelectedIndex];
            CompleteTask(curTask);
        }

        private void EulerCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (EulerListBox.SelectedIndex == -1)
            {
                return;
            }
            IfTask curTask = EulerTasks[EulerListBox.SelectedIndex];
            CompleteTask(curTask);
        }

        private void FrankCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (FrankListBox.SelectedIndex == -1)
            {
                return;
            }
            IfTask curTask = FrankTasks[FrankListBox.SelectedIndex];
            CompleteTask(curTask);
        }

        private void GloriaCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (GloriaListBox.SelectedIndex == -1)
            {
                return;
            }
            IfTask curTask = GloriaTasks[GloriaListBox.SelectedIndex];
            CompleteTask(curTask);
        }

        private void CompleteTask(IfTask curTask)
        {
            if(curTask.IsBindingProcedure == true)
            {
                //Procedure任务设置状态为Wait，等待调度器处理
                curTask.Status = "Wait";
                mainDataSet.UpdateRuntimeDataSet();
                return;
            }
            //Custom任务直接存入归档数据库
            mainDataSet.ArchiveTask(curTask);
            mainDataSet.UpdateRuntimeDataSet();
            mainDataSet.UpdateArchiveDataSet();
        }
        #endregion


    }
}
