using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.Reflection;
using Microsoft.Win32;
using Microsoft.Windows.Controls.Ribbon;
using Excel = Microsoft.Office.Interop.Excel ;
using SmartTaskChain.Model;
using System.AddIn.Hosting;

namespace SmartTaskChain.UI_Resources
{
    /// <summary>
    /// WinHistory.xaml 的交互逻辑
    /// </summary>
    public partial class WinHistory :RibbonWindow
    {
        MainDataSet mainDataSet;
        IList<AddInToken> tokens;
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
            AddInsInit();
            OnArchiveDataUpdate(null, null);
        }

        private void OnArchiveDataUpdate(object sender, MainDataSet.ArchiveDataUpdateEvenArgs e)
        {
            CompleteTasks = mainDataSet.CompletedTasks;
            HistoryTaskGrid.ItemsSource = CompleteTasks;
            ComboxSourceUpdate();
            On_HistoryTaskGrid_SourceUpdated();
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
            if(e.AddedItems.Count == 0)
            {
                return;
            }
            condition.Submitter = e.AddedItems[0].ToString();
            HistoryTaskGrid.ItemsSource = CompleteTasks.FindAll(condition.MatchRule);
            On_HistoryTaskGrid_SourceUpdated();
        }

        private void HandlerCombox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TaskFilter condition = BuildFilterCondition();
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            condition.Handler = e.AddedItems[0].ToString();
            HistoryTaskGrid.ItemsSource = CompleteTasks.FindAll(condition.MatchRule);
            On_HistoryTaskGrid_SourceUpdated();
        }

        private void TypeCombox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TaskFilter condition = BuildFilterCondition();
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            condition.Type = e.AddedItems[0].ToString();
            HistoryTaskGrid.ItemsSource = CompleteTasks.FindAll(condition.MatchRule);
            On_HistoryTaskGrid_SourceUpdated();
        }

        private void QlevelCombox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TaskFilter condition = BuildFilterCondition();
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            condition.QLevel = e.AddedItems[0].ToString();
            HistoryTaskGrid.ItemsSource = CompleteTasks.FindAll(condition.MatchRule);
            On_HistoryTaskGrid_SourceUpdated();
        }

        private void StartDataPicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TaskFilter condition = BuildFilterCondition();
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            DateTime newTime, oldTime;
            newTime = (DateTime)e.AddedItems[0];
            oldTime = condition.StartTime;
            condition.StartTime = new DateTime(newTime.Year,
                                                                                newTime.Month,
                                                                                newTime.Day,
                                                                                oldTime.Hour,
                                                                                oldTime.Minute,
                                                                                oldTime.Second);
            HistoryTaskGrid.ItemsSource = CompleteTasks.FindAll(condition.MatchRule);
            On_HistoryTaskGrid_SourceUpdated();
        }

        private void EndDataPicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TaskFilter condition = BuildFilterCondition();
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            DateTime newTime, oldTime;
            newTime = (DateTime)e.AddedItems[0];
            oldTime = condition.CompletedTime;
            condition.CompletedTime = new DateTime(newTime.Year,
                                                                                newTime.Month,
                                                                                newTime.Day,
                                                                                oldTime.Hour,
                                                                                oldTime.Minute,
                                                                                oldTime.Second);
            HistoryTaskGrid.ItemsSource = CompleteTasks.FindAll(condition.MatchRule);
            On_HistoryTaskGrid_SourceUpdated();
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

        private void On_HistoryTaskGrid_SourceUpdated()
        {
            HostView.KPICaculaterHostView addin = tokens[AddInList.SelectedIndex].Activate<HostView.KPICaculaterHostView>(AddInSecurityLevel.FullTrust);
            List<string> workload = new List<string>();

            foreach (CompletedTask curTask in HistoryTaskGrid.ItemsSource)
            {
                workload.Add(curTask.QLevel);
            }

            KPILabel.Content = addin.CaculateKPI(workload);
            return;
        }

        private void AddInsInit()
        {
            string strAddInPath = Environment.CurrentDirectory;

            AddInStore.Update(strAddInPath);

            tokens = AddInStore.FindAddIns(typeof(HostView.KPICaculaterHostView), strAddInPath);
            AddInList.ItemsSource = tokens;
            if(tokens.Count > 0)
            {
                AddInList.SelectedIndex = 0;
            }
        }


        #endregion

        private void ExportExcelButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savedialog;
            string strPath;

            //初始化对话框，文件类型，过滤器，初始路径等设置
            savedialog = new SaveFileDialog();
            savedialog.Filter = "Excel 97-2003 Workbook (*.xls)|*.xls";
            savedialog.FilterIndex = 0;
            savedialog.RestoreDirectory = true;
            //成功选取文件后，根据文件类型执行读取函数
            if (savedialog.ShowDialog() != true)
            {
                return;
            }
            strPath = savedialog.FileName;

            switch(GetExtension(strPath))
            {
                case ".xls":
                    break;
                default:
                    ShowStatus("Invalid file name.");
                    return;
            }
            if(SaveFile(strPath) == false)
            {
                ShowStatus("Save Failed.");
                return;
            }
            ShowStatus("Save Success.");
        }

        private bool SaveFile(string sPath)
        {
            Excel.Application newFile = new Excel.Application();

            try
            {
                newFile.Visible = false;

                //Create Workbook
                Excel.Workbook excelWB = newFile.Workbooks.Add(Type.Missing);

                //Create Worksheet
                Excel.Worksheet excelWS = (Excel.Worksheet)excelWB.Worksheets[1];

                Excel.Range cells = null;
                cells = excelWS.get_Range("A1", Type.Missing);


                PropertyInfo[] pInfos;
                int i = 0, j = 0;
                pInfos = typeof(CompletedTask).GetProperties();
                foreach (PropertyInfo pInfo in pInfos)
                {
                    cells.get_Offset(i, j).Cells.Value2 = pInfo.Name;
                     j++;
                }
                foreach (object curTask in HistoryTaskGrid.Items)
                {
                    pInfos = curTask.GetType().GetProperties();
                    i++;
                    j = 0;
                    foreach (PropertyInfo pInfo in pInfos)
                    {
                        cells.get_Offset(i, j).Cells.Value2 = pInfo.GetValue(curTask, null).ToString();
                        j++;
                    }
                }

                newFile.ActiveWorkbook.RefreshAll();
                newFile.Workbooks.Application.ActiveWorkbook.RefreshAll();

                excelWB.SaveAs(sPath, Excel.XlSaveAction.xlSaveChanges, Type.Missing, Type.Missing, false, false, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing,Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                excelWB.Close(false, null, null);
            }
            catch (Exception ex)
            {
                MessageBoxResult result = MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                newFile.Quit();
            }
            return true;
        }

        private string GetExtension(string sPath)
        {
            int index = sPath.LastIndexOf(".");
            if (index < 0)
            {
                return "";
            }
            return sPath.Substring(index);
        }

        
    }
}
