using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InfiniteScrolling
{
    public partial class InfiniteScrollingDialog : Form
    {
        SqlConnection connection = new SqlConnection("Data Source=172.16.1.30;Initial Catalog=BOSS_Development;User Id=boss_dev; Password=GoodDev!");
        int ThresholdRows = 20; //It should get from config file
        
        public InfiniteScrollingDialog()
        {
            InitializeComponent();
        }

        public virtual DataTable LoadDataForGrid(int startRow, int endRow, ref string commandStr)
        {
            DataTable resultDB = new DataTable();

            commandStr = "GetNoteList_TEST_1";

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

            this.gridNote.DataSource = LoadDataForGrid(1, ThresholdRows, ref commandStr);

            //Configures to get more data
            this.gridNote.InfiniteScrollingConfigs = new InfiniteScrollingConfigs
            {
                StoreProcedure = commandStr,
                ConnetionStringDefault = connection.ConnectionString.ToString(),
                ArchivedConnetionString = connection.ConnectionString.ToString(),
                MoreRowsCount = this.ThresholdRows,
                MaxRowsFromBOSS = CountMaxRowsFromBOSS()
            };
        }

        private int CountMaxRowsFromBOSS()
        {
            int countRows;
            string query = @"SELECT COUNT(*) CountRows FROM Note n (NOLOCK) WHERE n.CreatedDate >= '8/20/2020'";
            using (SqlConnection sqlConnection = new SqlConnection(connection.ConnectionString.ToString()))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    countRows = int.Parse(sqlCommand.ExecuteScalar().ToString());
                }

                sqlConnection.Close();
            }

            return countRows;
        }
    }
}
