using System;
using System.Data.SqlClient;
using Npgsql;

namespace ClassLibrary
{
    public static class TableDeleter
    {
        public static void Delete(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions)
        {
            Console.Clear();
            Console.WriteLine("Are you sure, you want to delete this table? [Type Y for Yes, or N for No, N is default]");
            string input = Console.ReadLine();
            if(input == "Y")
            {
                
            }
            else if(input == "N" || input == "")
            {
                tableOptions.Start(databaseDialog, whichDatabase, tableDialog, whichTable);
            }
            else
            {
                TableDeleter.Delete(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions);
            }
            if(serverType == "MSSQL Server")
            {
                SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog ={databaseDialog.options[whichDatabase]};");
                connection.Open();
                SqlCommand command = new SqlCommand($"Drop table {tableDialog.options[whichTable]}", connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            else if(serverType == "PostgreSQL")
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database ={databaseDialog.options[whichDatabase]};");
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"Drop table {tableDialog.options[whichTable]}", connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            tableDialog.Start(databaseDialog, whichDatabase, tableOptions);
        }
    }
}