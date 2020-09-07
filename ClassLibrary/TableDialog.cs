using System;
using System.Data.SqlClient;
using System.Data;
using Npgsql;

namespace ClassLibrary
{
    public class TableDialog : Dialog
    {
        public TableDialog(string connectionString, string serverType) : base(connectionString, serverType)
        {
            ConnectionString = connectionString;
            ServerType = serverType;    
        }

        private void UpdateList(DatabaseDialog databaseDialog, int whichDatabase)
        {
            options.Clear();
            DataTable dataTable = new DataTable();
            if(ServerType == "MSSQL Server")
            {
                SqlConnection connection = new SqlConnection(ConnectionString + $"Initial Catalog={databaseDialog.options[whichDatabase]};");
                SqlDataAdapter adapter = new SqlDataAdapter("Select table_name From  information_schema.tables;", connection);
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
                Add("--Add new Table--");
                Add("--Delete Database--");
                Add("--Return--");
            }
            else if(ServerType == "PostgreSQL")
            {
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString + $"Database={databaseDialog.options[whichDatabase]};");
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("Select table_name From  information_schema.tables where table_schema = 'public';", connection);
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
                Add("--Add new Table--");
                Add("--Delete Database--");
                Add("--Return--");
            }

            
        }
        public void Start(DatabaseDialog databaseDialog, int whichDatabase, TableOptions tableOptions)
        {
            UpdateList(databaseDialog, whichDatabase);
            int whichTable = Control();
            if(whichTable == options.Count - 3)
            {
                TableAdder.Add(ConnectionString, ServerType, databaseDialog, whichDatabase, this, tableOptions);
            }
            else if(whichTable == options.Count-2)
            {  
                DatabaseDeleter.Delete(ConnectionString, ServerType, databaseDialog, whichDatabase, this, tableOptions);
            }
            else if(whichTable == options.Count - 1)
            {
                databaseDialog.Start(this, tableOptions);
            }
            else
            {
                tableOptions.Start(databaseDialog, whichDatabase, this, whichTable);
            }
        }

    }
}