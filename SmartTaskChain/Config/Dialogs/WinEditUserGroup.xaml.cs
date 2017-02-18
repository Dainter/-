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
    /// WinEditUserGroup.xaml 的交互逻辑
    /// </summary>
    public partial class WinEditUserGroup : Window
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
        string strName, strDescription;
        List<string> Users;

        public WinEditUserGroup(MainDataSet DataSet)
        {
            InitializeComponent();
            mainDataSet = DataSet;
        }

        private void winEditUserGroup_Loaded(object sender, RoutedEventArgs e)
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
            NameComboBox.ItemsSource = mainDataSet.UserGroups;
            UsersComboBox.ItemsSource = mainDataSet.Users;
        }

        private void winEditUserGroup_MouseMove(object sender, MouseEventArgs e)
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
                CreateUserGroup();
            }
            else
            {
                EditUserGroup();
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
            //绑定用户
            Users = new List<string>();
            foreach (object curItem in UsersListBox.Items)
            {
                Users.Add(curItem.ToString());
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

        private void EditUserGroup()
        {
            UserGroup curUserGroup;
            strName = NameComboBox.Text;
            curUserGroup = mainDataSet.GetGroupItem(strName);
            if (curUserGroup == null)
            {
                InputWarning.PlacementTarget = NewNameBox;
                WarningInfo.Text = "Selected User is not exists in DB.";
                InputWarning.IsOpen = true;
                return;
            }

            RefreshUsers(curUserGroup);
            curUserGroup.Description = strDescription;
            return;
        }

        private void RefreshUsers(UserGroup curUserGroup)
        {
            bool bolIsExist;
            List<string> insertUsers;
            List<IfUser> deleteUsers;

            //比较两表生成一个Insert表，一个Delete表
            deleteUsers = new List<IfUser>();
            foreach (IfUser curUser in curUserGroup.Users)
            {
                bolIsExist = false;
                foreach (string sItem in Users)
                {
                    if (curUser.Name == sItem)
                    {
                        bolIsExist = true;
                    }
                }
                if (bolIsExist == false)
                {
                    deleteUsers.Add(curUser);
                }
            }
            insertUsers = new List<string>();
            foreach (string sItem in Users)
            {
                bolIsExist = false;
                foreach (IfUser curUser in curUserGroup.Users)
                {
                    if (curUser.Name == sItem)
                    {
                        bolIsExist = true;
                    }
                }
                if (bolIsExist == false)
                {
                    insertUsers.Add(sItem);
                }
            }
            if (deleteUsers.Count != 0)
            {
                foreach (IfUser curUser in deleteUsers)
                {
                    curUserGroup.Users.Remove(curUser);
                    curUser.UserGroups.Remove(curUserGroup);
                }
            }
            if (insertUsers.Count != 0)
            {
                IfUser newUser;
                foreach (string curItem in insertUsers)
                {
                    newUser = mainDataSet.GetUserItem(curItem);
                    if (newUser == null)
                    {
                        continue;
                    }
                    curUserGroup.Users.Add(newUser);
                    newUser.UserGroups.Add(curUserGroup);
                }
            }
        }

        private void CreateUserGroup()
        {
            UserGroup newGroup = new UserGroup(strName, strDescription);

            RefreshUsers(newGroup);
            mainDataSet.InsertNewGroup(newGroup);
            return;
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
            StepListBox.Items.Clear();
            UsersComboBox.Text = "";
            UsersListBox.Items.Clear();
            DescriptionBox.Text = "";
        }

        private void LoadInfo(string sName)
        {
            UserGroup curItem = mainDataSet.GetGroupItem(sName);
            if (curItem == null)
            {
                return;
            }
            foreach (ProcedureStep curStep in curItem.BindingStep)
            {
                StepListBox.Items.Add(curStep.Name);
            }
            foreach (IfUser curUser in curItem.Users)
            {
                UsersListBox.Items.Add(curUser.Name);
            }
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

        private void UsersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            UsersListBox.SelectedIndex = -1;
        }

        private void AddOrRemoveUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersComboBox.Text != "")
            {
                //加入
                if (UsersListBox.Items.Contains(UsersComboBox.Text) == false)
                {
                    UsersListBox.Items.Add(UsersComboBox.Text);
                }
                return;
            }
            if (UsersListBox.SelectedIndex == -1)
            {
                return;
            }
            //删除
            UsersListBox.Items.RemoveAt(UsersListBox.SelectedIndex);
            return;
        }

        private void UsersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            UsersComboBox.Text = "";
        }
    }
}
