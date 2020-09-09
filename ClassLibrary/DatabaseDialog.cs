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
                //If server type is MSSQL Server, create SqlConnection and SqlDataAdapter to get list of databases
                SqlConnection connection = new SqlConnection(ConnectionString);
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT name from master.dbo.sysdatabases WHERE dbid > 4", connection);
                connection.Open();
                //Pass the data into DataTable
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                //Add every database name in the DataTable to the options list
                for(int i = 0; i<dataTable.Rows.Count; i++)
                {
                    DataRow row = dataTable.Rows[i];
                    Add((string)row.ItemArray[0]);
                }
                connection.Close();
                adapter.Dispose();
                dataTable.Dispose();
                connection.Dispose();
                //Add option --Create new Database-- to the options list
                options.Add("--Create new Database--");
            }
            else if(ServerType == "PostgreSQL")
            {
                //If server type is PostgreSQL, create NpgsqlConnection and NpgsqlDataAdapter to get list of databases
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("SELECT datname FROM pg_database WHERE NOT datname = 'template0' AND NOT datname = 'template1'", connection);
                connection.Open();
                //Pass the data into DataTable
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                //Add every database name in the DataTable to the options list
                for(int i = 0; i<dataTable.Rows.Count; i++)
                {
                    DataRow row = dataTable.Rows[i];
                    Add((string)row.ItemArray[0]);
                }
                connection.Close();
                adapter.Dispose();
                dataTable.Dispose();
                connection.Dispose();
                //Add option --Create new Database-- to the options list
                options.Add("--Create new Database--");
            }
            
        }
        public void Start(TableDialog tableDialog, TableOptions tableOptions)
        {
            //Update list of databases when started
            UpdateList();
            //Start the Control() method, and set the returned value as whichDatabase
            int whichDatabase = Control();
            if(whichDatabase == options.Count-1)
                {
                    //If --Create new Database-- is selected, start adding new database
                    DatabaseAdder.Add(ConnectionString, ServerType, this, tableDialog, tableOptions);
                }
                else
                {
                    //If database is selected, start tableDialog, passing which database was selected
                    tableDialog.Start(this, whichDatabase, tableOptions);
                }
        }
        
    }
}