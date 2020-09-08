using System;
using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace ClassLibrary
{
    public static class TableEditor
    {


        public static void SelectCell(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions)
        {
            DataTable dataTable = new DataTable();
            DataTable columns = new DataTable();
            if(serverType == "MSSQL Server")
            {   
                SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog={databaseDialog.options[whichDatabase]}");  

                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {tableDialog.options[whichTable]}", connection);
                connection.Open();
                adapter.Fill(dataTable);

                SqlDataAdapter columnNames = new SqlDataAdapter($"Select column_name, data_type from information_schema.columns where table_name = '{tableDialog.options[whichTable]}'", connection);
                columnNames.Fill(columns);
                connection.Close();
            }
            else if(serverType == "PostgreSQL")
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database={databaseDialog.options[whichDatabase]}");  

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter($"SELECT * FROM {tableDialog.options[whichTable]}", connection);
                connection.Open();
                adapter.Fill(dataTable);

                NpgsqlDataAdapter columnNames = new NpgsqlDataAdapter($"Select column_name, data_type from information_schema.columns where table_name = '{tableDialog.options[whichTable]}'", connection);
                columnNames.Fill(columns);
                connection.Close();
            }
            int cellY = 0;
            int cellX = 0;
            bool inside = true;
            TablePrinter.PrintCellSelector(dataTable, columns,  cellX, cellY);
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
                                TablePrinter.PrintCellSelector(dataTable, columns, cellX, cellY);
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            if(cellX<dataTable.Rows[0].ItemArray.Length - 1)
                            {
                                cellX++;
                                TablePrinter.PrintCellSelector(dataTable, columns, cellX, cellY);
                            }
                            break;
                        case ConsoleKey.UpArrow:
                            if(cellY>0)
                            {
                                cellY--;
                                TablePrinter.PrintCellSelector(dataTable, columns, cellX, cellY);
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            if(cellY<dataTable.Rows.Count - 1)
                            {
                                cellY++;
                                TablePrinter.PrintCellSelector(dataTable, columns, cellX, cellY);
                            }
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
            Edit(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, whichTable, dataTable, columns, cellX, cellY);

            SelectCell(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions);
        } 

        private static void Edit(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, DataTable dataTable, DataTable columns, int cellX, int cellY)
        {
            
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
                else if((string)columns.Rows[i].ItemArray[1] == "integer")
                {
                columnString += $"{columns.Rows[i].ItemArray[0]} = {dataTable.Rows[cellY].ItemArray[i]} AND ";    
                }
                if(i == cellX)
                {
                    if((string)columns.Rows[i].ItemArray[1] == "varchar")
                    {
                    set += $"{columns.Rows[i].ItemArray[0]} = '{value}'";
                    }
                    else if((string)columns.Rows[i].ItemArray[1] == "integer" || (string)columns.Rows[i].ItemArray[1] == "int")
                    {
                    set += $"{columns.Rows[i].ItemArray[0]} = {value}";    
                    }
                }
                Console.WriteLine(columnString);
            }
            if(serverType == "MSSQL Server")
            {
                SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog = {databaseDialog.options[whichDatabase]}");
                connection.Open();
                columnString = columnString.Substring(0, columnString.Length-5);
                SqlCommand command = new SqlCommand($"Update {tableDialog.options[whichTable]} set {set} where {columnString}", connection);
                command.ExecuteNonQuery();
                connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            else if(serverType == "PostgreSQL")
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database = {databaseDialog.options[whichDatabase]}");
                connection.Open();

                columnString = columnString.Substring(0, columnString.Length-5);
                NpgsqlCommand command = new NpgsqlCommand($"Update {tableDialog.options[whichTable]} set {set} where {columnString}", connection);
                command.ExecuteNonQuery();
                connection.Close();
                command.Dispose();
                connection.Dispose(); 
            }
        }
    }
}