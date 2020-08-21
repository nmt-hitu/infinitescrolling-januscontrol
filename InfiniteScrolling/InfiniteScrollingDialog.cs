using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InfiniteScrolling
{
    public partial class InfiniteScrollingDialog : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=172.16.1.30;Initial Catalog=BOSS_Development;User Id=boss_dev; Password=GoodDev!");
        DataTable dataTable = new DataTable();
        DataSet dataSet;
        SqlDataAdapter sqlDataAdapter;

        public InfiniteScrollingDialog()
        {
            InitializeComponent();
        }

        public virtual DataTable LoadMoreRows(int startRow, int endRow, ref string commandStr)
        {
            DataTable resultDB = new DataTable();

            commandStr = "GetNoteList_TEST";

            using (SqlConnection sqlConnection = new SqlConnection(connection.ConnectionString.ToString()))
            {
                using (SqlCommand sqlCommand = new SqlCommand(commandStr, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.Add("@StartRow", SqlDbType.Int).Value = startRow;
                    sqlCommand.Parameters.Add("@EndRow", SqlDbType.Int).Value = endRow;

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    DataSet dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet);

                    if (dataSet != null && dataSet.Tables.Count > 0)
                    {
                        resultDB = dataSet.Tables[0];
                    }
                }
            }

            return resultDB;
        }

        private void InfiniteScrollingDialog_Load(object sender, EventArgs e)
        {
            string commandStr = string.Empty;

            this.gridNote.DataSource = LoadMoreRows(1, 20, ref commandStr);

            //Configures to get more data
            this.gridNote.InfiniteScrollingConfigs = new InfiniteScrollingConfigs
            {
                StoreProcedure = commandStr,
                ConnetionString = connection.ConnectionString.ToString(),
                MoreRowsCount = 10
            };
        }

        /*
        private void GridNote_Scroll(object sender, EventArgs e)
        {
            if (this.gridNote.VerticalScrollPosition > this.previousFirstDisplayedScrollingRowIndex
                && this.gridNote.LastVisibleRow(true) == this.gridNote.RowCount - 1)
            {
                //Load more
                DataTable newData = new DataTable();

                int latestStartRow = (startNow * endRow) + 1;
                int latestEndRow = (startNow + 1) * endRow;

                newData = GetDataFromTo(latestStartRow, latestEndRow);
                dataTable.Merge(newData);
                this.gridNote.DataSource = dataTable;
                this.gridNote.VerticalScrollPosition = latestStartRow;
                this.previousFirstDisplayedScrollingRowIndex = this.gridNote.VerticalScrollPosition;

                startNow++;
            }
        }
        */
    }
}
