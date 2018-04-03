using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace UpDgtPeriodic
{

    public partial class fMain : Form
    {
        private string Login;
        public fMain(string IDUSER)
        {
            InitializeComponent();
            this.Login = IDUSER;
            //trDgtPeriodic.Nodes.Add("1");
           // trDgtPeriodic.Nodes.Add("2");
            //trDgtPeriodic.Nodes.Add("3");
          //  trDgtPeriodic.Nodes.Add("4");
            ShowAdded();
        }
        private TreeNode root = new TreeNode("Оцифрованные издания периодики");
        public void ShowAdded()
        {
            trDgtPeriodic.Nodes.Clear();
            root = new TreeNode("Оцифрованные издания периодики");
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection(XMLConnections.XmlConnections.GetConnection("/Connections/SCANINFO"));
            da.SelectCommand.CommandText = "select * from BookAddInf..DgtPeriodic";
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");
            
            foreach (DataRow r in ds.Tables["t"].Rows)
            {
                if (NodeExistsInTreeView(r["PIN"].ToString()))
                {
                    TreeNode tmp = GetNodeByPIN(r["PIN"].ToString());
                    TreeNode YearNode = tmp.Nodes.Add(r["YEAR"].ToString());
                    YearNode.ToolTipText = r["PACKAGE"].ToString();

                }
                else
                {
                    string title = GetTitleByPIN(r["PIN"].ToString());
                    TreeNode NodeTitle = new TreeNode();
                    NodeTitle.Text = title;
                    NodeTitle.Tag = r["PIN"].ToString();
                    TreeNode YearNode = NodeTitle.Nodes.Add(r["YEAR"].ToString());
                    YearNode.ToolTipText = r["PACKAGE"].ToString();
                    
                    root.Nodes.Add(NodeTitle);
                    
                }
            }

            trDgtPeriodic.Nodes.Add(root);
            trDgtPeriodic.Sort();
            root.Expand();
            
        }

        private string GetTitleByPIN(string p)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = new SqlCommand();
            da.SelectCommand.Connection = new SqlConnection(XMLConnections.XmlConnections.GetConnection("/Connections/PERIOD"));
            da.SelectCommand.Parameters.Add("PIN", SqlDbType.NVarChar);
            da.SelectCommand.Parameters["PIN"].Value = p;
            da.SelectCommand.CommandText = "with F0 as "+
                                            "( " +
                                            "select A.* from PERIOD..[PI] A "+
                                            "where IDF = 120 and POLE = @PIN "+
                                            "), "+
                                            "FA as "+
                                            "( "+
                                            "select * from PERIOD..[PI] where VVERH = (select IDZ from F0) "+
                                            ")"+
                                            "select * from FA where IDF = 121";
            DataSet ds = new DataSet();
            int i = da.Fill(ds, "t");
            return ds.Tables["t"].Rows[0]["POLE"].ToString();
        }



        private TreeNode GetNodeByPIN(string p)
        {
            foreach (TreeNode n in root.Nodes)
            {
                if (n.Tag.ToString() == p)
                {
                    return n;
                }
            }
            return null;
        }

        private bool NodeExistsInTreeView(string p)
        {
            foreach (TreeNode n in root.Nodes)
            {
                if (n.Tag == null) continue;
                if (n.Tag.ToString() == p)
                {
                    return true;
                }
            }
            return false;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void JPGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAddJPGChoice fAddJPG = new fAddJPGChoice(this.Login, false);
            fAddJPG.ShowDialog();
            ShowAdded();
        }

        private void PDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAddJPGChoice fAddJPG = new fAddJPGChoice(this.Login, true);
            fAddJPG.ShowDialog();
            ShowAdded();
        }

        ToolTip tt = new ToolTip();
        private void trDgtPeriodic_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            
            //tt.IsBalloon = true;
            //int i = tt.AutomaticDelay;
            
            //tt.AutoPopDelay = 50000;
            //tt.ShowAlways = true;
            
            if (!string.IsNullOrEmpty(e.Node.ToolTipText))
            {
                tt.Show(e.Node.ToolTipText, trDgtPeriodic,30000);
            }
        }
    }
}
