using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using SmartTaskChain.Model;
using SmartTaskChain.Business;

namespace SmartTaskChain.Config.Dialogs
{

    /// <summary>
    /// WinEditUser.xaml 的交互逻辑
    /// </summary>
    public partial class WinEditUser : Window
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
        string strName, strPassword, strEmployee, strPhone, strCompany;
        List<string> UserGroups, Inferiors;

        public WinEditUser(MainDataSet DataSet)
        {
            InitializeComponent();
            mainDataSet = DataSet;
        }

        private void winEditUser_Loaded(object sender, RoutedEventArgs e)
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
            List<string> UserTypes = new List<string>();

            var classes = Assembly.Load("SmartTaskChain").GetTypes();
            foreach (var item in classes)
            {
                if(item.FullName.Contains("Business") == false)
                {
                    continue;
                }
                foreach(Type curType in item.GetInterfaces())
                {
                    if(curType.Name == "IfUser")
                    {
                        UserTypes.Add(item.Name);
                    }
                }
            }

            NameComboBox.ItemsSource = mainDataSet.Users;
            TypeComboBox.ItemsSource = UserTypes;
            GroupComboBox.ItemsSource = mainDataSet.UserGroups;
            UserComboBox.ItemsSource = mainDataSet.Users;
            
        }

        private void winEditUser_MouseMove(object sender, MouseEventArgs e)
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
                CreateUser();
            }
            else
            {
                EditUser();
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
            strPassword = PasswordTextBox.Text;
            if (TypeComboBox.Text == "Manager" || TypeComboBox.Text == "Engineer" || TypeComboBox.Text == "ServiceUser")
            {
                strEmployee = EmployeeTextBox.Text;
                if (strEmployee == "")
                {
                    InputWarning.PlacementTarget = CompanyTextBox;
                    WarningInfo.Text = "Please enter a non-empty value.";
                    InputWarning.IsOpen = true;
                    return false;
                }
            }

            //用户组
            UserGroups = new List<string>();
            foreach(object curItem in GroupListBox.Items)
            {
                UserGroups.Add(curItem.ToString());
            }
            if(TypeComboBox.Text == "Customer")
            {
                //联系电话
                strPhone = PhoneTextBox.Text;
                if (strPhone == "")
                {
                    InputWarning.PlacementTarget = PhoneTextBox;
                    WarningInfo.Text = "Please enter a non-empty value.";
                    InputWarning.IsOpen = true;
                    return false;
                }
                //客户单位
                strCompany = CompanyTextBox.Text;
                if (strCompany == "")
                {
                    InputWarning.PlacementTarget = CompanyTextBox;
                    WarningInfo.Text = "Please enter a non-empty value.";
                    InputWarning.IsOpen = true;
                    return false;
                }
            }
            else if(TypeComboBox.Text == "Manager")
            {
                Inferiors = new List<string>();
                foreach (object curItem in InferiorListBox.Items)
                {
                    Inferiors.Add(curItem.ToString());
                }
            }
            return true;
        }

        private void EditUser()
        {
            IfUser curUser;
            strName = NameComboBox.Text;
            curUser = mainDataSet.GetUserItem(strName);
            if (curUser == null)
            {
                InputWarning.PlacementTarget = NewNameBox;
                WarningInfo.Text = "Selected User is not exists in DB.";
                InputWarning.IsOpen = true;
                return;
            }
            curUser.Password = strPassword;
            RefreshUserGroup(curUser);
            if(TypeComboBox.Text == "Manager")
            {
                RefreshInferior((Manager)curUser);
            }
            return;
        }

        private void RefreshUserGroup(IfUser curUser)
        {
            bool bolIsExist;
            List<string> insertGroups;
            List<UserGroup> deleteGroups;

            //比较两表生成一个Insert表，一个Delete表
            deleteGroups = new List<UserGroup>();
            foreach (UserGroup curGroup in curUser.UserGroups)
            {
                bolIsExist = false;
                foreach (string sItem in UserGroups)
                {
                    if (curGroup.Name == sItem)
                    {
                        bolIsExist = true;
                    }
                }
                if (bolIsExist == false)
                {
                    deleteGroups.Add(curGroup);
                }
            }
            insertGroups = new List<string>();
            foreach (string sItem in UserGroups)
            {
                bolIsExist = false;
                foreach (UserGroup curGroup in curUser.UserGroups)
                {
                    if (curGroup.Name == sItem)
                    {
                        bolIsExist = true;
                    }
                }
                if (bolIsExist == false)
                {
                    insertGroups.Add(sItem);
                }
            }
            if (deleteGroups.Count != 0 )
            {
                foreach (UserGroup curGroup in deleteGroups)
                {
                    curUser.UserGroups.Remove(curGroup);
                    curGroup.Users.Remove(curUser);
                }
            }
            if (insertGroups.Count != 0)
            {
                UserGroup newGroup;
                foreach(string curItem in insertGroups)
                {
                    newGroup = mainDataSet.GetGroupItem(curItem);
                    if(newGroup == null)
                    {
                        continue;
                    }
                    curUser.UserGroups.Add(newGroup);
                    newGroup.Users.Add(curUser);
                }
            }
        }

        private void RefreshInferior(Manager curUser)
        {
            bool bolIsExist;
            List<string> insertInferiors;
            List<IfUser> deleteInferiors;

            //比较两表生成一个Insert表，一个Delete表
            deleteInferiors = new List<IfUser>();
            foreach (IfUser curInferior in curUser.Inferiors)
            {
                bolIsExist = false;
                foreach (string sItem in Inferiors)
                {
                    if (curInferior.Name == sItem)
                    {
                        bolIsExist = true;
                        break;
                    }
                }
                if (bolIsExist == false)
                {
                    deleteInferiors.Add(curInferior);
                }
            }
            insertInferiors = new List<string>();
            foreach (string sItem in Inferiors)
            {
                bolIsExist = false;
                foreach (IfUser curInferior in curUser.Inferiors)
                {
                    if (curInferior.Name == sItem)
                    {
                        bolIsExist = true;
                    }
                }
                if (bolIsExist == false)
                {
                    insertInferiors.Add(sItem);
                }
            }
            if (deleteInferiors.Count != 0)
            {
                foreach (IfUser curInferior in deleteInferiors)
                {
                    curUser.Inferiors.Remove(curInferior);
                }
            }
            if (insertInferiors.Count != 0)
            {
                IfUser newUser;
                foreach (string curItem in insertInferiors)
                {
                    newUser = mainDataSet.GetUserItem(curItem);
                    if (newUser == null)
                    {
                        continue;
                    }
                    curUser.Inferiors.Add(newUser);
                }
            }
        }

        private void CreateUser()
        {
            IfUser newUser = SelectUserType(TypeComboBox.Text);

            RefreshUserGroup(newUser);
            if (TypeComboBox.Text == "Manager")
            {
                RefreshInferior((Manager)newUser);
            }
            mainDataSet.InsertNewUser(newUser);
            return;
        }



        private IfUser SelectUserType(string sType)
        {
            switch(sType)
            {
                case "Customer":
                    return new Customer(strName, strPassword, strPhone, strCompany);
                case "ServiceUser":
                    return new ServiceUser(strName, strPassword, strEmployee);
                case "Engineer":
                    return new Engineer(strName, strPassword, strEmployee);
                case "Manager":
                    return new Manager(strName, strPassword, strEmployee);
                default:
                    return new Customer(strName, strPassword, "", "");
            }
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
            PasswordTextBox.Text = "";
            GroupComboBox.Text = "";
            GroupListBox.Items.Clear();
            PhoneTextBox.Text = "";
            CompanyTextBox.Text = "";
            UserComboBox.Text = "";
            InferiorListBox.Items.Clear();
        }

        private void LoadInfo(string sName)
        {
            IfUser curItem = mainDataSet.GetUserItem(sName);
            if (curItem == null)
            {
                return;
            }
            TypeComboBox.Text = curItem.Type;
            PasswordTextBox.Text = curItem.Password;
            foreach(UserGroup curGroup in curItem.UserGroups)
            {
                GroupListBox.Items.Add(curGroup.Name);
            }
            if(curItem.Type == "Customer")
            {
                PhoneTextBox.Text = ((Customer)curItem).Phone;
                CompanyTextBox.Text = ((Customer)curItem).Company;
            }
            else if (curItem.Type == "Engineer")
            {
                EmployeeTextBox.Text = ((Engineer)curItem).EmployeeNumber;
            }
            else if (curItem.Type == "ServiceUser")
            {
                EmployeeTextBox.Text = ((ServiceUser)curItem).EmployeeNumber;
            }
            else if(curItem.Type == "Manager")
            {
                EmployeeTextBox.Text = ((Manager)curItem).EmployeeNumber;
                foreach (IfUser curUser in ((Manager)curItem).Inferiors)
                {
                    InferiorListBox.Items.Add(curUser.Name);
                }
            }
        }

        private void GroupListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count == 0)
            {
                return;
            }
            GroupComboBox.Text = "";
        }

        private void GroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                GroupListBox.SelectedIndex = -1;
            }
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

        private void InferiorListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            UserComboBox.Text = "";
        }
        
        private void UserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                InferiorListBox.SelectedIndex = -1;
            }
        }

        private void AddOrRemoveGroupButton_Click(object sender, RoutedEventArgs e)
        {
            if(GroupComboBox.Text != "")
            {
                //加入
                if(GroupListBox.Items.Contains(GroupComboBox.Text) == false)
                {
                    GroupListBox.Items.Add(GroupComboBox.Text);
                }
                return;
            }
            if (GroupListBox.SelectedIndex == -1)
            {
                return;
            }
            //删除
            GroupListBox.Items.RemoveAt(GroupListBox.SelectedIndex);
            return;
        }

        private void AddOrRemoveUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserComboBox.Text != "")
            {
                //加入
                if (InferiorListBox.Items.Contains(UserComboBox.Text) == false)
                {
                    
                    if (BusinessContrain(UserComboBox.Text) == true)
                    {
                        InferiorListBox.Items.Add(UserComboBox.Text);
                    }
                }
                return;
            }
            if (InferiorListBox.SelectedIndex == -1)
            {
                return;
            }
            //删除
            InferiorListBox.Items.RemoveAt(InferiorListBox.SelectedIndex);
            return;
        }

        private bool BusinessContrain(string sName)
        {
            string curName;
            if (IsCreateCheckBox.IsChecked == true)
            {
                curName = NewNameBox.Text;
            }
            else
            {
                curName = NameComboBox.Text;
            }
            if (sName == curName)
            {
                InputWarning.PlacementTarget = UserComboBox;
                WarningInfo.Text = "Can not add user himself into the Inferior List.";
                InputWarning.IsOpen = true;
                return false;
            }
            return true;
        }
    }
}
