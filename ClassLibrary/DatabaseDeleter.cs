using System;
using System.Data.SqlClient;
using Npgsql;

namespace ClassLibrary
{
    public static class DatabaseDeleter
    {
        public static void Delete(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, TableOptions tableOptions)
        {
            Console.Clear();
            Console.WriteLine("Are you sure, you want to delete this database? [Type Y for Yes, or N for No, N is default]");
            string input = Console.ReadLine();
            if(input == "Y")
            {
                
            }
            else if(input == "N" || input == "")
            {
                tableDialog.Start(databaseDialog, whichDatabase, tableOptions);
            }
            else
            {
                DatabaseDeleter.Delete(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, tableOptions);
            }

            if(serverType == "MSSQL Server")
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = new SqlCommand($"Use master alter database {databaseDialog.options[whichDatabase]} set single_user with rollback immediate Drop database {databaseDialog.options[whichDatabase]}", connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
                databaseDialog.Start(tableDialog, tableOptions);
            }
            else if(serverType == "PostgreSQL")
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionString);
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{databaseDialog.options[whichDatabase]}'; Drop database {databaseDialog.options[whichDatabase]}", connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
                databaseDialog.Start(tableDialog, tableOptions);
            }
        }
    }
}