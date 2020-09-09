using System;
using System.Data.SqlClient;
using Npgsql;

namespace ClassLibrary
{
    public static class DatabaseAdder
    {
        public static void Add(string connectionString, string serverType, DatabaseDialog databaseDialog, TableDialog tableDialog, TableOptions tableOptions)
        {
            //Ask user for the name of new database
            Console.Clear();
            Console.WriteLine("Enter new database name:");
            string databaseName = Console.ReadLine();
            if(serverType == "MSSQL Server")
            {
                //If server type is MSSQL Server, create new database using SqlClient
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
                //If server type is PostgreSQL, create new database using Npgsql
                NpgsqlConnection connection = new NpgsqlConnection(connectionString);
                NpgsqlCommand command = new NpgsqlCommand($"CREATE DATABASE {databaseName};",connection);
                connection.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
                   
            //At the end return to the databaseDialog
            databaseDialog.Start(tableDialog, tableOptions);

        }
    }
}