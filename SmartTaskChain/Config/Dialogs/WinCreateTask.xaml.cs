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
        string strName,strSubmitter, strHandler, strType, strQlevel;
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
            //数据组织
            
            //数据插入数据表
            SaveTask();
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
            //任务类别
            strSubmitter = SubmitterComboBox.Text;
            if (strType == "")
            {
                InputWarning.PlacementTarget = SubmitterComboBox;
                WarningInfo.Text = "Please select a category for the task.";
                InputWarning.IsOpen = true;
                return false;
            }
            //任务类别
            strHandler = HandlerComboBox.Text;
            if (strType == "")
            {
                InputWarning.PlacementTarget = HandlerComboBox;
                WarningInfo.Text = "Please select the handler for the task.";
                InputWarning.IsOpen = true;
                return false;
            }
            //任务类别
            strType = typeProcedureComboBox.Text;
            if (strType == "")
            {
                InputWarning.PlacementTarget = typeProcedureComboBox;
                WarningInfo.Text = "Please select a type for the task or input a custom type.";
                InputWarning.IsOpen = true;
                return false;
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
            //步骤

            return true;
        }


        private void SaveTask()
        {

        }

        private void InsertTaskItem(string sName, string ssDate, string sdDate, string sCategory, string sQlevel)
        {

        }

        private void InsertTaskStep(string sStepName, string strKey)
        {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
