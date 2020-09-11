using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Npgsql;

namespace ClassLibrary
{
    public static class RowAdder
    {
         public static void Add(string connectionString, string serverType, DatabaseDialog databaseDialog, int whichDatabase, TableDialog tableDialog, int whichTable, TableOptions tableOptions)
        {
            DataTable dataTable = new DataTable();
            if(serverType == "MSSQL Server")
            {
                //If server type is MSSQL Server fill dataTable with column names and data types using SqlClient
                SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog={databaseDialog.options[whichDatabase]}");
                SqlDataAdapter adapter = new SqlDataAdapter($"Select Column_Name, Data_Type From INFORMATION_SCHEMA.COLUMNS where Table_Name = '{tableDialog.options[whichTable]}' ", connection);
                connection.Open();
                adapter.Fill(dataTable);
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
            }
            else if(serverType == "PostgreSQL")
            {
                //If server type is PostgreSQL fill dataTable with column names and data types using Npgsql
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database={databaseDialog.options[whichDatabase]}");
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter($"Select Column_Name, Data_Type From INFORMATION_SCHEMA.COLUMNS where Table_Name = '{tableDialog.options[whichTable]}' ", connection);
                connection.Open();
                adapter.Fill(dataTable);
                adapter.Dispose();
                connection.Close();
                connection.Dispose();
            }
            List<string> columnList = new List<string>();
            string columns = "";
            string values = "";
            for(int i = 0; i<dataTable.Rows.Count; i++)
            {
                Console.Clear();
                DataRow row = dataTable.Rows[i];
                //Create string containing column names
                columns += (string)row.ItemArray[0] + ", ";
                //Ask user for new value in column
                Console.WriteLine("Add " + (string)row.ItemArray[0] + " value:");
                //Create string containing values
                if((string)row.ItemArray[1] == "varchar" || (string)row.ItemArray[1] == "character varying")
                {
                    values += "'" + Console.ReadLine() + "', ";
                }
                else if((string)row.ItemArray[1] == "int" || (string)row.ItemArray[1] == "integer")
                {
                    values += Console.ReadLine() + ", ";
                }
            }
            //Remove last commas from columns and values string
            columns = columns.Substring(0, columns.Length - 2);
            values = values.Substring(0, values.Length - 2);
            dataTable.Dispose();
            if(serverType == "MSSQL Server")
            {
                //If server type is MSSQL Server add new row using SqlClient
                SqlConnection connection = new SqlConnection(connectionString + $"Initial Catalog={databaseDialog.options[whichDatabase]}");
                SqlCommand command = new SqlCommand($"INSERT INTO [{tableDialog.options[whichTable]}]({columns}) VALUES({values})", connection);
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine();
                    Console.ReadLine();
                }
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
            else if(serverType == "PostgreSQL")
            {
                //If server type is PostgreSQL add new row using Npgsql
                NpgsqlConnection connection = new NpgsqlConnection(connectionString + $"Database={databaseDialog.options[whichDatabase]}");
                NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO {tableDialog.options[whichTable]} VALUES ({values})", connection);
                connection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine();
                    Console.ReadLine();
                }
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }

            //At the end return to the tableOptions
            tableOptions.Start(databaseDialog, whichDatabase, tableDialog, whichTable);

        }
    }
}