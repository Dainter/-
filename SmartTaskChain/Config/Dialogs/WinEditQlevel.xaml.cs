using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using SmartTaskChain.Model;

namespace SmartTaskChain.Config.Dialogs
{
    /// <summary>
    /// WinEditQlevel.xaml 的交互逻辑
    /// </summary>
    public partial class WinEditQlevel : Window
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
        string strDescription;
        int intPriority;

        public WinEditQlevel(MainDataSet DataSet)
        {
            InitializeComponent();
            mainDataSet = DataSet;
        }

        private void WinEditQlevel_Loaded(object sender, RoutedEventArgs e)
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
            NameComboBox.ItemsSource = mainDataSet.QLevels;
        }

        private void WinEditQlevel_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
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
            EditQlevel();
            mainDataSet.UpdateRuntimeDataSet();
            this.DialogResult = true;
            this.Close();
        }

        private bool InputVarification()
        {
            //优先级
            if (PriorityTextBox.Text == "")
            {
                InputWarning.PlacementTarget = PriorityTextBox;
                WarningInfo.Text = "Please select a submitter for the task.";
                InputWarning.IsOpen = true;
                return false;
            }
            if (int.TryParse(PriorityTextBox.Text, out intPriority) == false)
            {
                InputWarning.PlacementTarget = PriorityTextBox;
                WarningInfo.Text = "Please input a number between 0 and 100.";
                InputWarning.IsOpen = true;
                return false;
            }
            if (intPriority > 100 || intPriority < 0)
            {
                InputWarning.PlacementTarget = PriorityTextBox;
                WarningInfo.Text = "Please input a number between 0 and 100.";
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

        private void EditQlevel()
        {
            string strName;
            QLevel curQLevel;

            strName = NameComboBox.Text;
            curQLevel = mainDataSet.GetQlevelItem(strName);
            if (curQLevel == null)
            {
                InputWarning.PlacementTarget = NameComboBox;
                WarningInfo.Text = "Selected Type is not exists in DB.";
                InputWarning.IsOpen = true;
                return;
            }
            curQLevel.Priority = intPriority;
            curQLevel.Description = strDescription;
            return;
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                ClearInfo();
                return;
            }
            if (e.AddedItems[0].ToString() == "")
            {
                return;
            }
            LoadInfo(e.AddedItems[0].ToString());
        }

        private void ClearInfo()
        {
            PriorityTextBox.Clear();
            DescriptionBox.Clear();
        }

        private void LoadInfo(string sName)
        {
            QLevel curItem = mainDataSet.GetQlevelItem(sName);
            if(curItem == null)
            {
                return;
            }
            PriorityTextBox.Text = curItem.Priority.ToString();
            DescriptionBox.Text = curItem.Description;
        }
    }
}
