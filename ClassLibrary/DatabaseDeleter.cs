using System;
using System.Data.SqlClient;
using Npgsql;

namespace ClassLibrary
{
    public static class DatabaseDeleter
    {
        public static void Delete(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, TableOptions tableOptions)
        {
            //Ask user for confirmation to delete database
            Console.Clear();
            Console.WriteLine("Are you sure, you want to delete this database? [Type Y for Yes, or N for No, N is default]");
            string input = Console.ReadLine();
            if(input == "Y")
            {
                
            }
            else if(input == "N" || input == "")
            {
                //If user inputs N, or leaves blank space return to the tableDialog
                tableDialog.Start(databaseDialog, whichDatabase, tableOptions);
            }
            else
            {
                //If the user inputs invalid data, start all over.
                DatabaseDeleter.Delete(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, tableOptions);
            }

            if(serverType == "MSSQL Server")
            {
                //If server type is MSSQL Server create SqlConnection and SqlCommand
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                //Terminate all connections to the database, then delete the database
                SqlCommand command = new SqlCommand($"Use master alter database {databaseDialog.options[whichDatabase]} set single_user with rollback immediate Drop database {databaseDialog.options[whichDatabase]}", connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            else if(serverType == "PostgreSQL")
            {
                //If server type is PostgreSQL Server create NpgsqlConnection and NpgsqlCommand
                NpgsqlConnection connection = new NpgsqlConnection(connectionString);
                connection.Open();
                //Terminate all connections to the database, then delete the database.
                NpgsqlCommand command = new NpgsqlCommand($"SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '{databaseDialog.options[whichDatabase]}'; Drop database {databaseDialog.options[whichDatabase]}", connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            //At the end return to databaseDialog,
            databaseDialog.Start(tableDialog, tableOptions);
        }
    }
}