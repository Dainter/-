using System;
using System.Collections.Generic;
using System.Windows;
using SmartTaskChain.Config;
using SmartTaskChain.DataAbstract;
using SmartTaskChain.Model;
using SmartTaskChain.Business;


namespace SmartTaskChain
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MainDataSet SmartChainDataSet;
        public MainWindow()
        {
            InitializeComponent();
            SmartChainDataSet = MainDataSet.GetDataSet();
            //SmartChainDataSet.InsertAllData();
        }

        private void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            ConfigWindow WinConfig = new ConfigWindow();
            WinConfig.ShowDialog();

            
        }


    }
}
