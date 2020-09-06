using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace ClassLibrary
{
    public static class TableAdder
    {
        public static void Add(string connectionString, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, TableOptions tableOptions)
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
                TableAdder.Add(connectionString, databaseDialog, whichDatabase, tableDialog, tableOptions);
            }
            string columnString = "";
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
    }
}