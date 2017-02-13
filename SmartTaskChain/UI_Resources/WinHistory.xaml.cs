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

        
    }
}
