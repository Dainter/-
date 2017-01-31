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
            //BuildProcedure();
            //BuildUserRole();
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

        private void BuildProcedure()
        {
            Procedure newProce = new Procedure("客服咨询处理流程", "客服咨询处理处理流程");
            DataReader.InsertRecord(new Record(newProce.Name, newProce.Type, newProce.XMLSerialize()));
            ProcedureStep newStep;
            newStep = new ProcedureStep("问题提交", 1, "客户提交问题");
            DataReader.InsertRecord(new Record(newStep.Name, newStep.Type, newStep.XMLSerialize()));
            newStep = new ProcedureStep("问题审核", 2, "客服人员审核问题");
            DataReader.InsertRecord(new Record(newStep.Name, newStep.Type, newStep.XMLSerialize()));
            newStep = new ProcedureStep("维修单分配", 3, "客服经理收集维修单");
            DataReader.InsertRecord(new Record(newStep.Name, newStep.Type, newStep.XMLSerialize()));
            newStep = new ProcedureStep("维修单处理", 4, "工程师处理维修单");
            DataReader.InsertRecord(new Record(newStep.Name, newStep.Type, newStep.XMLSerialize()));
            newStep = new ProcedureStep("维修结果反馈", 5, "客户反馈维修结果");
            DataReader.InsertRecord(new Record(newStep.Name, newStep.Type, newStep.XMLSerialize()));
            DataReader.AcceptModification();
        }
        
        private void BuildUserRole()
        {
            UserRole newRole;
            newRole = new UserRole("客户", "权限：提交维修单，查看自己提交的维修单。");
            DataReader.InsertRecord(new Record(newRole.Name, newRole.Type, newRole.XMLSerialize()));
            newRole = new UserRole("客服人员", "权限：审核维修单，查看自己处理的维修单。");
            DataReader.InsertRecord(new Record(newRole.Name, newRole.Type, newRole.XMLSerialize()));
            newRole = new UserRole("客服经理", "权限：收集维修单，查看自己处理的维修单，查看自己下属的维修单。");
            DataReader.InsertRecord(new Record(newRole.Name, newRole.Type, newRole.XMLSerialize()));
            newRole = new UserRole("维修工程师", "权限：处理维修单，查看自己处理的维修单。");
            DataReader.InsertRecord(new Record(newRole.Name, newRole.Type, newRole.XMLSerialize()));
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
