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
                //If server type is MSSQL Server fill one DataTable with all the table content and the other one with column names, and data types using SqlClient
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
                //If server type is PostgreSQL fill one DataTable with all the table content and the other one with column names, and data types using Npgsql
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database={databaseDialog.options[whichDatabase]}");  

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter($"SELECT * FROM {tableDialog.options[whichTable]}", connection);
                connection.Open();
                adapter.Fill(dataTable);

                NpgsqlDataAdapter columnNames = new NpgsqlDataAdapter($"Select column_name, data_type from information_schema.columns where table_name = '{tableDialog.options[whichTable]}'", connection);
                columnNames.Fill(columns);
                connection.Close();
            }
            //Set currently selected cell coordinates as 0,0
            int cellY = 0;
            int cellX = 0;
            bool inside = true;
            //Print table with highlighting currently selected cell
            TablePrinter.PrintCellSelector(dataTable, columns,  cellX, cellY);
            while(inside)
            {
                //Wait for user input
                if(Console.KeyAvailable)
                {
                    ConsoleKeyInfo input = Console.ReadKey();
                    switch(input.Key)
                    {
                        case ConsoleKey.LeftArrow:
                        //If user input is left arrow, and currently selected cell's X coordinate is bigger than 0
                        //decrease X coordinate by one, and print table again
                            if(cellX>0)
                            {
                                cellX--;
                                TablePrinter.PrintCellSelector(dataTable, columns, cellX, cellY);
                            }
                            break;
                        case ConsoleKey.RightArrow:
                        //If user input is Right arrow, and currently selected cell's X coordinate is smaller than amount of columns -1,
                        //increase X coordinate by one, and print table again
                            if(cellX<dataTable.Rows[0].ItemArray.Length - 1)
                            {
                                cellX++;
                                TablePrinter.PrintCellSelector(dataTable, columns, cellX, cellY);
                            }
                            break;
                        case ConsoleKey.UpArrow:
                        //If user input is Up arrow, and currently selected cell's Y coordinate is bigger than 0
                        //decrease Y coordinate by one, and print table again
                            if(cellY>0)
                            {
                                cellY--;
                                TablePrinter.PrintCellSelector(dataTable, columns, cellX, cellY);
                            }
                            break;
                        case ConsoleKey.DownArrow:
                        //If user input is Down arrow, and currently selected cell's Y coordinate is smaller than amount of rows -1,
                        //increase Y coordinate by one, and print table again
                            if(cellY<dataTable.Rows.Count - 1)
                            {
                                cellY++;
                                TablePrinter.PrintCellSelector(dataTable, columns, cellX, cellY);
                            }
                            break;
                        case ConsoleKey.Escape:
                        //If user input is Escape, return to tableOptions
                            Console.Clear();
                            Console.WriteLine("Returning...");
                            tableOptions.Start(databaseDialog, whichDatabase, tableDialog, whichTable);
                            break;
                        case ConsoleKey.Enter:
                        //If user input is enter get out of the loop
                            inside = false;
                            break;        
                    }

                }
            }
            //Start editing cell
            Edit(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, whichTable, dataTable, columns, cellX, cellY);

            //Return to selecting the cell
            SelectCell(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions);
        } 

        private static void Edit(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, DataTable dataTable, DataTable columns, int cellX, int cellY)
        {
            //Ask user fot new value
            string set = "";
            string columnString = "";
            Console.Clear();
            Console.WriteLine("Enter new value:");
            string value = Console.ReadLine();

            for(int i = 0; i<dataTable.Rows[0].ItemArray.Length; i++)
            {
                //Add to columnString "ColumnName = Value AND "
                if((string)columns.Rows[i].ItemArray[1] == "varchar")
                {
                columnString += $"{columns.Rows[i].ItemArray[0]} = '{dataTable.Rows[cellY].ItemArray[i]}' AND ";
                }
                else if((string)columns.Rows[i].ItemArray[1] == "integer")
                {
                columnString += $"{columns.Rows[i].ItemArray[0]} = {dataTable.Rows[cellY].ItemArray[i]} AND ";    
                }
                //Add to set string "ColumnName = Value"
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
            }
            //Delete last " AND " from column string
            columnString = columnString.Substring(0, columnString.Length-5);
            if(serverType == "MSSQL Server")
            {
                //If server type is MSSQL Server update the table using SqlClient
                SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog = {databaseDialog.options[whichDatabase]}");
                connection.Open();  
                SqlCommand command = new SqlCommand($"Update {tableDialog.options[whichTable]} set {set} where {columnString}", connection);
                command.ExecuteNonQuery();
                connection.Close();
                command.Dispose();
                connection.Dispose();
            }
            else if(serverType == "PostgreSQL")
            {
                //If server type is PostgreSQL update the table using Npgsql
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