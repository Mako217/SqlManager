using System;
using System.Data.SqlClient;
using System.Data;

namespace ClassLibrary
{
    public static class TablePrinter
    {
       public static void Print(string connectionString, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions)
        {

            SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog={databaseDialog.options[whichDatabase]}");

            using (connection)
            using (SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {tableDialog.options[whichTable]}", connection))
            {
                Console.Clear();
                connection.Open();
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow row = dataTable.Rows[i];
                    for (int j = 1; j <= row.ItemArray.Length; j++)
                    {
                        var item = row.ItemArray[j - 1];
                        Console.Write(item);
                        Console.SetCursorPosition(20 * j, i);
                        Console.Write("|");
                    }
                    Console.WriteLine();
                }
            }
            Console.ReadLine();
            tableOptions.Start(databaseDialog, whichDatabase, tableDialog, whichTable);
        }
         
        public static void PrintEditor(DataTable dataTable,int cellX, int cellY)
        {
            int i = 0;
            for (i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow row = dataTable.Rows[i];
                    for (int j = 1; j <= row.ItemArray.Length; j++)
                    {
                        if(i == cellY && j - 1 == cellX)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        if(i == 0 && j ==1)
                        {
                            Console.SetCursorPosition(0, 0);
                        }
                        var item = row.ItemArray[j - 1];
                        Console.Write(item);
                        if(Console.ForegroundColor == ConsoleColor.Blue)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.SetCursorPosition(20 * j, i);
                        Console.Write("|");
                    }
                    Console.WriteLine();
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(0, i + 2);
                Console.WriteLine("Use arrows to move around the table | Enter - select cell | Escape - return");
                Console.ForegroundColor = ConsoleColor.White;
        }
    }
}