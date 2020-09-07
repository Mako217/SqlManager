using System;
using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace ClassLibrary
{
    public class DatabaseDialog : Dialog
    {
        public DatabaseDialog(string connectionString, string serverType) : base(connectionString, serverType)
        {
            ServerType = serverType;
            ConnectionString = connectionString;
            UpdateList();            
        }
        private void UpdateList()
        {
            options.Clear();
            if(ServerType == "MSSQL Server")
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT name from master.dbo.sysdatabases WHERE dbid > 4", connection);
                connection.Open();
                DataTable dataTable = new DataTable();
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
            else if(ServerType == "PostgreSQL")
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("SELECT datname FROM pg_database WHERE NOT datname = 'template0' AND NOT datname = 'template1'", connection);
                connection.Open();
                DataTable dataTable = new DataTable();
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
            
        }
        public void Start(TableDialog tableDialog, TableOptions tableOptions)
        {
            UpdateList();
            int whichDatabase = Control();
            if(whichDatabase == options.Count-1)
                {
                    DatabaseAdder.Add(ConnectionString, ServerType, this, tableDialog, tableOptions);
                }
                else
                {
                    tableDialog.Start(this, whichDatabase, tableOptions);
                }
        }
        
    }
}