using Janus.Windows.GridEX;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteScrolling
{
    public class InfiniteScrolling : GridEX
    {
        private int previousFirstDisplayedScrollingRowIndex = 0;
        private int totalRow;
        private bool isBinding = false;
        private int latestStartRow = 0;
        private int startRow = 1;
        protected override void OnScroll(EventArgs e)
        {
            base.OnScroll(e);

            if (isBinding)
                return;

            totalRow = this.RowCount;

            if (!isBinding && this.VerticalScrollPosition > this.previousFirstDisplayedScrollingRowIndex
                && this.LastVisibleRow(true) == totalRow - 1)
            {
                isBinding = true;

                //Get more data
                latestStartRow = (startRow * this.InfiniteScrollingConfigs.MoreRowsCount) + 1;
                int latestEndRow = (startRow + 1) * this.InfiniteScrollingConfigs.MoreRowsCount;

                DataTable moreRows = new DataTable();
                moreRows = LoadMoreRows(latestStartRow, latestEndRow);
                this.DataSource.Merge(moreRows);

                this.previousFirstDisplayedScrollingRowIndex = this.VerticalScrollPosition;
                this.VerticalScrollPosition = latestStartRow;
                this.startRow++;

                isBinding = false;
            }
        }

        public new virtual DataTable DataSource
        {
            get
            {
                return base.DataSource as DataTable;
            }
            set
            {
                base.DataSource = value;
            }
        }

        public virtual InfiniteScrollingConfigs InfiniteScrollingConfigs { get; set; }

        public virtual DataTable LoadMoreRows(int startRow, int endRow)
        {
            DataTable resultDB = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(InfiniteScrollingConfigs.ConnetionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(InfiniteScrollingConfigs.StoreProcedure, sqlConnection))
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

    }

    public class InfiniteScrollingConfigs
    {
        public string StoreProcedure { get; set; }
        public string ConnetionString { get; set; }
        public int MoreRowsCount { get; set; }
    }
}
