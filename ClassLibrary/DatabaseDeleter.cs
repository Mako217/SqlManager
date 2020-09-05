using System;
using System.Data.SqlClient;

namespace ClassLibrary
{
    public static class DatabaseDeleter
    {
        public static void Delete(string connectionString, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, TableOptions tableOptions)
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
                DatabaseDeleter.Delete(connectionString, databaseDialog, whichDatabase, tableDialog, tableOptions);
            }


            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = new SqlCommand($"Use master alter database {databaseDialog.options[whichDatabase]} set single_user with rollback immediate Drop database {databaseDialog.options[whichDatabase]}", connection);
            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            databaseDialog.Start(tableDialog, tableOptions);
        }
    }
}