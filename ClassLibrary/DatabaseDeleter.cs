using System.Data.SqlClient;

namespace ClassLibrary
{
    public static class DatabaseDeleter
    {
        public static void Delete(string connectionString, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, TableOptions tableOptions)
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
    }
}