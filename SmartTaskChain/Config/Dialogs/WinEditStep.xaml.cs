using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// WinEditStep.xaml 的交互逻辑
    /// </summary>
    public partial class WinEditStep : Window
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
        string strName, strGroup, strDescription;
        bool bIsFeedback;
        public StepStore newStep;

        public WinEditStep(MainDataSet DataSet)
        {
            InitializeComponent();
            mainDataSet = DataSet;
        }

        private void winEditStep_Loaded(object sender, RoutedEventArgs e)
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
            GroupComboBox.ItemsSource = mainDataSet.UserGroups;
        }

        private void winEditStep_MouseMove(object sender, MouseEventArgs e)
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
            newStep = new StepStore(strName, strGroup, bIsFeedback, strDescription);
            this.DialogResult = true;
            this.Close();
        }

        private bool InputVarification()
        {
            const string strExtractPattern = @"[\u4E00-\u9FA5A-Za-z0-9_]+";  //匹配目标"Step:+Handler:"组合
            MatchCollection matches;
            Regex regObj;

            //用户名
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
            //是否是feedback步骤
            bIsFeedback = (bool)IsFeedbackCheckBox.IsChecked;
            if(bIsFeedback == false)
            {
                strGroup = GroupComboBox.Text;

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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }



    }
}
