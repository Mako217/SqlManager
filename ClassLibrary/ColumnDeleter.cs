using System;
using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace ClassLibrary
{
    public static class ColumnDeleter
    {
        public static void SelectColumn(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions)
        {
            DataTable dataTable = new DataTable();
            DataTable columns = new DataTable();
            if(serverType == "MSSQL Server")
            {   
                //If server type is MSSQL Server fill one DataTable with all table contents and second with column names and data types using SqlClient
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
                //If server type is PostgreSQL fill one DataTable with all table contents and second with column names and data types using Npgsql
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database={databaseDialog.options[whichDatabase]}");  

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter($"SELECT * FROM {tableDialog.options[whichTable]}", connection);
                connection.Open();
                adapter.Fill(dataTable);

                NpgsqlDataAdapter columnNames = new NpgsqlDataAdapter($"Select column_name, data_type from information_schema.columns where table_name = '{tableDialog.options[whichTable]}'", connection);
                columnNames.Fill(columns);
                connection.Close();
            }
            //Set selected columns at 0
            int whichColumn = 0;
            bool inside = true;
            //Print table with highlighted column 0
            TablePrinter.PrintColumnSelector(dataTable, columns, whichColumn);
            while(inside)
            {
                //Wait for user input
               if(Console.KeyAvailable)
               {
                   ConsoleKeyInfo input = Console.ReadKey();
                   switch(input.Key)
                   {
                        case ConsoleKey.LeftArrow:
                        //If user input is Left Arrow, and selected column is bigger than 0 
                        //decrease whichColumn by 1 and print table with highlighted current column
                        if(whichColumn>0)
                        {
                            whichColumn--;
                        }
                        TablePrinter.PrintColumnSelector(dataTable, columns, whichColumn);
                        break;
                        case ConsoleKey.RightArrow:
                        //If user input is Right Arrow, and selected column is smaller than column amount - 1 
                        //increase whichColumn by 1 and print table with highlighted current column
                        if(whichColumn < dataTable.Rows[0].ItemArray.Length - 1)
                        {
                            whichColumn++;
                        }
                        TablePrinter.PrintColumnSelector(dataTable, columns, whichColumn);
                        break;
                        case ConsoleKey.Escape:
                        //If user input is Escape return to tableOptions
                        Console.Clear();
                        Console.WriteLine("Returning...");
                        tableOptions.Start(databaseDialog, whichDatabase, tableDialog, whichTable);
                        break;
                        case ConsoleKey.Enter:
                        //If user input is Enter leave loop
                        inside = false;
                        break;
                   }
               }
            }
            //Start deleting currently selected column
            ColumnDeleter.Delete(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions, columns, whichColumn);

            //Return to column selecting
            ColumnDeleter.SelectColumn(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions);


        }

        private static void Delete(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions, DataTable columns, int whichColumn)
        {
            Console.Clear();
            //Ask user for confirmation to delete
            Console.WriteLine("Are you sure, you want to delete this column? [Type Y for Yes, or N for No, N is default]");
            string input = Console.ReadLine();
            if(input == "Y")
            {
                
            }
            else if(input == "N" || input == "")
            {
                //If user passes N or blank space, return to table options
                tableOptions.Start(databaseDialog, whichDatabase, tableDialog, whichTable);
            }
            else
            {
                //If user passes incorrect input, ask again
                ColumnDeleter.Delete(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions, columns, whichColumn);
            }

            if(serverType == "MSSQL Server")
            {
                //If serverType is MSSQL Server, delete column using SqlClient
                SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog = {databaseDialog.options[whichDatabase]}");
                SqlCommand command = new SqlCommand($"Alter table {tableDialog.options[whichTable]} drop column {columns.Rows[whichColumn].ItemArray[0]};", connection);
                connection.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            else if(serverType == "PostgreSQL")
            {
                //If serverType is PostgreSQL, delete column using Npgsql
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database = {databaseDialog.options[whichDatabase]}");
                NpgsqlCommand command = new NpgsqlCommand($"Alter table {tableDialog.options[whichTable]} drop column {columns.Rows[whichColumn].ItemArray[0]};", connection);
                connection.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }

        }
        
    }
}