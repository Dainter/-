using System;
using System.Collections.Generic;
using System.Windows;
using SmartTaskChain.Config;
using SmartTaskChain.DataAbstract;
using SmartTaskChain.Model;
using SmartTaskChain.Business;
using SmartTaskChain.UI_Resources;


namespace SmartTaskChain
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : GWindow
    {
        MainDataSet SmartChainDataSet;
        bool IsShow;
        public MainWindow()
        {
            InitializeComponent();
            SmartChainDataSet = MainDataSet.GetDataSet();
            IsShow = false;
            SmartChainDataSet.InsertAllData();
            WinDemo winDemo = new WinDemo(SmartChainDataSet);
            winDemo.ShowDialog();
        }

        private void DemoItem_Click(object sender, EventArgs e)
        {
            if (IsShow == true)
            {
                return;
            }
            IsShow = true;
            WinDemo winDemo = new WinDemo(SmartChainDataSet);
            winDemo.ShowDialog();
            IsShow = false;
        }

        private void ConfigItem_Click(object sender, EventArgs e)
        {
            if(IsShow == true)
            {
                return;
            }
            IsShow = true;
            ConfigWindow winConfig = new ConfigWindow(SmartChainDataSet);
            winConfig.ShowDialog();
            SmartChainDataSet.RefreshDataSet();
            IsShow = false;
        }

        private void ShowHistoryItem_Click(object sender, EventArgs e)
        {

        }

        private void ShowOptionItem_Click(object sender, EventArgs e)
        {

        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
