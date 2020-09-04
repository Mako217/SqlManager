using System.Data.SqlClient;

namespace ClassLibrary
{
    public static class TableDeleter
    {
        public static void Delete(string connectionString, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions)
        {
            SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog ={databaseDialog.options[whichDatabase]};");
            connection.Open();
            SqlCommand command = new SqlCommand($"Drop table {tableDialog.options[whichTable]}", connection);
            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            connection.Dispose();
            tableDialog.Start(databaseDialog, whichDatabase, tableOptions);
        }
    }
}