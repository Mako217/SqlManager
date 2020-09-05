using System;
using System.Data;
using System.Data.SqlClient;

namespace ClassLibrary
{
    public static class TableEditor
    {


        public static void Control(string connectionString, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions)
        {
            
            SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog={databaseDialog.options[whichDatabase]}");
            DataTable dataTable = new DataTable();

            using (connection)
            using (SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {tableDialog.options[whichTable]}", connection))
            {
                Console.Clear();
                connection.Open();
                adapter.Fill(dataTable);
               
            }
            int cellY = 0;
            int cellX = 0;
            bool inside = true;
            TablePrinter.PrintEditor(dataTable, cellX, cellY);
            while(inside)
            {
                if(Console.KeyAvailable)
                {
                    ConsoleKeyInfo input = Console.ReadKey();
                    switch(input.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            if(cellX>0)
                            {
                                cellX--;
                            }
                            TablePrinter.PrintEditor(dataTable, cellX, cellY);
                            break;
                        case ConsoleKey.RightArrow:
                            if(cellX<dataTable.Rows[0].ItemArray.Length - 1)
                            {
                                cellX++;
                            }
                            TablePrinter.PrintEditor(dataTable, cellX, cellY);
                            break;
                        case ConsoleKey.UpArrow:
                            if(cellY>0)
                            {
                                cellY--;
                            }
                            TablePrinter.PrintEditor(dataTable, cellX, cellY);
                            break;
                        case ConsoleKey.DownArrow:
                            if(cellY<dataTable.Rows.Count - 1)
                            {
                                cellY++;
                            }
                            TablePrinter.PrintEditor(dataTable, cellX, cellY);
                            break;
                        case ConsoleKey.Escape:
                            Console.Clear();
                            Console.WriteLine("Returning...");
                            tableOptions.Start(databaseDialog, whichDatabase, tableDialog, whichTable);
                            break;
                        case ConsoleKey.Enter:
                            inside = false;
                            break;        
                    }
                }
            }
            Edit(connectionString, databaseDialog, whichDatabase, tableDialog, whichTable, dataTable, cellX, cellY);

            Control(connectionString, databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions);
        } 

        public static void Edit(string connectionString,DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, DataTable dataTable, int cellX, int cellY)
        {
            SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog = {databaseDialog.options[whichDatabase]}");
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter($"Select column_name, data_type from information_schema.columns where table_name = '{tableDialog.options[whichTable]}'", connection);
            DataTable columns = new DataTable();
            adapter.Fill(columns);
            adapter.Dispose();
            string set = "";
            string columnString = "";
            Console.Clear();
            Console.WriteLine("Enter new value:");
            string value = Console.ReadLine();

            for(int i = 0; i<dataTable.Rows[0].ItemArray.Length; i++)
            {
                if((string)columns.Rows[i].ItemArray[1] == "varchar")
                {
                columnString += $"{columns.Rows[i].ItemArray[0]} = '{dataTable.Rows[cellY].ItemArray[i]}' AND ";
                }
                else if((string)columns.Rows[i].ItemArray[1] == "int")
                {
                columnString += $"{columns.Rows[i].ItemArray[0]} = {dataTable.Rows[cellY].ItemArray[i]} AND ";    
                }
                if(i == cellX)
                {
                    if((string)columns.Rows[i].ItemArray[1] == "varchar")
                    {
                    set += $"{columns.Rows[i].ItemArray[0]} = '{value}'";
                    }
                    else if((string)columns.Rows[i].ItemArray[1] == "int")
                    {
                    set += $"{columns.Rows[i].ItemArray[0]} = {value}";    
                    }
                }
            }
            columnString = columnString.Substring(0, columnString.Length-5);
            SqlCommand command = new SqlCommand($"Update {tableDialog.options[whichTable]} set {set} where {columnString}", connection);
            command.ExecuteNonQuery();
            connection.Close();
            command.Dispose();
            connection.Dispose();
        }
    }
}