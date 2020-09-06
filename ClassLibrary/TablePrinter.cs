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
            SqlDataAdapter columnNames = new SqlDataAdapter($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableDialog.options[whichTable]}'", connection);
            connection.Open();
            DataTable columns = new DataTable();
            columnNames.Fill(columns);
            columnNames.Dispose();
            using (SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {tableDialog.options[whichTable]}", connection))
            {
                Console.Clear();
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                Console.WriteLine($"|{columns.Rows[0].ItemArray[0]}");
                Console.SetCursorPosition(0,1);
                Console.WriteLine(new string('=', (columns.Rows.Count)*20));
                for(int i=1; i<columns.Rows.Count; i++)
                {
                    Console.SetCursorPosition(i*20, 0);
                    Console.Write($"|{columns.Rows[i].ItemArray[0]}");
                    Console.SetCursorPosition(i*20, 1);
                    Console.Write("|");
                }
                Console.SetCursorPosition(columns.Rows.Count * 20, 0);
                Console.Write("|");
                Console.SetCursorPosition(columns.Rows.Count * 20, 1);
                Console.Write("|");
                Console.SetCursorPosition(0, dataTable.Rows.Count + 2);
                Console.WriteLine(new string('=', (columns.Rows.Count)*20));
                Console.SetCursorPosition(0,1);
                Console.Write("|");
                Console.SetCursorPosition(0, dataTable.Rows.Count+2);
                Console.Write("|");
                Console.SetCursorPosition(0, 2);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                { 
                    Console.Write("|");
                    DataRow row = dataTable.Rows[i];
                    for (int j = 1; j <= row.ItemArray.Length; j++)
                    {

                        var item = row.ItemArray[j - 1];
                        
                        Console.Write(item);
                        Console.SetCursorPosition(20 * j, i+2);
                        Console.Write("|");
                        Console.SetCursorPosition(20 *j, i+3);
                        Console.Write("|");
                        Console.SetCursorPosition(20 * j + 1, i+2);
                    }
                    Console.SetCursorPosition(0, i+3);
                }
                dataTable.Dispose();
                columns.Dispose();
            }
            connection.Close();
            connection.Dispose();
            Console.ReadLine();
            tableOptions.Start(databaseDialog, whichDatabase, tableDialog, whichTable);
        }
         
        public static void PrintEditor(DataTable dataTable, DataTable columns, int cellX, int cellY)
        {
            Console.Clear();
            Console.WriteLine($"|{columns.Rows[0].ItemArray[0]}");
            Console.SetCursorPosition(0,1);
            Console.WriteLine(new string('=', columns.Rows.Count*20));
            for(int i=1; i<columns.Rows.Count; i++)
            {
                Console.SetCursorPosition(i*20, 0);
                Console.Write($"|{columns.Rows[i].ItemArray[0]}");
                Console.SetCursorPosition(i*20, 1);
                Console.Write("|");
            }
            Console.SetCursorPosition(columns.Rows.Count * 20, 0);
            Console.Write("|");
            Console.SetCursorPosition(columns.Rows.Count * 20, 1);
            Console.Write("|");
            Console.SetCursorPosition(0, dataTable.Rows.Count + 2);
            Console.WriteLine(new string('=', columns.Rows.Count*20));
            Console.SetCursorPosition(0,1);
            Console.Write("|");
            Console.SetCursorPosition(0, dataTable.Rows.Count+2);
            Console.Write("|");
            Console.SetCursorPosition(0, 2);
            
            for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Console.Write("|");
                    DataRow row = dataTable.Rows[i];
                    for (int j = 1; j <= row.ItemArray.Length; j++)
                    {  
                        if(i == cellY && j - 1 == cellX)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        var item = row.ItemArray[j - 1];
                        Console.Write(item);
                        if(Console.ForegroundColor == ConsoleColor.Blue)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.SetCursorPosition(20 * j, i);
                        Console.Write("|");
                        Console.SetCursorPosition(20 *j, i+3);
                        Console.Write("|");
                        Console.SetCursorPosition(20 * j + 1, i+2);
                    }
                    Console.SetCursorPosition(0, i+3);
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(0, dataTable.Rows.Count + 4);
                Console.WriteLine("Use arrows to move around the table | Enter - select cell | Escape - return");
                Console.ForegroundColor = ConsoleColor.White;
        }
    }
}