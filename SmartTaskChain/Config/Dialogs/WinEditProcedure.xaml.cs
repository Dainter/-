using System;
using System.Collections.Generic;
using System.Linq;
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
        string strName, strType, strDescription;
        List<StepStore> newSteps;
        List<ProcedureStep> newProceSteps;

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
            (StepsGird.Columns[1] as DataGridComboBoxColumn).ItemsSource = mainDataSet.UserGroups;
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
            if (InputVarification() == false)
            {
                return;
            }
            //数据插入数据表
            if (IsCreateCheckBox.IsChecked == true)
            {
                CreateProcedure();
            }
            else
            {
                EditProcedure();
            }
            mainDataSet.UpdateRuntimeDataSet();
            this.DialogResult = true;
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private bool InputVarification()
        {
            const string strExtractPattern = @"[\u4E00-\u9FA5A-Za-z0-9_]+";  //匹配目标"Step:+Handler:"组合
            MatchCollection matches;
            Regex regObj;

            //用户名
            if (IsCreateCheckBox.IsChecked == true)
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
            else
            {
                //绑定步骤检查
                int index = 0;
                newProceSteps = new List<ProcedureStep>();
                ProcedureStep newPStep, previousStep = null;
                
                foreach(StepStore newStep in newSteps)
                {
                    newPStep = new ProcedureStep(newStep.Name, index, newStep.IsFeedback, newStep.Description);
                    newPStep.HandleRole = mainDataSet.GetGroupItem(newStep.HandleGroup);
                    newPStep.HandleRole.BindingStep.Add(newPStep);
                    if (previousStep != null)
                    {
                        previousStep.NextStep = newPStep;
                        newPStep.PreviousStep = previousStep;
                    }
                    newProceSteps.Add(newPStep);
                    previousStep = newPStep;
                    index++;
                }
            }
            //任务类型
            strType = TypeComboBox.Text;
            if(strType != null)
            {
                regObj = new Regex(strExtractPattern);//正则表达式初始化，载入匹配模式
                matches = regObj.Matches(strType);//正则表达式对分词结果进行匹配
                if (matches.Count == 0)
                {
                    InputWarning.PlacementTarget = TypeComboBox;
                    WarningInfo.Text = "Type field only include Chinese, English, Underline characters.";
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

        private void CreateProcedure()
        {
            Procedure newProce = new Procedure(strName, strDescription);
            if(strType != "")
            {
                TaskType curType = mainDataSet.GetTypeItem(strType);
                if(curType == null)
                {
                    curType = new TaskType(strType);
                    mainDataSet.InsertNewType(curType);
                }
                newProce.BindingType = curType;
            }
            mainDataSet.InsertNewProcedure(newProce);
            return;
        }

        private void EditProcedure()
        {
            //Name存在性检查
            strName = NameComboBox.Text;
            Procedure curProce = mainDataSet.GetProcedureItem(strName);
            if (curProce == null)
            {
                InputWarning.PlacementTarget = NewNameBox;
                WarningInfo.Text = "Selected User is not exists in DB.";
                InputWarning.IsOpen = true;
                return;
            }
            //Task Type
            if (strType != "")
            {
                TaskType curType = mainDataSet.GetTypeItem(strType);
                if (curType == null)
                {
                    curType = new TaskType(strType);
                    mainDataSet.InsertNewType(curType);
                }
                curProce.BindingType = curType;
                curType.BindingProcedure = curProce;
            }
            //Procedure
            RefreshSteps(curProce);
            //Description
            curProce.Description = strDescription;
            return;
        }

        private void RefreshSteps(Procedure curProce)
        {
            foreach(ProcedureStep curStep in newProceSteps)
            {
                curStep.BelongToProcedure = curProce;
                mainDataSet.InsertNewStep(curStep);
            }
            curProce.ProcedureSteps = newProceSteps;
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
            newSteps = new List<StepStore>();
            foreach (ProcedureStep curStep in curItem.ProcedureSteps)
            {
                newSteps.Add(new StepStore(curStep));
            }
            StepsGird.ItemsSource = newSteps;
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

        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            if(StepsGird.SelectedIndex == -1 || StepsGird.SelectedIndex == 0)
            {
                return;
            }
            int index = StepsGird.SelectedIndex;
            StepStore curStep = newSteps[index];
            newSteps.Remove(curStep);
            newSteps.Insert(index - 1, curStep);
            StepsGird.ItemsSource = null;
            StepsGird.ItemsSource = newSteps;
            StepsGird.SelectedIndex = index - 1;
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (StepsGird.SelectedIndex == -1 || StepsGird.SelectedIndex == newSteps.Count -1)
            {
                return;
            }
            int index = StepsGird.SelectedIndex;
            StepStore curStep = newSteps[index];
            newSteps.Remove(curStep);
            newSteps.Insert(index + 1, curStep);
            StepsGird.ItemsSource = null;
            StepsGird.ItemsSource = newSteps;
            StepsGird.SelectedIndex = index + 1;
        }

        private void AddStepButton_Click(object sender, RoutedEventArgs e)
        {
            WinEditStep winEditStep = new WinEditStep(mainDataSet);
            winEditStep.Owner = this;
            if (winEditStep.ShowDialog() == false)
            {
                return;
            }
            StepsGird.ItemsSource = null;
            newSteps.Add(winEditStep.newStep);
            StepsGird.ItemsSource = newSteps;
        }

        private void DelStepButton_Click(object sender, RoutedEventArgs e)
        {
            if (StepsGird.SelectedIndex == -1)
            {
                return;
            }
            newSteps.RemoveAt(StepsGird.SelectedIndex);
            StepsGird.ItemsSource = null;
            StepsGird.ItemsSource = newSteps;
        }
    }

    public class StepStore
    {
        //Name
        string strName;
        //HandleRole[1:1]
        string strGroup;
        //IsFeedBack
        bool bolIsFeedback;
        //Description
        string strDescription;

        public string Name
        {
            set { strName = value; }
            get { return strName; }
        }
        public string HandleGroup
        {
            set { strGroup = value; }
            get { return strGroup; }
        }
        public bool IsFeedback
        {
            set { bolIsFeedback = value; }
            get { return bolIsFeedback; }
        }
        public string Description
        {
            set { strDescription = value; }
            get { return strDescription; }
        }

        public StepStore(string sName, string sGroup, bool bFeedback, string sDesc)
        {
            strName = sName;
            strGroup = sGroup;
            bolIsFeedback = bFeedback;
            strDescription = sDesc;
        }

        public StepStore(ProcedureStep curStep)
        {
            strName = curStep.Name;
            bolIsFeedback = curStep.IsFeedback;
            if (bolIsFeedback == false)
            {
                strGroup = curStep.HandleRole.Name;
            }
            strDescription = curStep.Description;
        }

    }
}
