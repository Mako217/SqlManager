using System;
using System.Data.SqlClient;
using Npgsql;

namespace ClassLibrary
{
    public static class TableAdder
    {
        public static void Add(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, TableOptions tableOptions)
        {
            Console.Clear();
            Console.WriteLine("Enter table name:");
            string tableName = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Enter amount of columns in the table:");
            int howManyColumns = 0;
            try
            {
                howManyColumns = Int32.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Invalid amount of columns");
                TableAdder.Add(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, tableOptions);
            }
            string columnString = "";
            

            if(serverType == "MSSQL")
            {
                for(int i = 1; i<=howManyColumns; i++)
                {
                    Console.Clear();
                    Console.WriteLine($"Column {i} name:");
                    string newColumn = Console.ReadLine();
                    columnString += $"[{newColumn}] ";
                    Console.WriteLine($"Column {i} datatype:");
                    columnString += $"{Console.ReadLine()}, ";
                }
                columnString = columnString.Substring(0, columnString.Length - 2);
                SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog={databaseDialog.options[whichDatabase]};");
                connection.Open();
                SqlCommand command = new SqlCommand($"CREATE Table {tableName}({columnString})", connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
                tableDialog.Start(databaseDialog, whichDatabase, tableOptions);
            }
            else if(serverType == "PostgreSQL")
            {
                for(int i = 1; i<=howManyColumns; i++)
                {
                    Console.Clear();
                    Console.WriteLine($"Column {i} name:");
                    string newColumn = Console.ReadLine();
                    columnString += $"{newColumn} ";
                    Console.WriteLine($"Column {i} datatype:");
                    columnString += $"{Console.ReadLine()}, ";
                }
                columnString = columnString.Substring(0, columnString.Length - 2);
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database={databaseDialog.options[whichDatabase]};");
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand($"CREATE Table {tableName} ({columnString})", connection);

                Console.WriteLine($"CREATE Table {tableName} ({columnString})", connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
                tableDialog.Start(databaseDialog, whichDatabase, tableOptions);
            }
        }
    }
}