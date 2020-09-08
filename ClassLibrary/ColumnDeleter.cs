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
            int whichColumn = 0;
            bool inside = true;
            TablePrinter.PrintColumnSelector(dataTable, columns, whichColumn);
            while(inside)
            {
               if(Console.KeyAvailable)
               {
                   ConsoleKeyInfo input = Console.ReadKey();
                   switch(input.Key)
                   {
                        case ConsoleKey.LeftArrow:
                        if(whichColumn>0)
                        {
                            whichColumn--;
                        }
                        TablePrinter.PrintColumnSelector(dataTable, columns, whichColumn);
                        break;
                        case ConsoleKey.RightArrow:
                        if(whichColumn < dataTable.Rows[0].ItemArray.Length - 1)
                        {
                            whichColumn++;
                        }
                        TablePrinter.PrintColumnSelector(dataTable, columns, whichColumn);
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
            ColumnDeleter.Delete(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions, columns, whichColumn);

            ColumnDeleter.SelectColumn(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions);


        }

        private static void Delete(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions, DataTable columns, int whichColumn)
        {
            Console.Clear();
            Console.WriteLine("Are you sure, you want to delete this column? [Type Y for Yes, or N for No, N is default]");
            string input = Console.ReadLine();
            if(input == "Y")
            {
                
            }
            else if(input == "N" || input == "")
            {
                tableOptions.Start(databaseDialog, whichDatabase, tableDialog, whichTable);
            }
            else
            {
                ColumnDeleter.Delete(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions, columns, whichColumn);
            }

            if(serverType == "MSSQL Server")
            {
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