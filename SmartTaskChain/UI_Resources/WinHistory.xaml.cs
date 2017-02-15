using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Windows.Controls.Ribbon;
using SmartTaskChain.Model;

namespace SmartTaskChain.UI_Resources
{
    /// <summary>
    /// WinHistory.xaml 的交互逻辑
    /// </summary>
    public partial class WinHistory :RibbonWindow
    {
        MainDataSet mainDataSet;
        DispatcherTimer StatusUpadteTimer;
        List<CompletedTask> CompleteTasks;
        List<string> SubmitterList;
        List<string> HandlerList;
        List<string> TypeList;
        List<string> QlevelList;

        public WinHistory(MainDataSet dataset)
        {
            InitializeComponent();
            mainDataSet = dataset;
            //订阅mainDataSet的归档数据更新消息
            mainDataSet.ArchiveDataUpdated += OnArchiveDataUpdate;
        }

        private void winHistory_Loaded(object sender, RoutedEventArgs e)
        {
            StatusUpdateTimer_Init();
            OnArchiveDataUpdate(null, null);
        }

        private void OnArchiveDataUpdate(object sender, MainDataSet.ArchiveDataUpdateEvenArgs e)
        {
            CompleteTasks = mainDataSet.CompletedTasks;
            HistoryTaskGrid.ItemsSource = CompleteTasks;
            ComboxSourceUpdate();
        }

        private void ComboxSourceUpdate()
        {
            SubmitterList = new List<string>();
            HandlerList = new List<string>();
            TypeList = new List<string>();
            QlevelList = new List<string>();

            foreach (CompletedTask curTask in CompleteTasks)
            {
                if(SubmitterList.IndexOf(curTask.Submitter) == -1)
                {
                    SubmitterList.Add(curTask.Submitter);
                }
                foreach(string sName in curTask.GetHandlerList())
                {
                    if(HandlerList.IndexOf(sName) == -1)
                    {
                        HandlerList.Add(sName);
                    }
                }
                if(TypeList.IndexOf(curTask.Type) == -1)
                {
                    TypeList.Add(curTask.Type);
                }
                if(QlevelList.IndexOf(curTask.QLevel) == -1)
                {
                    QlevelList.Add(curTask.QLevel);
                }
            }
            SubmitterList.Sort();
            SubmitterList.Insert(0, "");
            SubmitterCombox.ItemsSource = SubmitterList;
            HandlerList.Sort();
            HandlerList.Insert(0, "");
            HandlerCombox.ItemsSource = HandlerList;
            TypeList.Sort();
            TypeList.Insert(0, "");
            TypeCombox.ItemsSource = TypeList;
            QlevelList.Sort();
            QlevelList.Insert(0, "");
            QlevelCombox.ItemsSource = QlevelList;
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

        #region ConditionUpdate
        private void SubmitterCombox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TaskFilter condition = BuildFilterCondition();
            CompleteTasks.FindAll(condition.MatchRule);
        }

        private void HandlerCombox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TaskFilter condition = BuildFilterCondition();
            CompleteTasks.FindAll(condition.MatchRule);
        }

        private void TypeCombox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TaskFilter condition = BuildFilterCondition();
            CompleteTasks.FindAll(condition.MatchRule);
        }

        private void QlevelCombox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TaskFilter condition = BuildFilterCondition();
            CompleteTasks.FindAll(condition.MatchRule);
        }

        private void StartDataPicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TaskFilter condition = BuildFilterCondition();
            CompleteTasks.FindAll(condition.MatchRule);
        }

        private void EndDataPicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TaskFilter condition = BuildFilterCondition();
            CompleteTasks.FindAll(condition.MatchRule);
        }

        private TaskFilter BuildFilterCondition()
        {
            DateTime start, end;

            start = new DateTime(0);
            if(StartDataPicker.SelectedDate != null)
            {
                start = new DateTime(StartDataPicker.SelectedDate.Value.Year,
                                                    StartDataPicker.SelectedDate.Value.Month,
                                                    StartDataPicker.SelectedDate.Value.Day,
                                                    9,
                                                    0,
                                                    0);
            }
            end = DateTime.Now;
            if(EndDataPicker.SelectedDate != null)
            {
                end = new DateTime(EndDataPicker.SelectedDate.Value.Year,
                                                    EndDataPicker.SelectedDate.Value.Month,
                                                    EndDataPicker.SelectedDate.Value.Day,
                                                    17,
                                                    0,
                                                    0);
            }
            TaskFilter condition = new TaskFilter(SubmitterCombox.Text,
                                                                        HandlerCombox.Text,
                                                                        TypeCombox.Text,
                                                                        QlevelCombox.Text,
                                                                        start,
                                                                        end);
            return condition;
        }

        #endregion
    }
}
