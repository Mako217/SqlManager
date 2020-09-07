using System;
using System.Data.SqlClient;
using Npgsql;

namespace ClassLibrary
{
    public static class DatabaseAdder
    {
        public static void Add(string connectionString, string serverType, DatabaseDialog databaseDialog, TableDialog tableDialog, TableOptions tableOptions)
        {
            Console.Clear();
            Console.WriteLine("Enter new database name:");
            string databaseName = Console.ReadLine();
            if(serverType == "MSSQL Server")
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand($"CREATE DATABASE {databaseName};",connection);
                connection.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            else if(serverType == "PostgreSQL")
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionString);
                NpgsqlCommand command = new NpgsqlCommand($"CREATE DATABASE {databaseName};",connection);
                connection.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
                   

            databaseDialog.Start(tableDialog, tableOptions);

        }
    }
}