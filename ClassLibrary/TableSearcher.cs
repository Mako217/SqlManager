using System;
using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace ClassLibrary
{
    public static class TableSearcher
    {
        public static void Search(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions)
        {
            //Ask user for what to search in the table
            Console.Clear();
            Console.WriteLine("Enter what are you searching for:");
            string searched = Console.ReadLine();
            DataTable dataTable = new DataTable();
            DataTable columns = new DataTable();
            if(serverType == "MSSQL Server")
            {
                //If server type is MSSQL Server, search columns info using SqlClient
                SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog = {databaseDialog.options[whichDatabase]};");
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableDialog.options[whichTable]}';", connection);
                connection.Open();
                adapter.Fill(columns);
                //Create string for SELECT conditions
                string columnString = StringCreator(columns, searched);
                //Search for content in table
                adapter = new SqlDataAdapter($"SELECT * FROM {tableDialog.options[whichTable]} WHERE {columnString};", connection);
                adapter.Fill(dataTable);
                connection.Close();
                connection.Dispose();
                //Print table
                TablePrinter.Print(databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions, dataTable, columns);
            }
            else if(serverType == "PostgreSQL")
            {
                //If server type is PostgreSQL, search columns info using Npgsql
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database = {databaseDialog.options[whichDatabase]};");
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter($"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableDialog.options[whichTable]}';", connection);
                connection.Open();
                adapter.Fill(columns);
                //Create string for SELECT conditions
                string columnString = StringCreator(columns, searched);
                //Search for content in table
                adapter = new NpgsqlDataAdapter($"SELECT * FROM {tableDialog.options[whichTable]} WHERE {columnString};", connection);
                adapter.Fill(dataTable);
                connection.Close();
                connection.Dispose();
                //Print tab;e
                TablePrinter.Print(databaseDialog, whichDatabase, tableDialog, whichTable, tableOptions, dataTable, columns);

            }
        }
        private static string StringCreator(DataTable columns, string searched)
        {
            string result = "";
            int num = 0;
            for(int i = 0; i < columns.Rows.Count; i++)
            {
                if(((string)columns.Rows[i].ItemArray[1] != "int" && (string)columns.Rows[i].ItemArray[1] != "integer") || int.TryParse(searched, out num))
                result += columns.Rows[i].ItemArray[0] + " = ";
                if((string)columns.Rows[i].ItemArray[1] == "varchar" || (string)columns.Rows[i].ItemArray[1] == "character varying")
                {
                    result += $"'{searched}' OR ";
                }
                else if(((string)columns.Rows[i].ItemArray[1] == "int" || (string)columns.Rows[i].ItemArray[1] == "integer") && int.TryParse(searched, out num))
                {
                    result += $"{searched} OR ";
                }
            }
            result = result.Substring(0, result.Length-3);
            return result;
        }
        
    }
}