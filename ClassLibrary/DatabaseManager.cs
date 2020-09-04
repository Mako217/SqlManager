using System;
using System.Data.SqlClient;

namespace ClassLibrary
{
    public static class DatabaseManager
    {
        public static void AddNew(string connectionString)
        {
            Console.WriteLine("Enter new database name:");
            string databaseName = Console.ReadLine();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand($"CREATE DATABASE {databaseName};",connection);
            connection.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            connection.Dispose();
        }
    }
}