using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ExportBJ_XML.classes
{
    class PeriodTreeLoader
    {
        private int _IDZ_Start;
        private SqlDataAdapter _da;
        private DataSet _ds;
        private SqlConnection _con;
        public TreeNode _rootNode;
        private List<TreeNode> _yearsNodes;
        private bool _isYearsStarted = false;//в дерево начали подгружаться года издания
        public PeriodTreeLoader(int IDZ_Start, SqlConnection con)
        {
            this._IDZ_Start = IDZ_Start;
            this._con = con;
            _ds = new DataSet();
            _da = new SqlDataAdapter();
            _da.SelectCommand = new SqlCommand();
            _da.SelectCommand.Connection = con;
            _da.SelectCommand.Parameters.Add("IDZ", SqlDbType.Int);

            //загружаем корневой узел
            _da.SelectCommand.Parameters["IDZ"].Value = IDZ_Start;
            _da.SelectCommand.CommandText = "select IDZ,P.VNIZ,POLE,NAZV,PREFIX"
            + " FROM PERIOD..[PI] as P  "
            + " LEFT JOIN PERIOD..FIELDSK AS F ON F.IDF=P.IDF where IDZ = @IDZ";
            _da.Fill(_ds, "tn");
            _rootNode = new TreeNode();
            _rootNode.Text = _ds.Tables["tn"].Rows[0]["NAZV"].ToString() + ": " + _ds.Tables["tn"].Rows[0]["POLE"].ToString();
            _rootNode.Tag = IDZ_Start;

            _yearsNodes = new List<TreeNode>();

        }


        public void LoadChilds(int IDZ_, TreeNode main)
        //вариант когда года после сортировки встанут в конце. мне он больше нравится потому что он универсальнее. он сработает даже если между годами будет другой узел, но года станут в конце
        {
            load(IDZ_, main);

            //после загрузки всех узлов сортируем и добавляем года
            SortYearsDescending c = new SortYearsDescending();
            _yearsNodes.Sort(c);
            foreach (TreeNode n in _yearsNodes)
            {
                main.Nodes.Add(n);
            }
            _yearsNodes = new List<TreeNode>();

            

        }

        private void load(int IDZ_, TreeNode main)
        {
            TreeNode add = new TreeNode();
            int IDZ = IDZ_;
            _da.SelectCommand.Parameters["IDZ"].Value = IDZ;
            _da.SelectCommand.CommandText = "select IDZ,P.VNIZ,POLE,NAZV,PREFIX,P.VPRAVO "
            + " FROM PERIOD..[PI] as P  "
            + " LEFT JOIN PERIOD..FIELDSK AS F ON F.IDF=P.IDF where P.VVERH=@IDZ";
            _ds = new DataSet();
            int cnt = _da.Fill(_ds, "tn");
            foreach (DataRow row in _ds.Tables["tn"].Rows)
            {
                string POLE = row["POLE"].ToString();
                if (POLE == "") add.ForeColor = Color.Blue;
                add.Text = row["NAZV"].ToString() + ": " + POLE;
                add.Tag = (int)row["IDZ"];


                if ((row["NAZV"].ToString() == "Год издания"))
                {
                    _yearsNodes.Add(add);//узлы годов отдельно копим, чтобы их в конце воткнуть
                }
                else
                {
                    main.Nodes.Add(add);
                }

                int CurrentIDZ = (int)row["IDZ"];
                if (CurrentIDZ != 0)
                {
                    this.LoadChilds(CurrentIDZ, add);//рекурсивно вызываем до тех пор пока не будет соседних узлов
                }
            }
        }

        public int GetVNIZ(int IDZ)//получить VNIZ для заданного IDZ
        {
            _da.SelectCommand.Parameters["IDZ"].Value = IDZ;
            _da.SelectCommand.CommandText = "select IDZ,P.VNIZ,POLE,NAZV,PREFIX,P.VPRAVO "
            + " FROM PERIOD..[PI] as P "
            + " LEFT JOIN PERIOD..FIELDSK AS F ON F.IDF=P.IDF where IDZ=@IDZ";
            _ds = new DataSet();
            _da.Fill(_ds, "tn");
            return (int)_ds.Tables["tn"].Rows[0]["VNIZ"];
        }
        private class SortYearsDescending : IComparer<TreeNode>
        {
            public SortYearsDescending()
            { }


            public int Compare(TreeNode x, TreeNode y)
            {
                int xi, yi;
                if ((x == null) && (y == null))
                    return 0;
                if ((x != null) && (y == null))
                    return -1;
                if ((x == null) && (y != null))
                    return 1;
                if ((x != null) && (y != null))
                {
                    //x.Text и y.Text никогда не будут null потому что у них уже есть заглавие поля, нужно только узнать пустой ли год или нет
                    if ((x.Text.Substring(x.Text.IndexOf(":") + 2).Length < 4) && (y.Text.Substring(x.Text.IndexOf(":") + 2).Length < 4))
                        return 0;//если ни в том ни в другом нет года
                    if ((x.Text.Substring(x.Text.IndexOf(":") + 2).Length >= 4) && (y.Text.Substring(x.Text.IndexOf(":") + 2).Length < 4))
                        return -1;//если в x есть год а в y нет
                    if ((x.Text.Substring(x.Text.IndexOf(":") + 2).Length < 4) && (y.Text.Substring(x.Text.IndexOf(":") + 2).Length >= 4))
                        return 1;//наоборот
                }

                xi = int.Parse(x.Text.Substring(x.Text.IndexOf(":") + 2));
                yi = int.Parse(y.Text.Substring(y.Text.IndexOf(":") + 2));

                if (xi > yi)
                    return -1;//по убыванию
                else
                    if (yi == xi)
                        return 0;
                    else
                        return 1;
            }
        }

        internal void WriteTreeJSON(JsonWriter writer, TreeNode root)
        {
            foreach (TreeNode node in root.Nodes)
            {
                writer.WriteStartObject();
                string fieldName = node.Text.Substring(0, node.Text.IndexOf(":"));
                string fieldValue = node.Text.Substring(node.Text.IndexOf(":")+1);
                
                writer.WritePropertyName("id");
                writer.WriteValue(node.Tag.ToString());

                writer.WritePropertyName("parent");
                writer.WriteValue((node.Parent == null) ? "#" : node.Parent.Tag.ToString());

                writer.WritePropertyName("text");
                writer.WriteValue(node.Text);
                writer.WriteEndObject();
                this.WriteTreeJSON(writer, node);
            }
        }
    }

}
