using System;
using System.Data.SqlClient;
using Npgsql;

namespace ClassLibrary
{
    public static class ColumnAdder
    {
        public static void Add(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions)
        {
            Console.Clear();
            //Ask user for column name and data type
            Console.WriteLine("Enter column name:");
            string columnName = Console.ReadLine();
            Console.WriteLine("Enter column data type:");
            string dataType = Console.ReadLine();
            if(serverType == "MSSQL Server")
            {
                //If server type is MSSQL Server add new column using SqlClient
                SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog = {databaseDialog.options[whichDatabase]}");
                SqlCommand command = new SqlCommand($"Alter table {tableDialog.options[whichTable]} add {columnName} {dataType};", connection);
                connection.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            else if(serverType == "PostgreSQL")
            {
                //If server type is PostgreSQL add new column using Npgsql
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database = {databaseDialog.options[whichDatabase]}");
                NpgsqlCommand command = new NpgsqlCommand($"Alter table {tableDialog.options[whichTable]} add {columnName} {dataType};", connection);
                connection.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            tableOptions.Start(databaseDialog, whichDatabase, tableDialog, whichTable);
        }
        
    }
}