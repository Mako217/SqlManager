using System;
using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace ClassLibrary
{
    public static class RowDeleter
    {
        public static void SelectRow(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions)
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
                //If server type is PostgreSQL fill dataTable with column names and data types using Npgsql
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database={databaseDialog.options[whichDatabase]}");  

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter($"SELECT * FROM {tableDialog.options[whichTable]}", connection);
                connection.Open();
                adapter.Fill(dataTable);

                NpgsqlDataAdapter columnNames = new NpgsqlDataAdapter($"Select column_name, data_type from information_schema.columns where table_name = '{tableDialog.options[whichTable]}'", connection);
                columnNames.Fill(columns);
                connection.Close();
            }
            //Set currently selected row to 0
            int whichRow = 0;
            bool inside = true;
            //Print table with row 0 highlighted
            TablePrinter.PrintRowSelector(dataTable, columns, whichRow);
            while(inside)
            {
               if(Console.KeyAvailable)
               {
                   ConsoleKeyInfo input = Console.ReadKey();
                   switch(input.Key)
                   {
                        case ConsoleKey.UpArrow:
                        if(whichRow>0)
                        {
                            whichRow--;
                        }
                        TablePrinter.PrintRowSelector(dataTable, columns, whichRow);
                        break;
                        case ConsoleKey.DownArrow:
                        if(whichRow < dataTable.Rows.Count -1)
                        {
                            whichRow++;
                        }
                        TablePrinter.PrintRowSelector(dataTable, columns, whichRow);
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
            RowDeleter.Delete(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions, dataTable, columns, whichRow);

            RowDeleter.SelectRow(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions);
        }
        private static void Delete(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions, DataTable dataTable, DataTable columns, int whichRow)
        {
             Console.Clear();
            Console.WriteLine("Are you sure, you want to delete this row? [Type Y for Yes, or N for No, N is default]");
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
                RowDeleter.Delete(connectionString, serverType, databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions, dataTable, columns, whichRow);
            }

            string deleteString = "";
            for(int i = 0; i<columns.Rows.Count; i++)
            {
                deleteString += $"{columns.Rows[i].ItemArray[0]} = ";
                if((string)columns.Rows[i].ItemArray[1] == "varchar")
                {
                    deleteString += $"'{dataTable.Rows[whichRow].ItemArray[i]}'AND ";
                }
                else if((string)columns.Rows[i].ItemArray[1] == "int" || (string)columns.Rows[i].ItemArray[1] == "integer")
                {
                    deleteString += $"{dataTable.Rows[whichRow].ItemArray[i]} AND ";
                }
            }

            deleteString = deleteString.Substring(0, deleteString.Length-5);
            if(serverType == "MSSQL Server")
            {
                SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog = {databaseDialog.options[whichDatabase]};");
                SqlCommand command = new SqlCommand($"Delete from {tableDialog.options[whichTable]} where {deleteString};", connection);
                connection.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            else if(serverType == "PostgreSQL")
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database = {databaseDialog.options[whichDatabase]};");
                NpgsqlCommand command = new NpgsqlCommand($"Delete from {tableDialog.options[whichTable]} where {deleteString};", connection);
                connection.Open();
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }


        }
    }


}