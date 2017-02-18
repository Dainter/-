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
    /// WinEditProcedure.xaml 的交互逻辑
    /// </summary>
    public partial class WinEditProcedure : Window
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
        List<ProcedureStep> newSteps;

        public WinEditProcedure(MainDataSet DataSet)
        {
            InitializeComponent();
            mainDataSet = DataSet;
        }

        private void winEditProcedure_Loaded(object sender, RoutedEventArgs e)
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
            NameComboBox.ItemsSource = mainDataSet.Procedures;
            TypeComboBox.ItemsSource = mainDataSet.CustomTypes;
        }

        private void winEditProcedure_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            //合法性校验
            //if (InputVarification() == false)
            //{
            //    return;
            //}
            //数据组织

            //数据插入数据表
            //SaveTask();
            this.DialogResult = true;
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
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
            TypeComboBox.Text = "";
            StepsGird.ItemsSource = null;
            DescriptionBox.Clear();
        }

        private void LoadInfo(string sName)
        {
            Procedure curItem = mainDataSet.GetProcedureItem(sName);
            if (curItem == null)
            {
                return;
            }
            if (curItem.IsBindingType == true)
            {
                TypeComboBox.Text = curItem.BindingType.Name;
            }
            StepsGird.ItemsSource = curItem.ProcedureSteps;
            DescriptionBox.Text = curItem.Description;
        }

        private void IsCreateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ClearInfo();
        }

        private void IsCreateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            NameComboBox.Text = "";
            ClearInfo();
        }

        private void AddStepButton_Click(object sender, RoutedEventArgs e)
        {
            if (newSteps == null)
            {
                newSteps = new List<ProcedureStep>();
            }
            WinEditStep winEditStep = new WinEditStep(mainDataSet);
            winEditStep.Owner = this;
            if (winEditStep.ShowDialog() == false)
            {
                return;
            }
        }

        private void EditStepButton_Click(object sender, RoutedEventArgs e)
        {
            WinEditStep winEditStep;
            ProcedureStep curStep;
            if (StepsGird.SelectedIndex == -1)
            {
                return;
            }
            if(IsCreateCheckBox.IsChecked == true)
            {
                curStep = newSteps[StepsGird.SelectedIndex];
            }
            else
            {
                Procedure curProcedure = mainDataSet.GetProcedureItem(NameComboBox.Text);
                if(curProcedure == null)
                {
                    InputWarning.PlacementTarget = NewNameBox;
                    WarningInfo.Text = "Current Procedure is not exits in DB.";
                    InputWarning.IsOpen = true;
                    return;
                }
                curStep = curProcedure.ProcedureSteps[StepsGird.SelectedIndex];
            }

            winEditStep = new WinEditStep(mainDataSet, curStep.Name);
            winEditStep.Owner = this;
            if (winEditStep.ShowDialog() == false)
            {
                return;
            }
        }
    }
}
