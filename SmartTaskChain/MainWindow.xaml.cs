using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Xml;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SmartTaskChain.DataAbstract;
using SmartTaskChain.Model;
using SmartTaskChain.Business;
using SmartTaskChain.Config;
using GraphDB;

namespace SmartTaskChain
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        IfDataStrategy DataReader;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DataInit()
        {
            string strDBpath = "Database.xml";
            DataReader = DataStrategyFactory.GetFactory().GetDataReader(strDBpath);
            if(DataReader == null)
            {
                return;
            }
        }

        private void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            DataInit();
            //ConfigWindow WinConfig = new ConfigWindow();
            //WinConfig.ShowDialog();
            //Submit();
            //BuildQlevelNodes();
        }

        private void BuildQlevelNodes()
        {
            QLevel newNode;

            newNode = new QLevel(true, true);
            DataReader.InsertRecord(new Record(newNode.Name, newNode.Type, newNode.XMLSerialize()));
            newNode = new QLevel(true, false);
            DataReader.InsertRecord(new Record(newNode.Name, newNode.Type, newNode.XMLSerialize()));
            newNode = new QLevel(false, true);
            DataReader.InsertRecord(new Record(newNode.Name, newNode.Type, newNode.XMLSerialize()));
            newNode = new QLevel(false, false);
            DataReader.InsertRecord(new Record(newNode.Name, newNode.Type, newNode.XMLSerialize()));
            DataReader.AcceptModification();
        }

        

        private void Submit()
        {
            DefectTask newDefect;
            //1.创建Task

            //存入数据库
            if (DataReader == null)
            {
                return;
            }

            //存入数据库，调用数据抽象层接口
            for (int i = 1; i < 5; i++)
            {
                newDefect = new DefectTask("Defect"+i.ToString(), DateTime.Now, DateTime.Now, "New defect" + i.ToString(), true, true);
                DataReader.InsertRecord(new Record(newDefect.Name, newDefect.Type, newDefect.XMLSerialize()));
            }
            List<Record> Records = new List<Record>();
            for (int i = 5; i < 10; i++)
            {
                newDefect = new DefectTask("Defect" + i.ToString(), DateTime.Now, DateTime.Now, "New defect" + i.ToString(), true, true);
                Records.Add(new Record(newDefect.Name, newDefect.Type, newDefect.XMLSerialize()));
            }
            DataReader.InsertRecords(Records);
            DataReader.AcceptModification();
            //读取数据库，调用数据抽象层接口
            //XmlElement ModelPayload;
            //Task curTask = new Task(ModelPayload);
            //恢复为业务层对象
            //DefectTask deserialize = new DefectTask(ModelPayload);
        }

    }
}
