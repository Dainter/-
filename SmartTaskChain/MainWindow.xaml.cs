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
            ConfigWindow WinConfig = new ConfigWindow();
            WinConfig.ShowDialog();

            //BuildQlevelNodes();
            //BuildProcedure1();
            //BuildProcedure2();
            //BuildUserRole();
            //BuildTaskType();
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

        private void BuildProcedure1()
        {
            Procedure newProce = new Procedure("报修处理流程", "客户报修处理流程");
            DataReader.InsertRecord(new Record(newProce.Name, newProce.Type, newProce.XMLSerialize()));
            ProcedureStep newStep;
            newStep = new ProcedureStep("报修_问题提交", 1, "客户提交问题");
            DataReader.InsertRecord(new Record(newStep.Name, newStep.Type, newStep.XMLSerialize()));
            newStep = new ProcedureStep("报修_问题审核", 2, "客服人员审核问题");
            DataReader.InsertRecord(new Record(newStep.Name, newStep.Type, newStep.XMLSerialize()));
            newStep = new ProcedureStep("报修_维修单分配", 3, "客服经理收集维修单");
            DataReader.InsertRecord(new Record(newStep.Name, newStep.Type, newStep.XMLSerialize()));
            newStep = new ProcedureStep("报修_维修单处理", 4, "工程师处理维修单");
            DataReader.InsertRecord(new Record(newStep.Name, newStep.Type, newStep.XMLSerialize()));
            newStep = new ProcedureStep("报修_维修结果反馈", 5, "客户反馈维修结果");
            DataReader.InsertRecord(new Record(newStep.Name, newStep.Type, newStep.XMLSerialize()));

            //newRelation = new RelationShip("", "Procedure", "", "ProcedureStep", "Include", "1");
            RelationShip newRelation;
            newRelation = new RelationShip("报修处理流程", "Procedure", "报修_问题提交", "ProcedureStep", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修处理流程", "Procedure", "报修_问题审核", "ProcedureStep", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修处理流程", "Procedure", "报修_维修单分配", "ProcedureStep", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修处理流程", "Procedure", "报修_维修单处理", "ProcedureStep", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修处理流程", "Procedure", "报修_维修结果反馈", "ProcedureStep", "Include", "1");
            DataReader.InsertRelationShip(newRelation);

            //newRelation = new RelationShip("", "ProcedureStep", "", "ProcedureStep", "Include", "1");
            newRelation = new RelationShip("报修_问题提交", "ProcedureStep", "报修_问题审核", "ProcedureStep", "Next", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_问题审核", "ProcedureStep", "报修_问题提交", "ProcedureStep", "Previous", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_问题审核", "ProcedureStep", "报修_维修单分配", "ProcedureStep", "Next", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_维修单分配", "ProcedureStep", "报修_问题审核", "ProcedureStep", "Previous", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_维修单分配", "ProcedureStep", "报修_维修单处理", "ProcedureStep", "Next", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_维修单处理", "ProcedureStep", "报修_维修单分配", "ProcedureStep", "Previous", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_维修单处理", "ProcedureStep", "报修_维修结果反馈", "ProcedureStep", "Next", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_维修结果反馈", "ProcedureStep", "报修_维修单处理", "ProcedureStep", "Previous", "1");
            DataReader.InsertRelationShip(newRelation);
            DataReader.AcceptModification();
        }

        private void BuildProcedure2()
        {
            Procedure newProce = new Procedure("咨询处理流程", "客户咨询处理流程");
            DataReader.InsertRecord(new Record(newProce.Name, newProce.Type, newProce.XMLSerialize()));
            ProcedureStep newStep;
            newStep = new ProcedureStep("咨询_问题提交", 1, "客户提交问题");
            DataReader.InsertRecord(new Record(newStep.Name, newStep.Type, newStep.XMLSerialize()));
            newStep = new ProcedureStep("咨询_问题回复", 2, "客服人员回复问题解决方案");
            DataReader.InsertRecord(new Record(newStep.Name, newStep.Type, newStep.XMLSerialize()));
            newStep = new ProcedureStep("咨询_咨询结果反馈", 3, "客户反馈咨询结果");
            DataReader.InsertRecord(new Record(newStep.Name, newStep.Type, newStep.XMLSerialize()));

            //newRelation = new RelationShip("", "Procedure", "", "ProcedureStep", "Include", "1");
            RelationShip newRelation;
            newRelation = new RelationShip("咨询处理流程", "Procedure", "咨询_问题提交", "ProcedureStep", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询处理流程", "Procedure", "咨询_问题回复", "ProcedureStep", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询处理流程", "Procedure", "咨询_咨询结果反馈", "ProcedureStep", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            //newRelation = new RelationShip("", "ProcedureStep", "", "ProcedureStep", "Include", "1");
            newRelation = new RelationShip("咨询_问题提交", "ProcedureStep", "咨询_问题回复", "ProcedureStep", "Next", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询_问题回复", "ProcedureStep", "咨询_问题提交", "ProcedureStep", "Previous", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询_问题回复", "ProcedureStep", "咨询_咨询结果反馈", "ProcedureStep", "Next", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询_咨询结果反馈", "ProcedureStep", "咨询_问题回复", "ProcedureStep", "Previous", "1");
            DataReader.InsertRelationShip(newRelation);
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

            //newRelation = new RelationShip("", "UserRole", "", "ProcedureStep", "InCharge", "1");
            //newRelation = new RelationShip("", "ProcedureStep", "", "UserRole", "HandleBy", "1");
            RelationShip newRelation;

            newRelation = new RelationShip("客户", "UserRole", "报修_问题提交", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_问题提交", "ProcedureStep", "客户", "UserRole", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("客服人员", "UserRole", "报修_问题审核", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_问题审核", "ProcedureStep", "客服人员", "UserRole", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("客服经理", "UserRole", "报修_维修单分配", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_维修单分配", "ProcedureStep", "客服经理", "UserRole", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("维修工程师", "UserRole", "报修_维修单处理", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_维修单处理", "ProcedureStep", "维修工程师", "UserRole", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("客户", "UserRole", "报修_维修结果反馈", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_维修结果反馈", "ProcedureStep", "客户", "UserRole", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);


            newRelation = new RelationShip("客户", "UserRole", "咨询_问题提交", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询_问题提交", "ProcedureStep", "客户", "UserRole", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("客服人员", "UserRole", "咨询_问题回复", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询_问题回复", "ProcedureStep", "客服人员", "UserRole", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("客户", "UserRole", "咨询_咨询结果反馈", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询_咨询结果反馈", "ProcedureStep", "客户", "UserRole", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);

            DataReader.AcceptModification();
        }

        private void BuildTaskType()
        {
            TaskType newType;
            newType = new TaskType("维修任务", "用户申报维修任务。");
            DataReader.InsertRecord(new Record(newType.Name, newType.Type, newType.XMLSerialize()));
            newType = new TaskType("咨询任务", "用户提出咨询。");
            DataReader.InsertRecord(new Record(newType.Name, newType.Type, newType.XMLSerialize()));

            //Assign & Binding
            RelationShip newRelation;
            newRelation = new RelationShip("维修任务", "TaskType", "报修处理流程", "Procedure", "Assign", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修处理流程", "Procedure", "维修任务", "TaskType", "Binding", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询任务", "TaskType", "咨询处理流程", "Procedure", "Assign", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询处理流程", "Procedure", "咨询任务", "TaskType", "Binding", "1");
            DataReader.InsertRelationShip(newRelation);

            DataReader.AcceptModification();
        }
    }
}
