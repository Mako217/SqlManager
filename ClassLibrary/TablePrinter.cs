using System;
using System.Data.SqlClient;
using System.Data;
using Npgsql;

namespace ClassLibrary
{
    public static class TablePrinter
    {
       public static void Print(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions)
        {
            DataTable dataTable = new DataTable();
            DataTable columns = new DataTable();
            if(serverType == "MSSQL Server")
            {
                //If server type is MSSQL Server create new SqlConnection and SqlDataAdapter, and get all column names from table
                SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog={databaseDialog.options[whichDatabase]}");
                SqlDataAdapter columnNames = new SqlDataAdapter($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableDialog.options[whichTable]}'", connection);
                connection.Open();
                //Fill DataTable with column names
                columnNames.Fill(columns);
                columnNames.Dispose();
                //Get all data from the table
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {tableDialog.options[whichTable]}", connection);
                //Fill DataTable with all data
                adapter.Fill(dataTable);
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
                
            }
            
            else if(serverType == "PostgreSQL")
            {
                //If server type is PostgreSQL create new NpgsqlConnection and NpgsqlDataAdapter, and get all column names from table
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database={databaseDialog.options[whichDatabase]}");
                NpgsqlDataAdapter columnNames = new NpgsqlDataAdapter($"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableDialog.options[whichTable]}'", connection);
                connection.Open();
                //Fill DataTable with column names
                columnNames.Fill(columns);
                columnNames.Dispose();
                //Get all data from the table
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter($"SELECT * FROM {tableDialog.options[whichTable]}", connection);
                //Fill DataTable with all data
                adapter.Fill(dataTable);
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
                
            }
            Console.Clear();
            //Print the table frames and content
            ConsoleColor color = ConsoleColor.DarkGray;
            Console.ForegroundColor = color;
            Console.Write("|");
            Console.ForegroundColor = ConsoleColor.White;
            //Print first table name
            Console.WriteLine($"{columns.Rows[0].ItemArray[0]}");
            Console.SetCursorPosition(0,1);
            Console.ForegroundColor = color;
            Console.WriteLine(new string('=', (columns.Rows.Count)*20));
            //Print every table name
            for(int i=1; i<columns.Rows.Count; i++)
            {
                Console.SetCursorPosition(i*20, 0);
                Console.Write("|");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{columns.Rows[i].ItemArray[0]}");
                Console.ForegroundColor = color;
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
            //Print every item in the table
            for (int i = 0; i < dataTable.Rows.Count; i++)
            { 
                Console.Write("|");
                DataRow row = dataTable.Rows[i];
                for (int j = 1; j <= row.ItemArray.Length; j++)
                {
                    var item = row.ItemArray[j - 1];
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(item);
                    if(item.ToString().Length == 0)
                        {
                            Console.Write("*Empty cell*");
                        }
                    Console.ForegroundColor = color;
                    Console.SetCursorPosition(20 * j, i+2);
                    Console.Write("|");
                    Console.SetCursorPosition(20 *j, i+3);
                    Console.Write("|");
                    Console.SetCursorPosition(20 * j + 1, i+2);
                }
                Console.SetCursorPosition(0, i+3);
            }
            
        
            Console.ForegroundColor = ConsoleColor.White;
            dataTable.Dispose();
            columns.Dispose();
            //Wait for user to press any key
            Console.ReadKey();
            Console.WriteLine("Returning...");
            //Return to the tableOptions
            tableOptions.Start(databaseDialog, whichDatabase, tableDialog, whichTable);
    }
        
         
        public static void PrintCellSelector(DataTable dataTable, DataTable columns, int cellX, int cellY)
        {
            //Print the table frames and content
            ConsoleColor color = ConsoleColor.DarkGray;
            Console.Clear();
            Console.ForegroundColor = color;
            Console.Write("|");
            Console.ForegroundColor = ConsoleColor.White;
            //Print first column name
            Console.WriteLine($"{columns.Rows[0].ItemArray[0]}");
            Console.SetCursorPosition(0,1);
            Console.ForegroundColor = color;
            Console.WriteLine(new string('=', columns.Rows.Count*20));
            //Print every console name
            for(int i=1; i<columns.Rows.Count; i++)
            {
                Console.SetCursorPosition(i*20, 0);
                Console.Write("|");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{columns.Rows[i].ItemArray[0]}");
                Console.ForegroundColor = color;
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
            //Print every item in the table
            for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Console.Write("|");
                    DataRow row = dataTable.Rows[i];
                    Console.ForegroundColor = ConsoleColor.White;
                    for (int j = 1; j <= row.ItemArray.Length; j++)
                    {  
                        if(i == cellY && j - 1 == cellX)
                        {
                            //Highlight currently selected cell with color blue
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        var item = row.ItemArray[j - 1];
                        if(Console.ForegroundColor != ConsoleColor.Blue)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.Write(item);
                        if(item.ToString().Length == 0)
                        {
                            Console.Write("*Empty cell*");
                        }
                        Console.ForegroundColor = color;
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
                //Print controls for the user
                Console.WriteLine("Use arrows to move around the table | Enter - select cell | Escape - return");
                Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintColumnSelector(DataTable dataTable, DataTable columns, int whichColumn)
        {
            //Print table frame and contents
            ConsoleColor color = ConsoleColor.DarkGray;
            Console.Clear();
            Console.ForegroundColor = color;
            Console.Write("|");
            Console.ForegroundColor = ConsoleColor.White;
            if(whichColumn == 0)
            {
                //Highlight currently selected column with color blue
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            //Print first column name
            Console.WriteLine($"{columns.Rows[0].ItemArray[0]}");
            Console.SetCursorPosition(0,1);
            Console.ForegroundColor = color;
            Console.WriteLine(new string('=', columns.Rows.Count*20));
            //Print every column name
            for(int i=1; i<columns.Rows.Count; i++)
            {
                Console.SetCursorPosition(i*20, 0);
                Console.Write("|");
                Console.ForegroundColor = ConsoleColor.White;
                if(i == whichColumn)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                Console.Write($"{columns.Rows[i].ItemArray[0]}");
                Console.ForegroundColor = color;
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
            //Print every item in the table
            for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Console.Write("|");
                    DataRow row = dataTable.Rows[i];
                    Console.ForegroundColor = ConsoleColor.White;
                    for (int j = 1; j <= row.ItemArray.Length; j++)
                    {  
                        if(j - 1 == whichColumn)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        var item = row.ItemArray[j - 1];
                        if(Console.ForegroundColor != ConsoleColor.Blue)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.Write(item);
                        if(item.ToString().Length == 0)
                        {
                            Console.Write("*Empty cell*");
                        }
                        Console.ForegroundColor = color;
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
                //Print controls for the user
                Console.WriteLine("Use arrows to move around the table | Enter - select column | Escape - return");
                Console.ForegroundColor = ConsoleColor.White;
        }
        public static void PrintRowSelector(DataTable dataTable, DataTable columns, int whichRow)
        {
            //Print the table frame and content
            ConsoleColor color = ConsoleColor.DarkGray;
            Console.Clear();
            Console.ForegroundColor = color;
            Console.Write("|");
            Console.ForegroundColor = ConsoleColor.White;
            //Print first column name
            Console.WriteLine($"{columns.Rows[0].ItemArray[0]}");
            Console.SetCursorPosition(0,1);
            Console.ForegroundColor = color;
            Console.WriteLine(new string('=', columns.Rows.Count*20));
            //Print every column name
            for(int i=1; i<columns.Rows.Count; i++)
            {
                Console.SetCursorPosition(i*20, 0);
                Console.Write("|");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{columns.Rows[i].ItemArray[0]}");
                Console.ForegroundColor = color;
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
            //Print every item in the table
            for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    Console.Write("|");
                    DataRow row = dataTable.Rows[i];
                    Console.ForegroundColor = ConsoleColor.White;
                    for (int j = 1; j <= row.ItemArray.Length; j++)
                    {  
                        if(i == whichRow)
                        {
                            //Highlight currently selected row with the color blue
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        var item = row.ItemArray[j - 1];
                        if(Console.ForegroundColor != ConsoleColor.Blue)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.Write(item);
                        if(item.ToString().Length == 0)
                        {
                            Console.Write("*Empty cell*");
                        }
                        Console.ForegroundColor = color;
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
                //Print the controls for the user
                Console.WriteLine("Use arrows to move around the table | Enter - select row | Escape - return");
                Console.ForegroundColor = ConsoleColor.White;
        }
    }
}