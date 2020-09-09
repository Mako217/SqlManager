using System;
using System.Data.SqlClient;
using Npgsql;

namespace ClassLibrary
{
    public static class TableAdder
    {
        public static void Add(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, TableOptions tableOptions)
        {
            //Ask user for new table name, and amount of columns inside.
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
                //If server type is MSSQL Server, create string of column names, and their data types, putting column names in brackets.
                for(int i = 1; i<=howManyColumns; i++)
                {
                    Console.Clear();
                    Console.WriteLine($"Column {i} name:");
                    string newColumn = Console.ReadLine();
                    columnString += $"[{newColumn}] ";
                    Console.WriteLine($"Column {i} datatype:");
                    columnString += $"{Console.ReadLine()}, ";
                }
                //Remove last space and comma from column string
                columnString = columnString.Substring(0, columnString.Length - 2);
                //Create new SqlConnection adding Initial Catalog value to the connection string
                SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog={databaseDialog.options[whichDatabase]};");
                connection.Open();
                //Create new table using gathered data
                SqlCommand command = new SqlCommand($"CREATE Table {tableName}({columnString})", connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            else if(serverType == "PostgreSQL")
            {
                //If server type is PostgreSQL, create string of column names, and their data types.
                for(int i = 1; i<=howManyColumns; i++)
                {
                    Console.Clear();
                    Console.WriteLine($"Column {i} name:");
                    string newColumn = Console.ReadLine();
                    columnString += $"{newColumn} ";
                    Console.WriteLine($"Column {i} datatype:");
                    columnString += $"{Console.ReadLine()}, ";
                }
                //Remove last space and comma from the column string.
                columnString = columnString.Substring(0, columnString.Length - 2);
                //Create new NpgsqlConnection adding Database value to the connection string
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database={databaseDialog.options[whichDatabase]};");
                connection.Open();
                //Create new table using gathered data
                NpgsqlCommand command = new NpgsqlCommand($"CREATE Table {tableName} ({columnString})", connection);

                Console.WriteLine($"CREATE Table {tableName} ({columnString})", connection);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            //At the end return to the tableDialog
            tableDialog.Start(databaseDialog, whichDatabase, tableOptions);
        }
    }
}