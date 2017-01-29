using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using GraphDB;
using TaskChainLib.Model;

namespace TaskChainLib
{
    public partial class FrmMain : Form
    {
        GraphDataBase TaskChainDB;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            ErrorCode err = ErrorCode.NoError;
            TaskChainDB = new GraphDataBase();
            TaskChainDB.OpenDataBase("1.xml", ref err);

            string strFile = "c://task.xml";
            Task newTask = new Task();
            //IFormatter formatter = new Binary
            using (FileStream fs = new FileStream(strFile, FileMode.Create))
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Task));
                formatter.Serialize(fs, newTask);
            }



        }
    }
}
