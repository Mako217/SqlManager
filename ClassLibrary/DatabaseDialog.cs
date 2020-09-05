using System;
using System.Data;
using System.Data.SqlClient;

namespace ClassLibrary
{
    public class DatabaseDialog : Dialog
    {
        public DatabaseDialog(string connectionString) : base(connectionString)
        {
            ConnectionString = connectionString;
            UpdateList();            
        }
        private void UpdateList()
        {
            options.Clear();
            DataTable dataTable = new DataTable();
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT name from master.dbo.sysdatabases WHERE dbid > 4", connection);
            connection.Open();
            adapter.Fill(dataTable);
            for(int i = 0; i<dataTable.Rows.Count; i++)
            {
                DataRow row = dataTable.Rows[i];
                Add((string)row.ItemArray[0]);
            }
            connection.Close();
            adapter.Dispose();
            dataTable.Dispose();
            connection.Dispose();
            options.Add("--Create new Database--");
        }
        public void Start(TableDialog tableDialog, TableOptions tableOptions)
        {
            UpdateList();
            int whichDatabase = Control();
            if(whichDatabase == options.Count-1)
                {
                    DatabaseAdder.Add(ConnectionString, this, tableDialog, tableOptions);
                }
                else
                {
                    tableDialog.Start(this, whichDatabase, tableOptions);
                }
        }
        
    }
}