using System;
using System.Collections.Generic;
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
    /// WinEditType.xaml 的交互逻辑
    /// </summary>
    public partial class WinEditType : Window
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
        string strName, strProcedure, strDescription;
        int intPriority;

        public WinEditType(MainDataSet DataSet)
        {
            InitializeComponent();
            mainDataSet = DataSet;
        }

        private void winEditType_Loaded(object sender, RoutedEventArgs e)
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
            NameComboBox.ItemsSource = mainDataSet.TaskTypes;
            ProcedureComboBox.ItemsSource = mainDataSet.NoTypeProcedures;
        }

        private void winEditType_MouseMove(object sender, MouseEventArgs e)
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
            if(IsCreateCheckBox.IsChecked == true)
            {
                CreateType();
            }
            else
            {
                EditType();
            }
            mainDataSet.UpdateRuntimeDataSet();
            this.DialogResult = true;
            this.Close();
        }

        private bool InputVarification()
        {
            const string strExtractPattern = @"[\u4E00-\u9FA5A-Za-z0-9_]+";  //匹配目标"Step:+Handler:"组合
            MatchCollection matches;
            Regex regObj;

            //任务名
            if(IsCreateCheckBox.IsChecked == true)
            {
                strName = NewNameBox.Text;
                if (strName == "")
                {
                    InputWarning.PlacementTarget = NewNameBox;
                    WarningInfo.Text = "Please enter a non-empty value.";
                    InputWarning.IsOpen = true;
                    return false;
                }
                regObj = new Regex(strExtractPattern);//正则表达式初始化，载入匹配模式
                matches = regObj.Matches(strName);//正则表达式对分词结果进行匹配
                if (matches.Count == 0)
                {
                    InputWarning.PlacementTarget = NewNameBox;
                    WarningInfo.Text = "Name field only include Chinese, English, Underline characters.";
                    InputWarning.IsOpen = true;
                    return false;
                }
            }
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
            //绑定流程
            strProcedure = ProcedureComboBox.Text;
            if (strProcedure != "")
            {
                if(mainDataSet.GetProcedureItem(strProcedure) == null)
                {
                    InputWarning.PlacementTarget = PriorityTextBox;
                    WarningInfo.Text = "Selected Procedure is not exists in DB.";
                    InputWarning.IsOpen = true;
                    return false;
                }
                if (mainDataSet.GetProcedureItem(strProcedure).IsBindingType == true)
                {
                    InputWarning.PlacementTarget = PriorityTextBox;
                    WarningInfo.Text = "Selected Procedure already binding a Type.";
                    InputWarning.IsOpen = true;
                    return false;
                }
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

        private void EditType()
        {
            TaskType curType;
            strName = NameComboBox.Text;
            curType = mainDataSet.GetTypeItem(strName);
            if(curType == null)
            {
                InputWarning.PlacementTarget = NewNameBox;
                WarningInfo.Text = "Selected Type is not exists in DB.";
                InputWarning.IsOpen = true;
                return;
            }
            curType.Priority = intPriority;
            if (ProcedureComboBox.Text == "")
            {
                if(curType.BindingProcedure != null)
                {
                    curType.BindingProcedure.BindingType = null;
                }
                curType.BindingProcedure = null;
            }
            else
            {
                curType.BindingProcedure = mainDataSet.GetProcedureItem(strProcedure);
                curType.BindingProcedure.BindingType = curType;
            }
            curType.Description = strDescription;
            return;
        }

        private void CreateType()
        {
            TaskType newType = new TaskType(strName,intPriority, strDescription);
            if (ProcedureComboBox.Text != "")
            {
                newType.BindingProcedure = mainDataSet.GetProcedureItem(strProcedure);
                newType.BindingProcedure.BindingType = newType;
            }
            mainDataSet.InsertNewType(newType);
            return;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void IsCreateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(((CheckBox)sender).IsChecked == false)
            {
                return;
            }
            NameComboBox.Text = "";
            PriorityTextBox.Text = "";
            ProcedureComboBox.Text = "";
            DescriptionBox.Text = "";
        }

        private void NameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearInfo();
            if (e.AddedItems.Count == 0)
            {
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
            ProcedureComboBox.Text = "";
            DescriptionBox.Clear();
        }

        private void LoadInfo(string sName)
        {
            TaskType curItem = mainDataSet.GetTypeItem(sName);
            if (curItem == null)
            {
                return;
            }
            PriorityTextBox.Text = curItem.Priority.ToString();
            if (curItem.IsUseProcedure == true)
            {
                ProcedureComboBox.Text = curItem.BindingProcedure.Name;
            }
            DescriptionBox.Text = curItem.Description;
        }
    }
}
