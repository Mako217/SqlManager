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
                //If the server type is MSSQL Server, get all table names using SqlClient
                SqlConnection connection = new SqlConnection(ConnectionString + $"Initial Catalog={databaseDialog.options[whichDatabase]};");
                SqlDataAdapter adapter = new SqlDataAdapter("Select table_name From  information_schema.tables;", connection);
                connection.Open();
                //Pass all the data to DataTable
                adapter.Fill(dataTable);
                //Add every table name in DataTable to the option list
                for(int i = 0; i<dataTable.Rows.Count; i++)
                {
                    DataRow row = dataTable.Rows[i];
                    Add((string)row.ItemArray[0]);
                }
                connection.Close();
                adapter.Dispose();
                dataTable.Dispose();
                //Add the rest of the options to the list
                Add("--Add new Table--");
                Add("--Delete Database--");
                Add("--Return--");
            }
            else if(ServerType == "PostgreSQL")
            {
                //If the server type is PostgreSQL, get all table names using Npgsql
                NpgsqlConnection connection = new NpgsqlConnection(ConnectionString + $"Database={databaseDialog.options[whichDatabase]};");
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("Select table_name From  information_schema.tables where table_schema = 'public';", connection);
                connection.Open();
                //Pass all the data to DataTable
                adapter.Fill(dataTable);
                //Add every table name in DataTable to the option list
                for(int i = 0; i<dataTable.Rows.Count; i++)
                {
                    DataRow row = dataTable.Rows[i];
                    Add((string)row.ItemArray[0]);
                }
                connection.Close();
                adapter.Dispose();
                dataTable.Dispose();
                //Add the rest of the options to the list
                Add("--Add new Table--");
                Add("--Delete Database--");
                Add("--Return--");
            }

            
        }
        public void Start(DatabaseDialog databaseDialog, int whichDatabase, TableOptions tableOptions)
        {
            //At start update list of options
            UpdateList(databaseDialog, whichDatabase);
            //Start the Control() method and set returned value as whichTable
            int whichTable = Control();
            if(whichTable == options.Count - 3)
            {
                //If --Add new Table-- is selected, start adding new table
                TableAdder.Add(ConnectionString, ServerType, databaseDialog, whichDatabase, this, tableOptions);
            }
            else if(whichTable == options.Count-2)
            {  
                //If --Delete Database-- is selected start deleting currently selected database
                DatabaseDeleter.Delete(ConnectionString, ServerType, databaseDialog, whichDatabase, this, tableOptions);
            }
            else if(whichTable == options.Count - 1)
            {
                //If --Return-- is selected, return to databaseDialog
                databaseDialog.Start(this, tableOptions);
            }
            else
            {
                //If a table is selected, open list of options for this table
                tableOptions.Start(databaseDialog, whichDatabase, this, whichTable);
            }
        }

    }
}