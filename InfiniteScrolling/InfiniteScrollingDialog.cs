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

        int startNow = 1;
        int endRow = 100;
        int previousFirstDisplayedScrollingRowIndex = 0;

        public InfiniteScrollingDialog()
        {
            InitializeComponent();
        }

        public void LoadDataView()
        {
            dataTable = GetDataFromTo(startNow, endRow);
            this.gridNote.DataSource = dataTable;
        }

        private DataTable GetDataFromTo(int startRow, int endRow)
        {
            string commandString = @"SELECT ROW_NUMBER() OVER(ORDER BY N.CreatedDate DESC) AS RowIndex, N.Note, N.CreatedDate
	                                INTO #TMPthi
		                                FROM Note n (NOLOCK)
	                                WHERE n.CreatedDate >= '1/1/2020'
	                                ORDER BY n.CreatedDate DESC

	                                SELECT *
	                                FROM #TMP T
	                                WHERE T.RowIndex >= {0} AND T.RowIndex <= {1}
                                    
                                    DROP TABLE #TMP";

            commandString = string.Format(commandString, startRow, endRow);

            sqlDataAdapter = new SqlDataAdapter(commandString, connection);
            dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
             
            return dataSet.Tables[0];
        }

        private void InfiniteScrollingDialog_Load(object sender, EventArgs e)
        {
            LoadDataView();
        }
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
    }
}
