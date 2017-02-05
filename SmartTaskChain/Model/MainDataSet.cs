using System;
using System.Collections.Generic;
using System.Xml;
using SmartTaskChain.Business;
using SmartTaskChain.DataAbstract;

namespace SmartTaskChain.Model
{
    public class MainDataSet
    {
        List<QLevel> qlevelList;
        List<TaskType> taskTypeList;
        List<Procedure> procedureList;
        List<ProcedureStep> stepList;
        List<UserGroup> userGroupList;
        List<IfUser> userList;
        List<IfTask> taskList;

        IfDataStrategy DataReader;

        public List<QLevel>QLevels
        {
            get { return qlevelList; }
        }
        public List<TaskType> TaskTypes
        {
            get { return taskTypeList; }
        }
        public List<Procedure> Procedures
        {
            get { return procedureList; }
        }
        public List<ProcedureStep> ProcedureSteps
        {
            get { return stepList; }
        }
        public List<UserGroup> UserGroups
        {
            get { return userGroupList; }
        }
        public List<IfUser> Users
        {
            get { return userList; }
        }
        public List<IfTask> Tasks
        {
            get { return taskList; }
        }


        public MainDataSet()
        {
            string strDBpath = Properties.Settings.Default.DataBasePath;
            DataReader = DataStrategyFactory.GetFactory().GetDataReader(strDBpath);
            if (DataReader == null)
            {
                return;
            }
            UpdateAllList();
            UpdateRelation();
            return;
        }     

        private static MainDataSet _dataset; //(2)

        public static MainDataSet GetDataSet() //(3)
        {
            if (_dataset == null)
                _dataset = new MainDataSet();
            return _dataset;
        }

        public void UpdateAllList()
        {
            userList = new List<IfUser>();
            taskList = new List<IfTask>();
            qlevelList = new List<QLevel>();
            taskTypeList = new List<TaskType>();
            procedureList = new List<Procedure>();
            stepList = new List<ProcedureStep>();
            userGroupList = new List<UserGroup>();

            List<Record> recordlist;
            recordlist = DataReader.GetRecordList();
            foreach (Record curRec in recordlist)
            {
                switch(curRec.Type)
                {
                    case "Customer":
                        userList.Add(new Customer(curRec.Payload));
                        break;
                    case "ServiceUser":
                        userList.Add(new ServiceUser(curRec.Payload));
                        break;
                    case "Engineer":
                        userList.Add(new Engineer(curRec.Payload));
                        break;
                    case "Manager":
                        userList.Add(new Manager(curRec.Payload));
                        break;
                    case "ProcedureTask":
                        taskList.Add(new ProcedureTask(curRec.Payload));
                        break;
                    case "CustomTask":
                        taskList.Add(new CustomTask(curRec.Payload));
                        break;
                    case "QLevel":
                        qlevelList.Add(new QLevel(curRec.Payload));
                        break;
                    case "TaskType":
                        taskTypeList.Add(new TaskType(curRec.Payload));
                        break;
                    case "Procedure":
                        procedureList.Add(new Procedure(curRec.Payload));
                        break;
                    case "ProcedureStep":
                        stepList.Add(new ProcedureStep(curRec.Payload));
                        break;
                    case "UserGroup":
                        userGroupList.Add(new UserGroup(curRec.Payload));
                        break;
                    default:
                        break;
                }
            }
        }

        public void UpdateRelation()
        {
            foreach (TaskType curItem in taskTypeList)
            {
                curItem.UpdateRelation(DataReader, this);
            }
            foreach (Procedure curItem in procedureList)
            {
                curItem.UpdateRelation(DataReader, this);
            }
            foreach (ProcedureStep curItem in stepList)
            {
                curItem.UpdateRelation(DataReader, this);
            }
            foreach (UserGroup curItem in userGroupList)
            {
                curItem.UpdateRelation(DataReader, this);
            }
            foreach (IfUser curItem in userList)
            {
                curItem.UpdateRelation(DataReader, this);
            }
            foreach (IfTask curItem in taskList)
            {
                curItem.UpdateRelation(DataReader, this);
            }
        }

        public object GetItem(string sName, string sType)
        {
            switch (sType)
            {
                case "Customer":
                case "ServiceUser":
                case "Engineer":
                case "Manager":
                    return GetUserItem(sName);
                case "ProcedureTask":
                case "CustomTask":
                    return GetTaskItem(sName);
                case "QLevel":
                    return GetQlevelItem(sName);
                case "TaskType":
                    return GetTypeItem(sName);
                case "Procedure":
                    return GetProcedureItem(sName);
                case "ProcedureStep":
                    return GetStepItem(sName);
                case "UserGroup":
                    return GetGroupItem(sName);
                default:
                    break;
            }
            return null;
        }

        public TaskType GetTypeItem(string sName)
        {
            foreach (TaskType curItem in taskTypeList)
            {
                if (curItem.Name == sName)
                {
                    return curItem;
                }
            }
            return null;
        }

        public QLevel GetQlevelItem(string sName)
        {
            foreach (QLevel curItem in qlevelList)
            {
                if (curItem.Name == sName)
                {
                    return curItem;
                }
            }
            return null;
        }

        public Procedure GetProcedureItem(string sName)
        {
            foreach (Procedure curItem in procedureList)
            {
                if (curItem.Name == sName)
                {
                    return curItem;
                }
            }
            return null;
        }

        public ProcedureStep GetStepItem(string sName)
        {
            foreach (ProcedureStep curItem in stepList)
            {
                if (curItem.Name == sName)
                {
                    return curItem;
                }
            }
            return null;
        }

        public UserGroup GetGroupItem(string sName)
        {
            foreach (UserGroup curItem in userGroupList)
            {
                if (curItem.Name == sName)
                {
                    return curItem;
                }
            }
            return null; 
        }

        public IfUser GetUserItem(string sName)
        {
            foreach (IfUser curItem in userList)
            {
                if (curItem.Name == sName)
                {
                    return curItem;
                }
            }
            return null;
        }

        public IfTask GetTaskItem(string sName)
        {
            foreach(IfTask curItem in taskList)
            {
                if(curItem.Name == sName)
                {
                    return curItem;
                }
            }
            return null;
        }

        #region UpdateList
        public void UpdateUserList()
        {
            List<Record> recordlist;
            userList = new List<IfUser>();

            recordlist = DataReader.GetRecordList("Customer");
            foreach(Record curRec in recordlist)
            {
                userList.Add(new Customer(curRec.Payload));
            }

            recordlist = DataReader.GetRecordList("ServiceUser");
            foreach (Record curRec in recordlist)
            {
                userList.Add(new ServiceUser(curRec.Payload));
            }

            recordlist = DataReader.GetRecordList("Engineer");
            foreach (Record curRec in recordlist)
            {
                userList.Add(new Engineer(curRec.Payload));
            }

            recordlist = DataReader.GetRecordList("Manager");
            foreach (Record curRec in recordlist)
            {
                userList.Add(new Manager(curRec.Payload));
            }

        }

        public void UpdateTaskList()
        {
            List<Record> recordlist;
            taskList = new List<IfTask>();

            recordlist = DataReader.GetRecordList("ProcedureTask");
            foreach (Record curRec in recordlist)
            {
                taskList.Add(new ProcedureTask(curRec.Payload));
            }

            recordlist = DataReader.GetRecordList("CustomTask");
            foreach (Record curRec in recordlist)
            {
                taskList.Add(new CustomTask(curRec.Payload));
            }
        }

        public void UpdateQlevelList()
        {
            List<Record> recordlist;
            qlevelList = new List<QLevel>();

            recordlist = DataReader.GetRecordList("QLevel");
            foreach (Record curRec in recordlist)
            {
                qlevelList.Add(new QLevel(curRec.Payload));
            }
        }

        public void UpdateTaskTypeList()
        {
            List<Record> recordlist;
            taskTypeList = new List<TaskType>(); 

            recordlist = DataReader.GetRecordList("TaskType");
            foreach (Record curRec in recordlist)
            {
                taskTypeList.Add(new TaskType(curRec.Payload));
            }
        }

        public void UpdateProcedureList()
        {
            List<Record> recordlist;
            procedureList = new List<Procedure>();

            recordlist = DataReader.GetRecordList("Procedure");
            foreach (Record curRec in recordlist)
            {
                procedureList.Add(new Procedure(curRec.Payload));
            }
        }

        public void UpdateProceStepList()
        {
            List<Record> recordlist;
            stepList = new List<ProcedureStep>();

            recordlist = DataReader.GetRecordList("ProcedureStep");
            foreach (Record curRec in recordlist)
            {
                stepList.Add(new ProcedureStep(curRec.Payload));
            }
        }

        public void UpdateUserGroupList()
        {
            List<Record> recordlist;
            userGroupList = new List<UserGroup>();

            recordlist = DataReader.GetRecordList("UserGroup");
            foreach (Record curRec in recordlist)
            {
                userGroupList.Add(new UserGroup(curRec.Payload));
            }
        }

        #endregion

        #region Build Data

        public void InsertAllData()
        {
            if (DataReader == null)
            {
                return;
            }
            BuildQlevelNodes();
            BuildProcedure1();
            BuildProcedure2();
            BuildUserGroup();
            BuildTaskType();
            BuildUser();
            BuildTask();
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
            newRelation = new RelationShip("报修_问题提交", "ProcedureStep", "报修处理流程", "Procedure", "BelongTo", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修处理流程", "Procedure", "报修_问题审核", "ProcedureStep", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_问题审核", "ProcedureStep", "报修处理流程", "Procedure", "BelongTo", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修处理流程", "Procedure", "报修_维修单分配", "ProcedureStep", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_维修单分配", "ProcedureStep", "报修处理流程", "Procedure", "BelongTo", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修处理流程", "Procedure", "报修_维修单处理", "ProcedureStep", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_维修单处理", "ProcedureStep", "报修处理流程", "Procedure", "BelongTo", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修处理流程", "Procedure", "报修_维修结果反馈", "ProcedureStep", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_维修结果反馈", "ProcedureStep", "报修处理流程", "Procedure", "BelongTo", "1");
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
            newRelation = new RelationShip("咨询_问题提交", "ProcedureStep", "咨询处理流程", "Procedure", "BelongTo", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询处理流程", "Procedure", "咨询_问题回复", "ProcedureStep", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询_问题回复", "ProcedureStep", "咨询处理流程", "Procedure", "BelongTo", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询处理流程", "Procedure", "咨询_咨询结果反馈", "ProcedureStep", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询_咨询结果反馈", "ProcedureStep", "咨询处理流程", "Procedure", "BelongTo", "1");
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

        private void BuildUserGroup()
        {
            UserGroup newGroup;
            newGroup = new UserGroup("客户", "权限：提交维修单，查看自己提交的维修单。");
            DataReader.InsertRecord(new Record(newGroup.Name, newGroup.Type, newGroup.XMLSerialize()));
            newGroup = new UserGroup("客服人员", "权限：审核维修单，查看自己处理的维修单。");
            DataReader.InsertRecord(new Record(newGroup.Name, newGroup.Type, newGroup.XMLSerialize()));
            newGroup = new UserGroup("客服经理", "权限：收集维修单，查看自己处理的维修单，查看自己下属的维修单。");
            DataReader.InsertRecord(new Record(newGroup.Name, newGroup.Type, newGroup.XMLSerialize()));
            newGroup = new UserGroup("维修工程师", "权限：处理维修单，查看自己处理的维修单。");
            DataReader.InsertRecord(new Record(newGroup.Name, newGroup.Type, newGroup.XMLSerialize()));

            //newRelation = new RelationShip("", "UserGroup", "", "ProcedureStep", "InCharge", "1");
            //newRelation = new RelationShip("", "ProcedureStep", "", "UserGroup", "HandleBy", "1");
            RelationShip newRelation;

            newRelation = new RelationShip("客户", "UserGroup", "报修_问题提交", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_问题提交", "ProcedureStep", "客户", "UserGroup", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("客服人员", "UserGroup", "报修_问题审核", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_问题审核", "ProcedureStep", "客服人员", "UserGroup", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("客服经理", "UserGroup", "报修_维修单分配", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_维修单分配", "ProcedureStep", "客服经理", "UserGroup", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("维修工程师", "UserGroup", "报修_维修单处理", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_维修单处理", "ProcedureStep", "维修工程师", "UserGroup", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("客户", "UserGroup", "报修_维修结果反馈", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("报修_维修结果反馈", "ProcedureStep", "客户", "UserGroup", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);


            newRelation = new RelationShip("客户", "UserGroup", "咨询_问题提交", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询_问题提交", "ProcedureStep", "客户", "UserGroup", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("客服人员", "UserGroup", "咨询_问题回复", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询_问题回复", "ProcedureStep", "客服人员", "UserGroup", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("客户", "UserGroup", "咨询_咨询结果反馈", "ProcedureStep", "InCharge", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("咨询_咨询结果反馈", "ProcedureStep", "客户", "UserGroup", "HandleBy", "1");
            DataReader.InsertRelationShip(newRelation);

            DataReader.AcceptModification();
        }

        private void BuildTaskType()
        {
            TaskType newType;
            newType = new TaskType("维修任务", 80, "用户申报维修任务。");
            DataReader.InsertRecord(new Record(newType.Name, newType.Type, newType.XMLSerialize()));
            newType = new TaskType("咨询任务", 60,"用户提出咨询。");
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

        private void BuildUser()
        {
            IfUser newUser;
            newUser = new Customer("Alice", "", "159-1111-1111", "Intel");
            DataReader.InsertRecord(new Record(newUser.Name, newUser.Type, newUser.XMLSerialize()));
            newUser = new Customer("Bob", "", "159-2222-2222", "MicroSoft");
            DataReader.InsertRecord(new Record(newUser.Name, newUser.Type, newUser.XMLSerialize()));

            newUser = new ServiceUser("Clare", "", "S00001");
            DataReader.InsertRecord(new Record(newUser.Name, newUser.Type, newUser.XMLSerialize()));
            newUser = new ServiceUser("Douglas", "", "S00002");
            DataReader.InsertRecord(new Record(newUser.Name, newUser.Type, newUser.XMLSerialize()));

            newUser = new Engineer("Euler", "", "S00003");
            DataReader.InsertRecord(new Record(newUser.Name, newUser.Type, newUser.XMLSerialize()));
            newUser = new Engineer("Frank", "", "S00004");

            DataReader.InsertRecord(new Record(newUser.Name, newUser.Type, newUser.XMLSerialize()));
            newUser = new Manager("Gloria", "", "S00005");
            DataReader.InsertRecord(new Record(newUser.Name, newUser.Type, newUser.XMLSerialize()));


            RelationShip newRelation;
            //Inferior
            newRelation = new RelationShip("Gloria", "Manager", "Clare", "ServiceUser", "Inferior", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Gloria", "Manager", "Douglas", "ServiceUser", "Inferior", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Gloria", "Manager", "Euler", "Engineer", "Inferior", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Gloria", "Manager", "Frank", "Engineer", "Inferior", "1");
            DataReader.InsertRelationShip(newRelation);

            //Include&Belong to
            newRelation = new RelationShip("客户", "UserGroup", "Alice", "Customer", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Alice", "Customer", "客户", "UserGroup", "BelongTo", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("客户", "UserGroup", "Bob", "Customer", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Bob", "Customer", "客户", "UserGroup", "BelongTo", "1");
            DataReader.InsertRelationShip(newRelation);

            newRelation = new RelationShip("客服人员", "UserGroup", "Clare", "ServiceUser", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Clare", "ServiceUser", "客服人员", "UserGroup", "BelongTo", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("客服人员", "UserGroup", "Douglas", "ServiceUser", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Douglas", "ServiceUser", "客服人员", "UserGroup", "BelongTo", "1");
            DataReader.InsertRelationShip(newRelation);

            newRelation = new RelationShip("维修工程师", "UserGroup", "Euler", "Engineer", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Euler", "Engineer", "维修工程师", "UserGroup", "BelongTo", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("维修工程师", "UserGroup", "Frank", "Engineer", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Frank", "Engineer", "维修工程师", "UserGroup", "BelongTo", "1");
            DataReader.InsertRelationShip(newRelation);

            newRelation = new RelationShip("客服经理", "UserGroup", "Gloria", "Manager", "Include", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Gloria", "Manager", "客服经理", "UserGroup", "BelongTo", "1");
            DataReader.InsertRelationShip(newRelation);

            DataReader.AcceptModification();
        }

        private void BuildTask()
        {
            ProcedureTask newPTask;
            CustomTask newCTask;
            RelationShip newRelation;


            //Q1////////////////////////////////////////////////////////////////////////////////////////////////////////////
            newPTask = new ProcedureTask("Question_1", DateTime.Now, DateTime.Now, "Qusestion1............");
            DataReader.InsertRecord(new Record(newPTask.Name, newPTask.Type, newPTask.XMLSerialize()));
            //Submitter&Submit
            newRelation = new RelationShip("Question_1", "ProcedureTask", "Alice", "Customer", "Submitter", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Alice", "Customer", "Question_1", "ProcedureTask", "Submit", "1");
            DataReader.InsertRelationShip(newRelation);
            //SetPriority
            newRelation = new RelationShip("Question_1", "ProcedureTask", "Q1", "QLevel", "SetPriority", "1");
            DataReader.InsertRelationShip(newRelation);
            //SetType
            newRelation = new RelationShip("Question_1", "ProcedureTask", "维修任务", "TaskType", "SetType", "1");
            DataReader.InsertRelationShip(newRelation);
            //CurrentStep
            newRelation = new RelationShip("Question_1", "ProcedureTask", "报修_问题审核", "ProcedureStep", "CurrentStep", "1");
            DataReader.InsertRelationShip(newRelation);
            //Handler&Handle
            newRelation = new RelationShip("Question_1", "ProcedureTask", "Clare", "ServiceUser", "Handler", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Clare", "ServiceUser", "Question_1", "ProcedureTask", "Handle", "1");
            DataReader.InsertRelationShip(newRelation);

            //Q2/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            newPTask = new ProcedureTask("Question_2", DateTime.Now, DateTime.Now, "Qusestion2............");
            DataReader.InsertRecord(new Record(newPTask.Name, newPTask.Type, newPTask.XMLSerialize()));
            //Submitter&Submit
            newRelation = new RelationShip("Question_2", "ProcedureTask", "Bob", "Customer", "Submitter", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Bob", "Customer", "Question_2", "ProcedureTask", "Submit", "1");
            DataReader.InsertRelationShip(newRelation);
            //SetPriority
            newRelation = new RelationShip("Question_2", "ProcedureTask", "Q2", "QLevel", "SetPriority", "1");
            DataReader.InsertRelationShip(newRelation);
            //SetType
            newRelation = new RelationShip("Question_2", "ProcedureTask", "维修任务", "TaskType", "SetType", "1");
            DataReader.InsertRelationShip(newRelation);
            //CurrentStep
            newRelation = new RelationShip("Question_2", "ProcedureTask", "报修_维修单处理", "ProcedureStep", "CurrentStep", "1");
            DataReader.InsertRelationShip(newRelation);
            //Handler&Handle
            newRelation = new RelationShip("Question_2", "ProcedureTask", "Euler", "Engineer", "Handler", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Euler", "Engineer", "Question_2", "ProcedureTask", "Handle", "1");
            DataReader.InsertRelationShip(newRelation);


            //PC1/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            newPTask = new ProcedureTask("Consulation_1", DateTime.Now, DateTime.Now, "Consulation_1............");
            DataReader.InsertRecord(new Record(newPTask.Name, newPTask.Type, newPTask.XMLSerialize()));
            //Submitter&Submit
            newRelation = new RelationShip("Consulation_1", "ProcedureTask", "Bob", "Customer", "Submitter", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Bob", "Customer", "Consulation_1", "ProcedureTask", "Submit", "1");
            DataReader.InsertRelationShip(newRelation);
            //SetPriority
            newRelation = new RelationShip("Consulation_1", "ProcedureTask", "Q3", "QLevel", "SetPriority", "1");
            DataReader.InsertRelationShip(newRelation);
            //SetType
            newRelation = new RelationShip("Consulation_1", "ProcedureTask", "咨询任务", "TaskType", "SetType", "1");
            DataReader.InsertRelationShip(newRelation);
            //CurrentStep
            newRelation = new RelationShip("Consulation_1", "ProcedureTask", "咨询_问题回复", "ProcedureStep", "CurrentStep", "1");
            DataReader.InsertRelationShip(newRelation);
            //Handler&Handle
            newRelation = new RelationShip("Consulation_1", "ProcedureTask", "Douglas", "ServiceUser", "Handler", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Douglas", "ServiceUser", "Consulation_1", "ProcedureTask", "Handle", "1");
            DataReader.InsertRelationShip(newRelation);



            //C1/////////////////////////////////////////////////////////////////////////////////////////////////////////////
            newCTask = new CustomTask("CustomTask_1", DateTime.Now, DateTime.Now, "CustomTask_1............");
            DataReader.InsertRecord(new Record(newCTask.Name, newCTask.Type, newCTask.XMLSerialize()));
            //Submitter&Submit
            newRelation = new RelationShip("CustomTask_1", "CustomTask", "Gloria", "Manager", "Submitter", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Gloria", "Manager", "CustomTask_1", "CustomTask", "Submit", "1");
            DataReader.InsertRelationShip(newRelation);
            //SetPriority
            newRelation = new RelationShip("CustomTask_1", "CustomTask", "Q2", "QLevel", "SetPriority", "1");
            DataReader.InsertRelationShip(newRelation);
            //SetType
            TaskType newType = new TaskType("填报工时", 30, "填报本月工时");
            DataReader.InsertRecord(new Record(newType.Name, newType.Type, newType.XMLSerialize()));
            newRelation = new RelationShip("CustomTask_1", "CustomTask", "填报工时", "TaskType", "SetType", "1");
            DataReader.InsertRelationShip(newRelation);
            //Handler&Handle
            newRelation = new RelationShip("CustomTask_1", "CustomTask", "Frank", "Engineer", "Handler", "1");
            DataReader.InsertRelationShip(newRelation);
            newRelation = new RelationShip("Frank", "Engineer", "CustomTask_1", "CustomTask", "Handle", "1");
            DataReader.InsertRelationShip(newRelation);

            DataReader.AcceptModification();
        }

        #endregion
    }
}
