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
using System.Windows.Interop;
using System.Runtime.InteropServices;
using SmartTaskChain.Model;

namespace SmartTaskChain.Config.Dialogs
{
    /// <summary>
    /// WinCreateTask.xaml 的交互逻辑
    /// </summary>
    public partial class WinCreateTask : Window
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        };

        [DllImport("DwmApi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(
            IntPtr hwnd,
            ref MARGINS pMarInset);
        //Global Elements
        MainDataSet mainDataSet;
        string strName,strSubmitter, strHandler, strType, strQlevel, strDescription;
        DateTime sDate, dDate;
        //Database


        public WinCreateTask(MainDataSet DataSet)
        {
            InitializeComponent();
            mainDataSet = DataSet;
        }

        private void WinCreateTask_Loaded(object sender, RoutedEventArgs e)
        {
            StyleInit();
            DataInit();
        }

        private void StyleInit()
        {
            this.Resources["TransparentForeColor"] = Properties.Settings.Default.ForeColor;
            this.Background = Brushes.Transparent;
            ExtendAeroGlass(this);
        }

        private void ExtendAeroGlass(Window window)
        {
            try
            {
                // 为WPF程序获取窗口句柄
                IntPtr mainWindowPtr = new WindowInteropHelper(window).Handle;
                HwndSource mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);
                mainWindowSrc.CompositionTarget.BackgroundColor = Colors.Transparent;

                // 设置Margins
                MARGINS margins = new MARGINS();

                // 扩展Aero Glass
                margins.cxLeftWidth = -1;
                margins.cxRightWidth = -1;
                margins.cyTopHeight = -1;
                margins.cyBottomHeight = -1;

                int hr = DwmExtendFrameIntoClientArea(mainWindowSrc.Handle, ref margins);
                if (hr < 0)
                {
                    MessageBox.Show("DwmExtendFrameIntoClientArea Failed");
                }
            }
            catch (DllNotFoundException)
            {
                Application.Current.MainWindow.Background = Brushes.White;
            }
        }

        private void DataInit()
        {
            SubmitterComboBox.ItemsSource = mainDataSet.Users;
            HandlerComboBox.ItemsSource = mainDataSet.Users;
            typeProcedureComboBox.ItemsSource = mainDataSet.ProcedureTypes;
            typeCustomComboBox.ItemsSource = mainDataSet.CustomTypes;
            qlevelComboBox.ItemsSource = mainDataSet.QLevels;
        }

        private void WinCreateTask_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            //合法性校验
            if (InputVarification() == false)
            {
                return;
            }
            //数据插入数据表
            if(IsCustomCheckBox.IsChecked == true)
            {
                SaveCustomTask();
            }
            else
            {
                SaveProcedureTask();
            }
            this.Close();
        }

        private bool InputVarification()
        {
            //任务名
            strName = taskNameTextBox.Text;
            if (strName == "")
            {
                InputWarning.PlacementTarget = taskNameTextBox;
                WarningInfo.Text = "Please enter a non-empty value.";
                InputWarning.IsOpen = true;
                return false;
            }
            //起始时间
            sDate = DateTime.Now;
            if (IsNowCheckBox.IsChecked == false)
            {
                if (startDateDatePicker.SelectedDate == null)
                {
                    InputWarning.PlacementTarget = startDateDatePicker;
                    WarningInfo.Text = "Please select a start date for the task.";
                    InputWarning.IsOpen = true;
                    return false;
                }
                sDate = new DateTime(startDateDatePicker.SelectedDate.Value.Year,
                                                    startDateDatePicker.SelectedDate.Value.Month,
                                                    startDateDatePicker.SelectedDate.Value.Day,
                                                    DateTime.Now.Hour,
                                                    DateTime.Now.Minute,
                                                    DateTime.Now.Second);
            }
            //完成期限
            if (deadDateDatePicker.SelectedDate == null)
            {
                InputWarning.PlacementTarget = deadDateDatePicker;
                WarningInfo.Text = "Please select a deadline for the task.";
                InputWarning.IsOpen = true;
                return false;
            }
            dDate = new DateTime(deadDateDatePicker.SelectedDate.Value.Year,
                                                    deadDateDatePicker.SelectedDate.Value.Month,
                                                    deadDateDatePicker.SelectedDate.Value.Day,
                                                    17,
                                                    0,
                                                    0);
            if (dDate.CompareTo(sDate) <= 0)
            {
                InputWarning.PlacementTarget = deadDateDatePicker;
                WarningInfo.Text = "The deadline must later than the start date.";
                InputWarning.IsOpen = true;
                return false;
            }
            //提交人
            strSubmitter = SubmitterComboBox.Text;
            if (strSubmitter == "")
            {
                InputWarning.PlacementTarget = SubmitterComboBox;
                WarningInfo.Text = "Please select a submitter for the task.";
                InputWarning.IsOpen = true;
                return false;
            }
            if(IsCustomCheckBox.IsChecked == true)
            {
                //处理人
                strHandler = HandlerComboBox.Text;
                if (strHandler == "")
                {
                    InputWarning.PlacementTarget = HandlerComboBox;
                    WarningInfo.Text = "Please select a handler for the task.";
                    InputWarning.IsOpen = true;
                    return false;
                }
                //自定义任务类别
                strType = typeCustomComboBox.Text;
                if (strType == "")
                {
                    InputWarning.PlacementTarget = typeCustomComboBox;
                    WarningInfo.Text = "Please input a custom type.";
                    InputWarning.IsOpen = true;
                    return false;
                }
            }
            else
            {
                //流程性任务类别
                strType = typeProcedureComboBox.Text;
                if (strType == "")
                {
                    InputWarning.PlacementTarget = typeProcedureComboBox;
                    WarningInfo.Text = "Please select a type for the task.";
                    InputWarning.IsOpen = true;
                    return false;
                }
            }
            //任务级别
            strQlevel = qlevelComboBox.Text;
            if (strQlevel == "")
            {
                InputWarning.PlacementTarget = qlevelComboBox;
                WarningInfo.Text = "Please select a Q level for the task.";
                InputWarning.IsOpen = true;
                return false;
            }
            //描述
            strDescription = DescriptionBox.Text;
            if (strDescription == "")
            {
                InputWarning.PlacementTarget = DescriptionBox;
                WarningInfo.Text = "Please enter a non-empty value.";
                InputWarning.IsOpen = true;
                return false;
            }
            return true;
        }

        private void SaveProcedureTask()
        {
            ProcedureTask newTask = new ProcedureTask(strName, sDate, dDate, strDescription);
            newTask.BusinessType = mainDataSet.GetTypeItem(strType);
            newTask.Submitter = mainDataSet.GetUserItem(strSubmitter);
            newTask.QLevel = mainDataSet.GetQlevelItem(strQlevel);
            if(newTask.BusinessType.IsUseProcedure == true)
            {
                newTask.CurrentStep = newTask.BusinessType.BindingProcedure.GetFirstStep();
            }
            //流程任务进入等待区，等待后台调度器调度
            newTask.Status = "Wait";
            mainDataSet.InsertProcedureTask(newTask);
        }

        private void SaveCustomTask()
        {
            CustomTask newTask = new CustomTask(strName, sDate, dDate, strDescription);
            TaskType curType = mainDataSet.GetTypeItem(strType);
            if(curType == null)
            {
                curType = new TaskType(strType);
            }
            newTask.BusinessType = curType;
            newTask.Submitter = mainDataSet.GetUserItem(strSubmitter);
            newTask.Handler = mainDataSet.GetUserItem(strHandler);
            newTask.QLevel = mainDataSet.GetQlevelItem(strQlevel);

            mainDataSet.InsertCustomTask(newTask, curType);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
