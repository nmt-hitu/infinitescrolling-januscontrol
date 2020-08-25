using Janus.Windows.GridEX;
using System;
using System.Data;
using System.Data.SqlClient;

namespace InfiniteScrolling
{
    public class InfiniteScrolling : GridEX
    {
        private int previousFirstDisplayedScrollingRowIndex = 0;
        private int totalRow;
        private bool isBinding = false;
        private int latestStartRow = 0;

        private int startRow = 1;
        private int latestStartRowSnapshot = 0;
        private int newEndRow = 0;
        private int newStartRow = 0;
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
                latestStartRow = newStartRow > 0 ?
                    newStartRow + 1
                   : (startRow * this.InfiniteScrollingConfigs.MoreRowsCount) + 1;

                int latestEndRow = newEndRow > 0 ?
                    ((startRow + 1) * this.InfiniteScrollingConfigs.MoreRowsCount) - (newEndRow * startRow)
                    : ((startRow + 1) * this.InfiniteScrollingConfigs.MoreRowsCount);

                DataTable moreRows = new DataTable();

                if (totalRow <= this.InfiniteScrollingConfigs.MaxRowsFromBOSS)
                {
                    moreRows = LoadMoreRows(latestStartRow, latestEndRow, false);

                    if (moreRows.Rows.Count < InfiniteScrollingConfigs.MoreRowsCount)
                    {
                        //get more data from sencond database if the result rows does not equal defined rows.
                        DataTable moreRowsFromSecondDB = new DataTable();

                        //Reset start and end row value for new database
                        latestStartRowSnapshot = latestStartRow;
                        startRow = 0;
                        newEndRow = this.InfiniteScrollingConfigs.MoreRowsCount - moreRows.Rows.Count;
                        
                        //Set start value manually by the last row for the next scroll
                        newStartRow = newEndRow;

                        moreRowsFromSecondDB = LoadMoreRows(startRow, newEndRow, true);
                        moreRows.Merge(moreRowsFromSecondDB);
                    }
                }
                else
                {
                    //Set start value manually by the last row for the next scroll
                    newStartRow = latestEndRow;
                    moreRows = LoadMoreRows(latestStartRow, latestEndRow, true);
                }

                this.DataSource.Merge(moreRows);

                this.previousFirstDisplayedScrollingRowIndex = this.VerticalScrollPosition;
                this.VerticalScrollPosition = latestStartRow == latestStartRowSnapshot ? latestStartRow : (latestStartRow + latestStartRowSnapshot);
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

        public DataTable LoadMoreRows(int startRow, int endRow, bool switchDB = false)
        {
            DataTable resultDB = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(switchDB ? InfiniteScrollingConfigs.ArchivedConnetionString : InfiniteScrollingConfigs.ConnetionStringDefault))
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
        public string ConnetionStringDefault { get; set; }
        public string ArchivedConnetionString { get; set; }
        public int MoreRowsCount { get; set; }
        public int MaxRowsFromBOSS { get; set; }
    }
}
