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
        public MainWindow()
        {
            InitializeComponent();
            SmartChainDataSet = MainDataSet.GetDataSet();
            //SmartChainDataSet.InsertAllData();
            ConfigWindow WinConfig = new ConfigWindow(SmartChainDataSet);
            WinConfig.ShowDialog();
        }

        private void DemoItem_Click(object sender, EventArgs e)
        {

        }

        private void ConfigItem_Click(object sender, EventArgs e)
        {
            
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
