using LibflClassLibrary.BJUsers;
using LibflClassLibrary.Circulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CirculationApp
{
    public partial class TableDataVisualizer : Form
    {
        private fDatePeriod dp_;
        private CirculationStatisticsManager csm_ = new CirculationStatisticsManager();
        private BJUserInfo bjUser_;


        public TableDataVisualizer()
        {
            InitializeComponent();
        }

        public TableDataVisualizer(fDatePeriod dp, BJUserInfo bjUser)
        {
            this.dp_ = dp;
            bjUser_ = bjUser;
            InitializeComponent();
            HallService();
        }

        private void HallService()
        {
            this.Text = $"Обслуживание в залах за период с {dp_.StartDate.ToString("dd.MM.yyyy")} по {dp_.EndDate.ToString("dd.MM.yyyy")}";

            int BooksIssuedFromHallCount = csm_.GetBooksIssuedFromHallCount(dp_.StartDate, dp_.EndDate, bjUser_);
            int BooksIssuedFromBookKeepingCount = csm_.GetBooksIssuedFromBookkeepingCount(dp_.StartDate, dp_.EndDate, bjUser_);
            //int BooksIssuedFromReserveCount = csm_.GetBooksIssuedFromReserveCount(dp_.StartDate, dp_.EndDate, bjUser_);
            
            KeyValuePair<string, string>[] columns =
            {
                new KeyValuePair<string, string> ( "id" , "п.п." ),
                new KeyValuePair<string, string> ( "name", "Наименование показателя"),
                new KeyValuePair<string, string> ( "value", "Значение показателя"),
            };
            dgViewer.Columns.Clear();
            foreach (var c in columns)
                dgViewer.Columns.Add(c.Key, c.Value);

            dgViewer.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgViewer.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            dgViewer.Columns["id"].Width = 50;
            dgViewer.Columns["name"].Width = 200;
            dgViewer.Columns["value"].Width = 100;

            dgViewer.Rows.Add();
            var row = dgViewer.Rows[dgViewer.Rows.Count - 1];
            row.Cells["id"].Value = 1;
            row.Cells["name"].Value = "Выдано из зала";
            row.Cells["value"].Value = BooksIssuedFromHallCount;

            dgViewer.Rows.Add();
            row = dgViewer.Rows[dgViewer.Rows.Count - 1];
            row.Cells["id"].Value = 2;
            row.Cells["name"].Value = "Выдано из книгохранения";
            row.Cells["value"].Value = BooksIssuedFromBookKeepingCount;



        }


    }
}
